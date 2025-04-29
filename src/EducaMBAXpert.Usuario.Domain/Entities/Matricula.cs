using EducaMBAXpert.Core.DomainObjects;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace EducaMBAXpert.Usuarios.Domain.Entities
{
    public class Matricula : Entity
    {
        public Matricula(Guid usuarioId, Guid cursoId, DateTime dataMatricula, bool ativo)
        {
            UsuarioId = usuarioId;
            CursoId = cursoId;
            DataMatricula = dataMatricula;
            Ativo = ativo;
        }

        public Guid UsuarioId { get; private set; }
        public Guid CursoId { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public bool Ativo { get; private set; }

        [JsonIgnore]
        public Usuario Usuario { get; set; }


        private void Validar()
        {
            Validacoes.ValidarGuid(Id, "ID da Matricula Invalida.");
            Validacoes.ValidarGuid(UsuarioId, "ID do Usuário inválido.");
            Validacoes.ValidarGuid(CursoId, "ID do Curso inválido.");
        }

    }
}
