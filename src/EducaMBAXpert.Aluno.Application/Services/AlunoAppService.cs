using AutoMapper;
using EducaMBAXpert.Core.DomainObjects;
using EducaMBAXpert.Alunos.Application.Interfaces;
using EducaMBAXpert.Alunos.Application.ViewModels;
using EducaMBAXpert.Alunos.Domain.Entities;
using EducaMBAXpert.Alunos.Domain.Interfaces;
using EducaMBAXpert.Alunos.Data.Repository;

namespace EducaMBAXpert.Alunos.Application.Services
{
    public class AlunoAppService : IAlunoComandoAppService, IAlunoConsultaAppService
    {

        private readonly IAlunoRepository _alunoRepository;
        private readonly IAlunoService _alunoService;
        private readonly IMapper _mapper;

        public AlunoAppService(IAlunoRepository alunoRepository,
                                 IAlunoService alunoService,
                                 IMapper mapper)
        {
            _alunoRepository = alunoRepository;
            _alunoService = alunoService;
            _mapper = mapper;
        }


        public async Task<AlunoViewModel> ObterPorId(Guid id)
        {
            return _mapper.Map<AlunoViewModel>(await _alunoRepository.ObterPorId(id));
        }

        public async Task<IEnumerable<AlunoViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<AlunoViewModel>>(await _alunoRepository.ObterTodos());
        }


        public async Task Adicionar(AlunoInputModel alunoViewModel)
        {
            var _aluno = _mapper.Map<Aluno>(alunoViewModel);
            _alunoRepository.Adicionar(_aluno);

            await _alunoRepository.UnitOfWork.Commit();
        }

        public async Task Atualizar(AlunoInputModel alunoViewModel)
        {
            var _aluno = _mapper.Map<Aluno>(alunoViewModel);
            _alunoRepository.Atualizar(_aluno);

            await _alunoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Ativar(Guid id)
        {
            var _sucess = await _alunoService.Ativar(id);
            if (!_sucess)
            {
                throw new DomainException("Falha ao Inativar Aluno");
            }

            return true;
        }

        public  async Task<bool> Inativar(Guid id)
        {
            var _sucess = await _alunoService.Inativar(id);
            if (!_sucess)
            {
                throw new DomainException("Falha ao Inativar Aluno");
            }

            return true;
        }

        public async Task AdicionarEndereco(EnderecoInputModel enderecoViewModel)
        {
            var _endereco = _mapper.Map<Endereco>(enderecoViewModel);
            _alunoRepository.AdicionarEndereco(_endereco);

            await _alunoRepository.UnitOfWork.Commit();
        }

        public async Task AdicionarMatriculaCurso(MatriculaInputModel matriculaInputModel)
        {
            var _matricula = _mapper.Map<Matricula>(matriculaInputModel);

            _alunoRepository.AdicionarMatricula(_matricula);

            await _alunoRepository.UnitOfWork.Commit();
        }


        public async Task AtualizarMatriculaCurso(MatriculaInputModel matriculaInputModel)
        {
            var _matricula = _mapper.Map<Matricula>(matriculaInputModel);

            _alunoRepository.AtualizarMatricula(_matricula);

            await _alunoRepository.UnitOfWork.Commit();
        }


        public async Task<MatriculaViewModel> ObterMatriculaPorId(Guid id)
        {
            return _mapper.Map<MatriculaViewModel>(await _alunoRepository.ObterMatriculaPorId(id));
        }

        public async Task<IEnumerable<MatriculaViewModel>> ObterTodasMatriculasPorAlunoId(Guid id,bool ativas)
        {
            var xxxx = await _alunoRepository.ObterTodasMatriculasPorAlunoId(id, ativas);

            return _mapper.Map<IEnumerable<MatriculaViewModel>>(await _alunoRepository.ObterTodasMatriculasPorAlunoId(id, ativas));
        }

        public void Dispose()
        {
            _alunoRepository?.Dispose();
            _alunoService?.Dispose();
        }

    }
}
