using AutoMapper;
using EducaMBAXpert.Pagamentos.Application.ViewModels;
using EducaMBAXpert.Pagamentos.Business.Interfaces;

namespace EducaMBAXpert.Pagamentos.Application.Services
{
    public class PagamentoAppService : IPagamentoAppService
    {

        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMapper _mapper;

        public PagamentoAppService(IPagamentoRepository pagamentoRepository,
                                   IMapper mapper)
        {
            _pagamentoRepository = pagamentoRepository;
            _mapper = mapper;
        }


        public async Task<PagamentoViewModel> ObterPorId(Guid id)
        {
            return _mapper.Map<PagamentoViewModel>(await _pagamentoRepository.ObterPorId(id));
        }

        public async Task<IEnumerable<PagamentoViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<PagamentoViewModel>>(await _pagamentoRepository.ObterTodos());
        }

        public void Dispose()
        {
            _pagamentoRepository?.Dispose();
        }
    }
}
