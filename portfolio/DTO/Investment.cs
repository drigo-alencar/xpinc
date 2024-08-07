namespace DTO;

public class Investment {
    public required string Name { get; set; }

    public double Price { get; set; }

    public required DateTime Expiration { get; set; }
}