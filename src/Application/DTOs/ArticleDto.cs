using System;

namespace Application.DTOs;
public class ArticleDto
{
    public string Reference { get; set; } = "";
    public string Nom { get; set; } = "";
    public decimal PrixHT { get; set; }
    public decimal PrixTTC { get; set; }
    public int QuantiteStock { get; set; }
    public string TypeArticle { get; set; } = "";
    public DateTime? DLC { get; set; }
    public string? TypeVente { get; set; }
    public string? Packaging { get; set; }
}
