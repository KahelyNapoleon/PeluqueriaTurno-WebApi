using BLL.Mapping;
using BLL.Result;
using BLL.Services.Interfaces;
using Contracts.DTOs.HistorialTurnoDTOs;
using Contracts.DTOs.TurnoDTOs;
using Contracts.DTOs.TurnoServicioDTOs;
using DAL.EstadoTurnoEnum;
using DAL.Repositorios.Interfaces;
using DAL.UnitOfWork.IUnitOfWork;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TurnoUpdatesService  //ITurnoUpdateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServicioRepository _servicioRepository;
        private IMappingService<Turno, TurnoReadDTO, TurnoCreateUpdateDTO> _mapperTurno;
        private IMappingService<HistorialTurno, HistorialTurnoReadDTO, HistorialTurnoCreateUpdateDTO> _mapperHistorialTurno;
        private IMappingService<TurnoServicio, TurnoServicioReadDTO, TurnoServicioCreateUpdateDTO> _mapperTurnoServicio;
        public TurnoUpdatesService(
            IUnitOfWork unitOfWork,
            IServicioRepository servicioRepository,
            IMappingService<Turno, TurnoReadDTO, TurnoCreateUpdateDTO> mapperTurno,
            IMappingService<HistorialTurno, HistorialTurnoReadDTO, HistorialTurnoCreateUpdateDTO> mapperHistorialTurno,
            IMappingService<TurnoServicio, TurnoServicioReadDTO, TurnoServicioCreateUpdateDTO> mapperTurnoServicio
            )
        {
            _unitOfWork = unitOfWork;
            _servicioRepository = servicioRepository;
            _mapperTurno = mapperTurno;
            _mapperHistorialTurno = mapperHistorialTurno;
            _mapperTurnoServicio = mapperTurnoServicio;
        }

        public async Task<Result<TurnoReadDTO>> UpdateEstadoTurno(int turnoId, int estadoTurnoId)
        {
            var turno = await _unitOfWork.TurnoRepository.GetById(turnoId);
            if (turno == null)
            {
                return Result<TurnoReadDTO>.Fail($"TurnoId {turnoId} inexistente o incorrecto");
            }

            //Valida que el estadoTurnoID sea valido por medio de los valores de Enum que lo definen.
            bool esValido = Enum.IsDefined(typeof(EstadoTurnoEnum), estadoTurnoId);
            if (!esValido)
            {
                return Result<TurnoReadDTO>.Fail($"El id de estado turno no existe");
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                turno.EstadoTurnoId = estadoTurnoId;

                //Crear HistorialTurno del Turno Actualizado ... ... ...


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
        public async Task<Result<TurnoReadDTO>> UpdateFechaYHora(int turnoId, DateOnly fecha, TimeOnly hora)
        {
            var turno = await _unitOfWork.TurnoRepository.GetById(turnoId);

            if (turno == null) return Result<TurnoReadDTO>.Fail("Turno inexistente o incorrecto.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                turno.FechaTurno = fecha;
                turno.HoraTurno = hora;

                //Crear Historial Turno Del Turno Actualizado Con EstadoTurno Id = 'modificado' y la respectivas fechas y horas nuevas
                //EstadoTurnoId = Modificado,
                //FechaYHoraNueva = new DateTimeOffset(Fecha, Hora, TimeSpan.FromHours(-3)),
                //Crear un DTO para actualizar la fecha de una sola vez.

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

        //public async Task<Result<TurnoReadDTO>> UpdateServicios(int turnoId, List<int> serviciosIds)
        //{
        //    if (serviciosIds.Count == 0) throw new ArgumentException("Debe ingresar al menos un servicio");

        //    var turno = await _unitOfWork.TurnoRepository.GetById(turnoId);

        //    if (turno == null) return Result<TurnoReadDTO>.Fail("Turno inexistente o incorrecto.");

        //    //Validar que los servidiosIds sean parte de la lista de servicios existentes ... ... ...
        //    var validationServiciosIdsResult = ValidarServicios(serviciosIds);
        //    if (!validationServiciosIdsResult)
        //    {
        //        return Result<TurnoReadDTO>.Fail($"Uno o mas servicios no pertenecen a los servicios existentes.");
        //    }

        //    await _unitOfWork.BeginTransactionAsync();

        //    try
        //    {
        //        var serviciosActuales = turno.TurnoServicios.Select(ts => ts.ServicioId);

                 
                
        //    }
        //    catch (Exception ex)
        //    {
        //        await _unitOfWork.RollBackAsync();
        //        //Eviar error por medio de LOG ... ... ...
        //        return Result<TurnoReadDTO>.Fail($"Algo salio mal al intentar actualizar los servicios: {ex.Message}");
        //    }
        //}

        //MEtodo Auxiliar de Validacion de servicios.
        //Evalua cada entero de servicios, si existen en los registros de Servicio.ServicioId.
        public bool ValidarServicios(List<int> servicios)
        {
            var servicioEntity = _servicioRepository.GetAll();
            var serviciosIds =  servicioEntity.Result.Select(s => s!.ServicioId).ToList();


            bool serviciosValidos = servicios.All(s => serviciosIds.Contains(s));
            if (serviciosValidos)
            {
                return true;
            }

            return false;
        }


    }

}
