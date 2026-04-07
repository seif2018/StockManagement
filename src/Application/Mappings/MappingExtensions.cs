using Domain.Entities;
using System;

namespace Application.Mappings;

public static class MappingExtensions
{
    public static string GetTypeArticle(this Article article) =>
        article is ArticleAlimentaire ? "Alimentaire" : "NonAlimentaire";

    public static DateTime? GetDLC(this Article article) =>
        (article as ArticleAlimentaire)?.DLC;

    public static string? GetTypeVente(this Article article) =>
        (article as ArticleAlimentaire)?.TypeVente.ToString();

    public static string? GetPackaging(this Article article) =>
        (article as ArticleNonAlimentaire)?.Packaging.ToString();
}