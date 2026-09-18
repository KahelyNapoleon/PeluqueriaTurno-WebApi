using Contracts.DTOs.ServicioDTOs;
using Contracts.DTOs.TurnoServicioDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.TurnoDTOs
{
    public class TurnoCreateUpdateDTO
    {
        public string? Detalle { get; set; }//EL DETALLE PUEDE CAMBIAR SI EL CLIENTE CAMBIA O QUITA UN SERVICIO.

        public int ClienteId { get; set; } // Se debe dejar este campo para actualizar, en caso de que el cliente
                                           //de baja el turno y el msimo quede libre
        public int EstadoTurnoId { get; set; } //CAMBIA SI: se confirma turno, si se cambia fecha y/o hora, si se agrega o cambia detalle y si se cambia 
                                                //el turno por otro cliente al abandonar el primer cliente el turno.
                                              
        public TimeOnly HoraTurno { get; set; }

        public DateOnly FechaTurno { get; set; }

        //Parece ser que es mejor que sea una lista de enteros paara referir los serivicos a actuializar.
        public List<int> ServiciosIds { get; set; } = new List<int>();
      
    }
}
