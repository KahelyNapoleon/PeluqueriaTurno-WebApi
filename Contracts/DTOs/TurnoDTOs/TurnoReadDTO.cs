using Contracts.DTOs.TurnoServicioDTOs;

namespace Contracts.DTOs.TurnoDTOs
{
    public class TurnoReadDTO
    {
        public int TurnoId { get; set; }
        public string Detalle { get; set; } = null!;
        public int ClienteId { get; set; } 
        public int EstadoTurnoId { get; set; }
        public DateOnly FechaTurno { get; set; }
        public TimeOnly HoraTurno { get; set; }

        public IEnumerable<TurnoServicioReadDTO> Servicios { get; set; } = new List<TurnoServicioReadDTO>();
      

    }
}
