using EducaMBAXpert.Core.DomainObjects;
using System.Text.Json.Serialization;

namespace EducaMBAXpert.Alunos.Domain.Entities
{
    public class AulaConcluida : Entity
    {
        public Guid MatriculaId { get; private set; }
        public Guid AulaId { get; private set; }
        public DateTime DataConclusao { get; private set; }

        [JsonIgnore]
        public Matricula Matricula { get; private set; }

        protected AulaConcluida() { }

        public AulaConcluida(Guid matriculaId, Guid aulaId)
        {
            MatriculaId = matriculaId;
            AulaId = aulaId;
            DataConclusao = DateTime.Now;
        }
    }
}
