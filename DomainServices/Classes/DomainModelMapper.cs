using AutoMapper;
using DomainServices.DomainModels;
using Models = DataLibrary.Models;

namespace DomainServices.Classes
{
    public class DomainModelMapper : Profile
    {
        public DomainModelMapper() 
        {
            CreateMap<Transaction, Models.Transaction>();

            CreateMap<Models.Transaction, Transaction>()
                .ForMember(dest => dest.PaymentTypeName, map => map.MapFrom(src => src.PaymentType != null ? src.PaymentType.Name : string.Empty));
        }
    }
}
