using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int TurnoId { get; set; }

    public int MetodoPagoId { get; set; }

    public decimal MontoTotal { get; set; }

    public DateTimeOffset FechaPago { get; set; }

    public virtual MetodoPago MetodoPago { get; set; } = null!;

    public virtual Turno Turno { get; set; } = null!;
}
