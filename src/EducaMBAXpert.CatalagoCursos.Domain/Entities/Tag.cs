using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Domain.Entities
{
    namespace EducaMBAXpert.CatalagoCursos.Domain.Entities
    {
        public class Tag : ValueObject
        {
            public string Valor { get; private set; }

            protected Tag() { } // Para o EF Core

            public Tag(string valor)
            {
                Valor = valor ?? throw new ArgumentNullException(nameof(valor));
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Valor;
            }
        }
    }
}
