using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeluqueriaTurnoWebApi.DTOs.TurnoDTOs
{
    internal class TurnoCreateDTO
    {
        public string? Detalle { get; set; }

        public int ClienteId { get; set; }

        public int EstadoTurnoId { get; set; }

        public TimeOnly HoraTurno { get; set; }

        public DateOnly FechaTurno { get; set; }
    }
}
