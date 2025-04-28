using AutoMapper;
using EducaMBAXpert.Pagamentos.Application.ViewModels;
using EducaMBAXpert.Pagamentos.Business.Entities;

namespace EducaMBAXpert.Pagamentos.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<PagamentoViewModel, Pagamento>()
                     .ForMember(dest => dest.Transacao, opt => opt.MapFrom(src => src.Transacao));
 
            CreateMap<TransacaoViewModel, Transacao>();
        }

    }
}
