using System;

namespace Application.DTOs;

public class InventaireExportDto
{
    public string ReferenceArticle { get; set; }
    public string NomArticle { get; set; }
    public decimal PrixHT { get; set; }
    public decimal PrixTTC { get; set; }
    public string TypeArticle { get; set; }
    public int AncienneQuantite { get; set; }
    public int NouvelleQuantite { get; set; }
    public int Ecart { get; set; }
    public DateTime Date { get; set; }
}