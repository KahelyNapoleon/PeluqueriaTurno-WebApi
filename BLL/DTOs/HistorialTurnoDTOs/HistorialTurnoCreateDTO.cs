using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.HistorialTurnoDTOs
{
    internal class HistorialTurnoCreateDTO
    {
        public int TurnoId { get; set; }

        public DateTimeOffset? FechaHoraAnterior { get; set; }

        public DateTimeOffset FechaHoraActual { get; set; }

        public int EstadoTurnoAnterior { get; set; }

        public int EstadoTurnoActual { get; set; }
    }
}
