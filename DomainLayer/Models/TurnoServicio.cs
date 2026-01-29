using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class TurnoServicio
{
    public int TurnoServicioId { get; set; }

    public int TurnoId { get; set; }

    public int ServicioId { get; set; }

    public decimal MontoAplicado { get; set; }

    public int? TiempoAplicado { get; set; }

    public virtual Servicio Servicio { get; set; } = null!;

    public virtual Turno Turno { get; set; } = null!;
}
