using BLL.Mapping;
using Contracts.DTOs.PagoDTOs;
using DomainLayer.Models;


namespace BLL.MappingMethods
{
    public class PagoMapping : IMappingService<Pago, PagoReadDTO, PagoCreateUpdateDTO>
    {
        public  PagoReadDTO ToReadDTO( Pago p)
        {
            return new PagoReadDTO
            {
                PagoId = p.PagoId,
                TurnoId = p.TurnoId,
                MetodoPagoId = p.MetodoPagoId,
                MontoTotal = p.MontoTotal,
                FechaPago = p.FechaPago
            };
        }

        public Pago ToEntity( PagoCreateUpdateDTO p)
        {
            return new Pago
            {
                TurnoId = p.TurnoId,
                MetodoPagoId = p.MetodoPagoId,
                MontoTotal = p.MontoTotal,
                FechaPago = p.FechaPago
            };
        }

        public Pago ToUpdateEntity(int id, PagoCreateUpdateDTO p)
        {
            return new Pago
            {
                PagoId = id,
                TurnoId = p.TurnoId,
                MetodoPagoId = p.MetodoPagoId,
                MontoTotal = p.MontoTotal,
                FechaPago = p.FechaPago
            };

        }
    }
}
