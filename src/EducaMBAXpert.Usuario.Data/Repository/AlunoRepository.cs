using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Alunos.Data.Context;
using EducaMBAXpert.Alunos.Domain.Entities;
using EducaMBAXpert.Alunos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducaMBAXpert.Alunos.Data.Repository
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly AlunoContext _context;

        public AlunoRepository(AlunoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Aluno>> ObterTodos()
        {
            return await _context.Alunos
                 .Include(u => u.Enderecos)
                 .Include(u => u.Matriculas) 
                 .AsNoTracking()
                 .ToListAsync();
        }

        public async Task<Aluno?> ObterPorId(Guid id)
        {
            return await _context.Alunos
                .Include(u => u.Enderecos)
                .Include(u => u.Matriculas) 
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }



        public void Adicionar(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
        }

        public void Atualizar(Aluno aluno)
        {
           _context.Alunos.Update(aluno);
        }

        public void AdicionarEndereco(Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
        }

        public void AdicionarMatricula(Matricula matricula)
        {
            _context.Matriculas.AddAsync(matricula);
        }

        public void AtualizarMatricula(Matricula matricula)
        {
            _context.Matriculas.Update(matricula);
        }

        public async Task<Matricula?> ObterMatriculaPorId(Guid matriculaId)
        {
            return await _context.Matriculas
                 .AsNoTracking()
                 .FirstOrDefaultAsync(m => m.Id == matriculaId);
        }

        public async Task<IEnumerable<Matricula>> ObterTodasMatriculasPorAlunoId(Guid Alunoid,bool ativas)
        {
            return await _context.Matriculas
                .AsNoTracking()
                .Where(m => m.AlunoId == Alunoid && m.Ativo == ativas)
                .ToListAsync();
        }


        public void Dispose()
        {
            _context?.Dispose();
        }


    }
}
