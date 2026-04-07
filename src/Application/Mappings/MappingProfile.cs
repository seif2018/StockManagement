using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Article, ArticleDto>()
            .ForMember(dest => dest.PrixHT, opt => opt.MapFrom(src => src.PrixHT.Valeur))
            .ForMember(dest => dest.PrixTTC, opt => opt.MapFrom(src => src.PrixTTC.Valeur))
            .ForMember(dest => dest.TypeArticle, opt => opt.MapFrom(src => src is ArticleAlimentaire ? "Alimentaire" : "NonAlimentaire"))
            .ForMember(dest => dest.DLC, opt => opt.MapFrom(src => GetDLC(src)))
            .ForMember(dest => dest.TypeVente, opt => opt.MapFrom(src => GetTypeVente(src)))
            .ForMember(dest => dest.Packaging, opt => opt.MapFrom(src => GetPackaging(src)));
    }

    private static DateTime? GetDLC(Article article) => (article as ArticleAlimentaire)?.DLC;
    private static string? GetTypeVente(Article article) => (article as ArticleAlimentaire)?.TypeVente.ToString();
    private static string? GetPackaging(Article article) => (article as ArticleNonAlimentaire)?.Packaging.ToString();
}