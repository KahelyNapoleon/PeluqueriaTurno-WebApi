using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.TurnoDTOs
{
    public class TurnoUpdateFechaYHoraDTO
    {
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
    }
}

