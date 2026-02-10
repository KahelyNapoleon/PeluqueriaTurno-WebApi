using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.PagoDTOs
{
    internal class PagoUpdateDTO
    {
        public int MetodoPagoId { get; set; }

        public decimal MontoTotal { get; set; }

        public DateTimeOffset FechaPago { get; set; }
    }
}
