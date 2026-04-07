namespace Domain.ValueObjects;

public class ReferenceEAN13
{
    public string Value { get; private set; }

    // Constructeur privé pour EF Core
    private ReferenceEAN13() { }

    public ReferenceEAN13(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 13 || !value.All(char.IsDigit))
            throw new ArgumentException("Format EAN-13 invalide (13 chiffres)");
        Value = value;
    }

    public override string ToString() => Value;
}