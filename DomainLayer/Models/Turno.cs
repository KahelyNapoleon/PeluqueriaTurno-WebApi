using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class Turno
{
    public int TurnoId { get; set; }

    public string? Detalle { get; set; }

    public int ClienteId { get; set; }

    public int EstadoTurnoId { get; set; }

    public TimeOnly HoraTurno { get; set; }

    public DateOnly FechaTurno { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual EstadoTurno EstadoTurno { get; set; } = null!;

    public virtual ICollection<HistorialTurno> HistorialTurnos { get; set; } = new List<HistorialTurno>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<TurnoServicio> TurnoServicios { get; set; } = new List<TurnoServicio>();
}
