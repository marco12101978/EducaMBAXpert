using EducaMBAXpert.Alunos.Domain.Entities;

namespace EducaMBAXpert.Alunos.Application.ViewModels
{
    public class MatriculaViewModel
    {
        public Guid Id { get; set; }

        // Relacionamento
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }

        // Dados da matrícula
        public DateTime DataMatricula { get; set; }
        public DateTime? DataDeConclusao { get; set; }
        public bool Ativo { get; set; }
        public decimal PercentualConclusao { get; set; }
        public List<AulaConcluida> AulasConcluidas { get; set; }
    }
}
