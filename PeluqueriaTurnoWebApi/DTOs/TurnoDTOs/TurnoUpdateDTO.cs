using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeluqueriaTurnoWebApi.DTOs.TurnoDTOs
{
    internal class TurnoUpdateDTO
    {
        public string? Detalle { get; set; }//EL DETALLE PUEDE CAMBIAR SI EL CLIENTE CAMBIA O QUITA UN SERVICIO.

        public int EstadoTurnoId { get; set; } //SI SE ACTUALIZA AL CONFIRMAR
                                               //RECORDAR ESTAOD: LIBRE>CONFIRMAR>OCUPADO>EN PROCESO>FINALIZADO
        public TimeOnly HoraTurno { get; set; }

        public DateOnly FechaTurno { get; set; }
    }
}
