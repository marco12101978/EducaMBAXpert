﻿using AutoMapper;
using EducaMBAXpert.Usuarios.Application.ViewModels;
using EducaMBAXpert.Usuarios.Domain.Entities;

namespace EducaMBAXpert.Usuarios.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UsuarioViewModel, Usuario>()
                .ConstructUsing(u => new Usuario(u.Id, u.Nome, u.Email));

            CreateMap<EnderecoViewModel, Endereco>()
                .ConstructUsing(e =>
                    new Endereco(e.Rua, e.Numero, e.Complemento, e.Bairro, e.Cidade, e.Estado, e.Cep, e.UsuarioId)
                );
        }
    }
}
