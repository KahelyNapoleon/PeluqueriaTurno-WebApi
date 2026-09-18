using BLL.Result;
using BLL.Services.Interfaces;
using DAL.UnitOfWork.IUnitOfWork;
using DAL.EstadoTurnoEnum;
using DomainLayer.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Mapping;
using Contracts.DTOs.TurnoDTOs;
using Contracts.DTOs.HistorialTurnoDTOs;
using Contracts.DTOs.TurnoServicioDTOs;
using Contracts.DTOs.ServicioDTOs;
using System.Reflection.PortableExecutable;

namespace BLL.Services
{
    public class TurnoService : ITurnoService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<TurnoCreateUpdateDTO> _validatorTurno;
        private readonly IValidator<HistorialTurnoCreateUpdateDTO> _validatorHistorialTurno;
        private readonly IValidator<TurnoServicioCreateUpdateDTO> _validatorTurnoServicio;
        private IMappingService<Turno, TurnoReadDTO, TurnoCreateUpdateDTO> _mapperTurno;
        private IMappingService<HistorialTurno, HistorialTurnoReadDTO, HistorialTurnoCreateUpdateDTO> _mapperHistorialTurno;
        private IMappingService<TurnoServicio, TurnoServicioReadDTO, TurnoServicioCreateUpdateDTO> _mapperTurnoServicio;

        public TurnoService(IUnitOfWork unitOfWork,
            IValidator<TurnoCreateUpdateDTO> validatorTurno,
            IValidator<HistorialTurnoCreateUpdateDTO> validatorHistorialTurno,
            IValidator<TurnoServicioCreateUpdateDTO> validatorTurnoServicio,
            IMappingService<Turno, TurnoReadDTO, TurnoCreateUpdateDTO> mapperTurno,
            IMappingService<HistorialTurno, HistorialTurnoReadDTO, HistorialTurnoCreateUpdateDTO> mapperHistorialTurno,
            IMappingService<TurnoServicio, TurnoServicioReadDTO, TurnoServicioCreateUpdateDTO> mapperTurnoServicio
            )
        {
            _unitOfWork = unitOfWork;
            _validatorTurno = validatorTurno;
            _validatorHistorialTurno = validatorHistorialTurno;
            _validatorTurnoServicio = validatorTurnoServicio;
            _mapperTurno = mapperTurno;
            _mapperHistorialTurno = mapperHistorialTurno;
            _mapperTurnoServicio = mapperTurnoServicio;
        }

        public async Task<Result<IEnumerable<TurnoReadDTO?>>> GetPaged(int pageNumber, int pageSize)
        {
            var result = await _unitOfWork.TurnoRepository.GetPaged(pageNumber, pageSize);
            if (!result.Any())
            {
                return Result<IEnumerable<TurnoReadDTO?>>.Fail("Aun no hay registros de turnos.");
            }

            var turnosDTO = result.Select(t => _mapperTurno.ToReadDTO(t!));

            return Result<IEnumerable<TurnoReadDTO?>>.Succes(turnosDTO);
        }

        //>>>>>>>>>>>>>CORREGIR TODO DE ACA HACIA ABAJO Y REVEER LO DE ARRIBA<<<<<<<<<<<<<<<<<<<<<<<<<

        public async Task<Result<IEnumerable<TurnoReadDTO?>>> GetAll()
        {
            var turnos = await _unitOfWork.TurnoRepository.GetAll();
            if (!turnos.Any())
            {
                return Result<IEnumerable<TurnoReadDTO?>>.Fail("Error, aun no hay registros de Turnos");
            }

            var turnosDTO = turnos.Select(t => _mapperTurno.ToReadDTO(t!));

            return Result<IEnumerable<TurnoReadDTO?>>.Succes(turnosDTO);
        }

        //OBTENER POR ID
        //Si lo que se obtiene de GetById es un turno con servicios incluidos y estadoTurnos, entonces falta especificar esos objetos en la clase TurnoReadDTO
        public async Task<Result<TurnoReadDTO>> GetById(int id)
        {
            var turno = await _unitOfWork.TurnoRepository.GetById(id);
            if (turno == null)
            {
                return Result<TurnoReadDTO>.Fail($"No existe registro con id {id}");
            }

            var turnoDTO = _mapperTurno.ToReadDTO(turno);

            return Result<TurnoReadDTO>.Succes(turnoDTO);
        }





