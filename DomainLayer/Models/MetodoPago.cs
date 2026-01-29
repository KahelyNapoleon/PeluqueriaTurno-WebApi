using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class MetodoPago
{
    public int MetodoPagoId { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
