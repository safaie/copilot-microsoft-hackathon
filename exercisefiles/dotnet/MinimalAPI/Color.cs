public class Color
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!; // Make nullable or initialize

    [JsonPropertyName("category")]
    public string Category { get; set; } = null!; // Make nullable or initialize

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!; // Make nullable or initialize

    [JsonPropertyName("code")]
    public ColorCode Code { get; set; } = null!; // Make nullable or initialize
}

public class ColorCode
{
    [JsonPropertyName("rgba")]
    public int[]? RGBA { get; set; } // Make nullable

    [JsonPropertyName("hex")]
    public string HEX { get; set; } = null!; // Make nullable or initialize
}