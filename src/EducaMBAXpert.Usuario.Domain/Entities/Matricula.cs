using EducaMBAXpert.Core.DomainObjects;
using System.Text.Json.Serialization;

namespace EducaMBAXpert.Alunos.Domain.Entities
{
    public class Matricula : Entity , IAggregateRoot
    {
        public Matricula(Guid alunoId, Guid cursoId)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
            DataMatricula = DateTime.Now;
            Ativo = false;

            Validar();
        }
        public Matricula(Guid alunoId, Guid cursoId, DateTime dataMatricula, bool ativo)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
            DataMatricula = dataMatricula;
            Ativo = ativo;

            Validar();
        }

        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public bool Ativo { get; private set; }

        public void Ativar()
        {
            Ativo = true;
        }

        public void Desativar()
        {
            Ativo = false;
        }


        private readonly List<AulaConcluida> _aulasConcluidas = new();
        public IReadOnlyCollection<AulaConcluida> AulasConcluidas => _aulasConcluidas;

        public void MarcarAulaComoConcluida(Guid aulaId)
        {
            if (_aulasConcluidas.Any(a => a.AulaId == aulaId)) return;
            _aulasConcluidas.Add(new AulaConcluida(this.Id, aulaId));
        }

        public bool PodeEmitirCertificado(int totalAulasCurso)
        {
            return _aulasConcluidas.Select(a => a.AulaId).Distinct().Count() == totalAulasCurso;
        }


        [JsonIgnore]
        public Aluno Aluno { get; set; }


        private void Validar()
        {
            Validacoes.ValidarGuid(AlunoId, "ID do Aluno inválido.");
            Validacoes.ValidarGuid(CursoId, "ID do Curso inválido.");
        }

    }
}
