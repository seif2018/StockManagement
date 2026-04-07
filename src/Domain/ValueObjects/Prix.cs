namespace Domain.ValueObjects;

public class Prix
{
    public decimal Valeur { get; private set; }

    // Constructeur privé pour EF Core
    private Prix() { }

    public Prix(decimal valeur)
    {
        if (valeur < 0) throw new ArgumentException("Le prix ne peut être négatif");
        Valeur = Math.Round(valeur, 2);
    }

    public override string ToString() => $"{Valeur:F2} €";
}