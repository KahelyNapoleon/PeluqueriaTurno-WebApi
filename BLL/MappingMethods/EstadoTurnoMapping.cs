using BLL.Mapping;
using Contracts.DTOs.EstadoTurnoDTOs;
using DomainLayer.Models;


namespace BLL.MappingMethods
{
    public class EstadoTurnoMapping : IMappingService<EstadoTurno, EstadoTurnoReadDTO, EstadoTurnoCreateUpdateDTO>
    {
        public EstadoTurno ToEntity(EstadoTurnoCreateUpdateDTO et)
        {
            return new EstadoTurno
            {
                Descripcion = et.Descripcion
            };
        }

        public EstadoTurno ToUpdateEntity(int id, EstadoTurnoCreateUpdateDTO dto)
        {
            return new EstadoTurno
            {
                EstadoTurnoId = id,
                Descripcion = dto.Descripcion
            };
        }

        public EstadoTurnoReadDTO ToReadDTO(EstadoTurno e)
        {
            return new EstadoTurnoReadDTO
            {
                EstadoTurnoId = e.EstadoTurnoId,
                Descripcion = e.Descripcion
            };
        }
               
            
        
    }
}
