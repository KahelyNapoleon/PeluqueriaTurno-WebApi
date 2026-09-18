using DomainLayer.Models;
using Contracts.DTOs.ClienteDTOs;
using BLL.Mapping;

namespace BLL.MappingMethods
{
    public class ClienteMapping : IMappingService<Cliente,ClienteReadDTO,ClienteCreateUpdateDTO>
    {
        public Cliente ToEntity(ClienteCreateUpdateDTO clienteCreateDto)
        {
            return new Cliente
            {
                Nombre = clienteCreateDto.Nombre,
                Apellido = clienteCreateDto.Apellido,
                NroCelular = clienteCreateDto.NroCelular,
                CorreoElectronico = clienteCreateDto.CorreoElectronico,
                FechaNacimiento = clienteCreateDto.FechaNacimiento,
                Preferencias = clienteCreateDto.Preferencias,
                Observaciones = clienteCreateDto.Observaciones,
            };
        }

        public Cliente ToUpdateEntity(int id,ClienteCreateUpdateDTO dto)
        {
            return new Cliente
            {
                ClienteId = id,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                NroCelular = dto.NroCelular,
                CorreoElectronico = dto.CorreoElectronico,
                FechaNacimiento = dto.FechaNacimiento,
                Preferencias = dto.Preferencias,
                Observaciones = dto.Observaciones
            };
        }

        public ClienteReadDTO ToReadDTO(Cliente c)
        {
            return new ClienteReadDTO
            {
                ClienteId = c.ClienteId,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                NroCelular = c.NroCelular,
                CorreoElectronico = c.CorreoElectronico,
                FechaNacimiento = c.FechaNacimiento,
                Preferencias = c.Preferencias,
                Observaciones = c.Observaciones,
                Activo = c.Activo
            };
        }

       
    }
}
