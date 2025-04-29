using AutoMapper;
using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.CatalagoCursos.Domain.Entities;

namespace EducaMBAXpert.CatalagoCursos.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<CursoViewModel, Curso>();
            CreateMap<ModuloViewModel, Modulo>();
            CreateMap<AulaViewModel, Aula>();

            CreateMap<CursoInputModel, Curso>()
            .ConstructUsing(vm =>
                new Curso(vm.Titulo, vm.Descricao, vm.Categoria, vm.Nivel))
            .AfterMap((vm, curso) =>
            {
                if (vm.Tags != null)
                {
                    foreach (var tag in vm.Tags)
                    {
                        curso.AdicionarTag(tag);
                    }
                }

                if (vm.Modulos != null)
                {
                    foreach (var moduloVm in vm.Modulos)
                    {
                        var modulo = new Modulo(moduloVm.Nome);
                        curso.AdicionarModulo(modulo);
                    }
                }

                if (vm.Ativo)
                    curso.Ativar();
                else
                    curso.Inativar();
            });

        }
    }
}
