using AutoMapper;
using EducaMBAXpert.Pagamentos.Application.ViewModels;
using EducaMBAXpert.Pagamentos.Business.Entities;

namespace EducaMBAXpert.Pagamentos.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Pagamento, PagamentoViewModel>()
                      .ForMember(dest => dest.Transacao, opt => opt.MapFrom(src => src.Transacao));

            CreateMap<Transacao, TransacaoViewModel>();
        }

    }
}