        public async Task<Result<TurnoReadDTO>> Add(TurnoCreateUpdateDTO turno)
        {
            if (turno == null) throw new ArgumentNullException("El turno debe completarse.");

            var validarturno = await _validatorTurno.ValidateAsync(turno);
            if (!validarturno.IsValid)
            {
                var errors = string.Join("; ", validarturno.Errors.Select(e => e.ErrorMessage));
                return Result<TurnoReadDTO>.Fail(errors);
            }

            //Buscar cada servicio id en el repositorio y verificar que pertenezca a la lista
            var servicios = turno.ServiciosIds;
            List<Servicio> serviciosEntities = new List<Servicio>();

            foreach (var servicio in servicios)
            {
                var servicioEntity = await _unitOfWork.ServicioRepository.GetById(servicio);
                serviciosEntities.Add(servicioEntity!);
            }

            var turnoToEntity = _mapperTurno.ToEntity(turno);

            await _unitOfWork.BeginTransactionAsync();


            try
            { //------------------------------TURNO--------------------------------



                await _unitOfWork.TurnoRepository.Add(turnoToEntity);

                //---------------------------FIN-TURNO-------------------------------

                //---------------------------HISTORIALTURNO-------------------------------
                //Por ser la primera vez que se crea el turno, el estado anterior seria Turno DISPONIBLE            

                //Inicia el historial
                var inicioHistorialTurno = new HistorialTurnoCreateUpdateDTO
                {
                    TurnoId = turnoToEntity.TurnoId,
                    FechaHoraAnterior = null,
                    FechaHoraActual = new DateTimeOffset(turnoToEntity.FechaTurno, turnoToEntity.HoraTurno, TimeSpan.FromHours(-3)),
                    EstadoTurnoAnterior = (int)EstadoTurnoEnum.Disponible,
                    EstadoTurnoActual = (int)EstadoTurnoEnum.Reservado,
                };

                var validarHistorialTurno = await _validatorHistorialTurno.ValidateAsync(inicioHistorialTurno);
                if (!validarHistorialTurno.IsValid)
                {
                    var errors = string.Join("; ", validarHistorialTurno.Errors.Select(e => e.ErrorMessage));
                    return Result<TurnoReadDTO>.Fail(errors);
                }

                var historialTurnoToEntity = _mapperHistorialTurno.ToEntity(inicioHistorialTurno);

                await _unitOfWork.HistorialTurnoRepository.Add(historialTurnoToEntity);

                //---------------------------FIN-HISTORIALTURNO-------------------------------


                //---------------------------TURNOSERVICIO-------------------------------
                //BLOQUE AGREGAR SERVICIOS A TABLA TurnoServicio
                //Crear Registros de los Servicios del turno
                //EN OBSERVACION... ... ... XXXXXXXXXXXXXXXXXXXXXXXX

                foreach (var servicio in serviciosEntities)
                {
                    var agregarTurnoServicioDTO = new TurnoServicioCreateUpdateDTO
                    {
                        TurnoId = turnoToEntity.TurnoId,
                        ServicioId = servicio.ServicioId,
                        MontoAplicado = servicio.Precio,
                        TiempoAplicado = servicio.Duracion,
                    };

                    //mapear turno servicio a Entity para poder ingresarlo al parametro de repositorio.
                    var servicioEntity = _mapperTurnoServicio.ToEntity(agregarTurnoServicioDTO);

                    await _unitOfWork.TurnoServicioRepository.Add(servicioEntity);


                }
                //---------------------------FIN-TURNOSERVICIO-------------------------------

                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.CommitAsync();


                var turnoDto = _mapperTurno.ToReadDTO(turnoToEntity);

                return Result<TurnoReadDTO>.Succes(turnoDto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                return Result<TurnoReadDTO>.Fail($"Error al intentar crear el Turno {ex.Message}");
            }

        }

        //public async Task<Result<TurnoReadDTO>> Update(int Id, TurnoCreateUpdateDTO TurnoActualizado)
        //{
        //    if (TurnoActualizado == null) return Result<TurnoReadDTO>.Fail("Debe ingresar algun valor de actualizacion para continuar");

        //    var turno = await _unitOfWork.TurnoRepository.GetById(Id);
        //    if (turno == null)
        //    {
        //        return Result<TurnoReadDTO>.Fail($"TurnoId {Id} no existe o ingreso mal el id");
        //    }

        //    var estadoTurnoAnterior = turno.EstadoTurnoId;

        //    var validarTurnoActualizado = await _validatorTurno.ValidateAsync(TurnoActualizado);
        //    if (!validarTurnoActualizado.IsValid)
        //    {
        //        var errores = string.Join(",", validarTurnoActualizado.Errors.Select(e=> e.ErrorMessage));
        //        return Result<TurnoReadDTO>.Fail(errores);
        //    }

        //    await _unitOfWork.BeginTransactionAsync();

        //    try
        //    {
        //        var turnoActualizadoToEntity =  _mapperTurno.ToEntity(TurnoActualizado);

        //        await _unitOfWork.TurnoRepository.Update(Id, turnoActualizadoToEntity);

        //        var historialTurno = new HistorialTurno
        //        {
        //            TurnoId = Id,
        //            EstadoTurnoAnterior = estadoTurnoAnterior,
        //            EstadoTurnoActual = turnoActualizadoToEntity.EstadoTurnoId,
        //            FechaHoraActual = new DateTimeOffset(turnoActualizadoToEntity.FechaTurno, turnoActualizadoToEntity.HoraTurno, new TimeSpan(-3)),
        //            FechaHoraAnterior = new DateTimeOffset(turno.FechaTurno, turno.HoraTurno, new TimeSpan(-3))

        //        };

        //        await _unitOfWork.HistorialTurnoRepository.Add(historialTurno);

        //        await _unitOfWork.SaveChangeAsync();

        //        await _unitOfWork.CommitAsync();

        //        var turnoToDTO = _mapperTurno.ToReadDTO(turnoActualizadoToEntity);

        //        return Result<TurnoReadDTO>.Succes(turnoToDTO);

        //    }catch(Exception ex)
        //    {
        //        var messagge = ex.InnerException!.Message;
        //        return Result<TurnoReadDTO>.Fail(messagge);

        //    }
        //}


        public async Task<Result<TurnoReadDTO>> UpdateEstadoTurno(int TurnoId, int estadoTurnoId)
        {
            var turno = await _unitOfWork.TurnoRepository.GetById(TurnoId);
            if (turno == null)
            {
                return Result<TurnoReadDTO>.Fail($"TurnoId {TurnoId} inexistente o incorrecto");
            }

            bool esValido = Enum.IsDefined(typeof(EstadoTurnoEnum), estadoTurnoId); //Valida que el estadoTurnoID sea valido por medio de los valores de Enum que lo definen.
            if (!esValido)
            {
                return Result<TurnoReadDTO>.Fail($"El id de estado turno no existe");
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                turno.EstadoTurnoId = estadoTurnoId;

                await _unitOfWork.SaveChangeAsync();

                var turnoToDTO = _mapperTurno.ToReadDTO(turno);

                return Result<TurnoReadDTO>.Succes(turnoToDTO);

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                return Result<TurnoReadDTO>.Fail($"Error al intentar actualizar el estado del turno, {ex.Message}");
            }


        }

        //ACTUALIZAR FECHA Y HORA.
        //AQUI SE DEBE HALLAR LA FORMA DE VALIDAR LA FECHA Y LA HORA.
        public async Task<Result<TurnoReadDTO>> UpdateFechaYHora(int TurnoId, DateOnly nuevaFecha, TimeOnly nuevaHora)
        {
            var turno = await _unitOfWork.TurnoRepository.GetById(TurnoId);

            if (turno == null) return Result<TurnoReadDTO>.Fail("Turno inexistente o incorrecto.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                turno.FechaTurno = nuevaFecha;
                turno.HoraTurno = nuevaHora;

                await _unitOfWork.SaveChangeAsync();

                var turnoToDTO = _mapperTurno.ToReadDTO(turno);

                return Result<TurnoReadDTO>.Succes(turnoToDTO);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();

                return Result<TurnoReadDTO>.Fail($"Algo salio mal al intentar actualizar fecha y hora del turno: {ex.Message}");
            }


        }



        //sI LO QUE QUIERO CONSEGUIR ES CAMBIAR EL ESTADO DEL TURNO, EL NOMBRE Y LA IMPLEMENTACION DEL METODO
        //DEBEN CAMBIAR Y SER DIFERENTE COMO UpdateEstadoTurno y recibir como parametro el estado del Turno actualizado.
        //public async Task<Result<TurnoReadDTO>> Update(int id, TurnoCreateUpdateDTO turnoActualizado)
        //{
        //    if (turnoActualizado == null) throw new ArgumentNullException("Los campos de Turno deben completarse.");


        //    await _unitOfWork.BeginTransactionAsync();

        //    //RECUPERA EL REGISTRO DEL TURNO CON EL ID EN CUESTION
        //    var turnoActual = await _unitOfWork.TurnoRepository.GetById(id);
        //    if (turnoActual == null)
        //    {
        //        return Result<TurnoReadDTO>.Fail($"No existe registro con id {id}");
        //    }
        //    // VALIDACION DE LOS DATOS DEL TurnoDTO
        //    var validarTurnoActualizar = await _validatorTurno.ValidateAsync(turnoActualizado);
        //    if (!validarTurnoActualizar.IsValid)
        //    {
        //        var errors = string.Concat("; ", validarTurnoActualizar.Errors.Select(e => e));
        //        return Result<TurnoReadDTO>.Fail(errors);
        //    }

        //    //VER QUE SE ACTUALIZA DE TURNO PORQUE AHI TAMBIEN SE ENCUENTRAN LOS SERVICIOS [CORREGIR ]
        //    var nuevoRegistrosDeTurno = new TurnoCreateUpdateDTO
        //    {
        //        Detalle = turnoActualizado.Detalle,
        //        ClienteId = turnoActualizado.ClienteId,
        //        EstadoTurnoId = turnoActualizado.EstadoTurnoId,
        //        HoraTurno = turnoActualizado.HoraTurno,
        //        FechaTurno = turnoActualizado.FechaTurno
        //        //[COREGIR] NO CONTIENE LISTA DE SERVICIOS (EN ESTE CASO Turno.cs contiene una lista de turnoServicios.cs del cual debe acceder a cada uno por cada servicio agregado al turno)
        //    };

        //    //var serviciosDelTurno = turnoActualizado.Servicios <---ALGO ASI DEBE SER LA VARIABLE QUE ALMACENE LOS SERVICIOS DEL TURNO A ACTUALIZAR


        //    //MAPEO DEL TURNO A ENTITY
        //    var turnoActualizadoEntity = _mapperTurno.ToEntity(nuevoRegistrosDeTurno);
        //    //ACTUALIZACION DEL TURNO
        //    await _unitOfWork.TurnoRepository.Update(id, turnoActualizadoEntity);
        //    await _unitOfWork.SaveChangeAsync();

        //    //Acciones:
        //    //Agrega nuevo HistorialTurno de turno.
        //    //Registro que se conserva para el nuevo registro: Turno.
        //    //Registros Anteriores y Registros Actuales: EstadoTurno, FechaHora
        //    //         
        //    var fechaHoraActual = new DateTimeOffset(turnoActual.FechaTurno, turnoActual.HoraTurno, new TimeSpan(-3));
        //    var fechaHoraActualizada = new DateTimeOffset(turnoActualizadoEntity.FechaTurno, turnoActualizadoEntity.HoraTurno, new TimeSpan(-3));
        //    var estadoTurnoActual = turnoActual.EstadoTurnoId;
        //    var estadoTurnoActualizado = turnoActualizadoEntity.EstadoTurnoId;

        //    var historialTurno = new HistorialTurno
        //    {
        //        TurnoId = turnoActualizadoEntity.TurnoId,
        //        FechaHoraAnterior = fechaHoraActual,
        //        FechaHoraActual = fechaHoraActualizada,
        //        EstadoTurnoAnterior = estadoTurnoActual,
        //        EstadoTurnoActual = estadoTurnoActual
        //    };

        //    //Agregar nuevo registro de historial turno.
        //    await _unitOfWork.HistorialTurnoRepository.Add(historialTurno);
        //    await _unitOfWork.SaveChangeAsync();

        //    //-------------------------TURNOSERVICIOS ACTUALIZACION--------------------?DONDE SE LLEVA A CABO LA RELACION DE LOS SERVICIOS EN ESTA CAPA DE TURNOUPDATE?

        //    //Actualizacion de Servicio o servicios | Agregar, Quitar, Cambiar(Quitar,Agregar)
        //    //var turnoServicioId = await _unitOfWork.TurnoServicioRepository.GetById(turno.);

        //    var serviciosActuales = turnoActual.TurnoServicios.Select(s => s.ServicioId);
        //    var serviciosActualizado = turnoActualizado.Servicios.Select(s => s.ServicioId);

        //    var serviciosAgregar = serviciosActualizado.Except(serviciosActuales).ToList();
        //    var serviciosEliminar = serviciosActuales.Except(serviciosActualizado).ToList();//Si esto es asi entonces serviciosActualizados contiene los servicio
        //                                                                                    //que ya existian o serviciosActuales

        //    //Recuperar los registros de TurnoServicio, relacionados al TURNO
        //    // con el id correspondiente a los servicios que se quieren eliminar
        //    //ACLARACION: llamada a la variable 'turnoActualizadoEntity' porque es el turno actual en cuestion que se quiere modificar.
        //    //ACLARACION: 'ts' -> turnoServicio.
        //    var eliminarRelaciones = turnoActualizadoEntity.TurnoServicios
        //        .Where(ts => serviciosEliminar.Contains(ts.ServicioId))
        //        .ToList();

        //    //Aquellos ServicioId's que coinciden con los nuevos servicios agregados.  
        //    //ACLARACION: el origen de Datos es 'turnoActualizado' porque de ahi yacen los servicios actualizados del turno
        //    var agregarRelaciones = turnoActualizado.Servicios
        //        .Where(s => serviciosAgregar.Contains(s.ServicioId))
        //        .ToList();


        //    //Una vez con los registros que se quieren eliminar
        //    foreach (var servicio in eliminarRelaciones)
        //    {
        //        await _unitOfWork.TurnoServicioRepository.Remove(servicio);
        //    }

        //    foreach (var servicio in agregarRelaciones)
        //    {

        //        //turnoUpdate.TurnoServicios.Add();
        //        //Aca se puede simplificar y agregar estos objetos a la coleccion de 
        //        //ServicioActualizadoEntity.
        //        //await _unitOfWork.TurnoServicioRepository.Add(
        //        turnoActualizadoEntity.TurnoServicios.Add(
        //           new TurnoServicio
        //           {
        //               TurnoId = turnoActualizadoEntity.TurnoId,
        //               ServicioId = servicio.ServicioId,
        //               MontoAplicado = servicio.Precio,
        //               TiempoAplicado = servicio.Duracion
        //           }
        //           );
        //    }

        //    await _unitOfWork.SaveChangeAsync();

        //    await _unitOfWork.CommitAsync();

        //    var turnoReadDTO = _mapperTurno.ToReadDTO(turnoActualizadoEntity);
        //    return Result<TurnoReadDTO>.Succes(turnoReadDTO);
        //}





        //Porque se eliminaria un Turno? porque se cancelo? entonces, debe estar ese lugar libre para otros?
        //Turno Se elimina cuando: 1) Cliente no asiste 2) Cliente cancela antes de tiempo 3)Que pasa luego de que el cliente complete su turno -> Se elimina tal registro o se conserva para archivar historial???
        public async Task<Result<string>> Delete(int id)
        {
            var eliminarTurno = await _unitOfWork.TurnoRepository.GetById(id);
            if (eliminarTurno == null)
            {
                return Result<string>.Fail($"Registro de Turno {id} no existe.");
            }

            await _unitOfWork.TurnoRepository.Remove(eliminarTurno);

            return Result<string>.Succes("Turno eliminado.");
        }



        //Procedimientos




    }
}
