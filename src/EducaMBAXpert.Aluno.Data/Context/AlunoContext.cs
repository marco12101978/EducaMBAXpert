using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Core.Messages;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using EducaMBAXpert.Alunos.Domain.Entities;
using EducaMBAXpert.Alunos.Data.Mappings;
using Microsoft.EntityFrameworkCore.Design;

namespace EducaMBAXpert.Alunos.Data.Context
{
    public class AlunoContext : DbContext, IUnitOfWork
    {
        public AlunoContext(DbContextOptions<AlunoContext> options) : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<AulaConcluida> AulasConcluidas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfiguration(new AlunoMapping());
            modelBuilder.ApplyConfiguration(new EnderecoMapping());
            modelBuilder.ApplyConfiguration(new MatriculasMapping());
            modelBuilder.ApplyConfiguration(new AulaConcluidaMapping());

            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(CursoContext).Assembly);

            base.OnModelCreating(modelBuilder);

        }


        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            return await base.SaveChangesAsync() > 0;
        }
    }
}
