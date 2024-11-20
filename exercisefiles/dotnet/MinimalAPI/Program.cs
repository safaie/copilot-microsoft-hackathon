using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // Adds support for API explorer, which is used by Swagger.
builder.Services.AddSwaggerGen(); // Adds support for generating Swagger documents.

var app = builder.Build(); // Builds the WebApplication.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enables middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwaggerUI(); // Enables middleware to serve Swagger UI.
}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS.

// Define a simple GET endpoint that returns "Hello World!".
app.MapGet("/", () => "Hello World!");

app.Run(); // Runs the application.

// Endpoint to calculate days between two dates.
app.MapGet("/daysbetweendates", (DateTime date1, DateTime date2) =>
{
    var daysBetween = (date2 - date1).TotalDays;
    return Results.Ok(daysBetween);
});

// Endpoint to validate a phone number in Spanish format.
// Receives a parameter called phoneNumber via query string.
// Validates phoneNumber with Spanish format, for example +34666777888.
// Returns true if phoneNumber is valid, otherwise false.
app.MapGet("/validatephonenumber", (string phoneNumber) =>
{
    var isValid = Regex.IsMatch(phoneNumber, @"^\+[1-9]{1}[0-9]{3,14}$");
    return Results.Ok(isValid);
});

// Endpoint to validate a Spanish DNI.
// Receives a parameter called dni via query string.
// Calculates DNI letter and checks if it is valid.
// Returns "valid" if DNI is valid, otherwise "invalid".
app.MapGet("/validatespanishdni", (string dni) =>
{
    var dniNumber = int.Parse(dni.Substring(0, 8));
    var dniLetter = dni.Substring(8, 1).ToUpper();
    var letter = "TRWAGMYFPDXBNJZSQVHLCKE";
    var valid = dniLetter == letter[dniNumber % 23].ToString();
    return Results.Ok(valid ? "valid" : "invalid");
});

// Endpoint to return the RGBA value of a color from colors.json.
app.MapGet("/returncolorcode", async (string color) =>
{
    var colorsJson = await File.ReadAllTextAsync("colors.json");
    var colors = JsonSerializer.Deserialize<List<Color>>(colorsJson);

    var colorData = colors?.FirstOrDefault(c => c.Name.Equals(color, StringComparison.OrdinalIgnoreCase));
    if (colorData != null)
    {
        return Results.Ok(colorData.Rgba);
    }
    return Results.NotFound("Color not found");
});

app.Run(); // Runs the application.

public class Color
{
    public string Name { get; set; }
    public string Rgba { get; set; }
    public string Hex { get; set; }
}

// Endpoint to return a random joke from the joke API.
// Makes a call to the joke API and returns a random joke.
app.MapGet("/tellmeajoke", async () =>
{
    var client = new HttpClient();
    var response = await client.GetAsync("https://official-joke-api.appspot.com/random_joke");
    var joke = await response.Content.ReadAsStringAsync();
    return Results.Ok(joke);
});

// Endpoint to return a list of movies by a director from the movie API.
// Receives a parameter called director via query string.
// Makes a call to the movie API and returns a list of movies by that director.
app.MapGet("/moviesbydirector", async (string director) =>
{
    var client = new HttpClient();
    var response = await client.GetAsync($"https://api.themoviedb.org/3/search/person?api_key=de804e50&query={director}");
    var directorData = await response.Content.ReadAsStringAsync();
    var directorId = JsonSerializer.Deserialize<Director>(directorData).Results.FirstOrDefault()?.Id;

    if (directorId != null)
    {
        response = await client.GetAsync($"https://api.themoviedb.org/3/discover/movie?api_key=de804e50&with_people={directorId}");
        var moviesData = await response.Content.ReadAsStringAsync();
        var movies = JsonSerializer.Deserialize<Movies>(moviesData);
        return Results.Ok(movies);
    }
    return Results.NotFound("Director not found");
});

// Endpoint to parse a URL and return its components.
// Receives a parameter called someurl via query string.
// Parses the URL and returns the protocol, host, port, path, query string, and hash.
app.MapGet("/parseurl", (string someurl) =>
{
    var uri = new Uri(someurl);
    var parsedUrl = new
    {
        Protocol = uri.Scheme,
        Host = uri.Host,
        Port = uri.Port,
        Path = uri.AbsolutePath,
        QueryString = uri.Query,
        Hash = uri.Fragment
    };
    return Results.Ok(parsedUrl);
});

app.Run(); // Runs the application.

// Endpoint to list files in the current directory.
// Gets the current directory and returns the list of files.
app.MapGet("/listfiles", () =>
{
    var files = Directory.GetFiles(Directory.GetCurrentDirectory());
    return Results.Ok(files);
});

app.Run(); // Runs the application.

// Endpoint to return the memory consumption of the process in GB.
// Returns the memory consumption of the process in GB, rounded to 2 decimals.
app.MapGet("/calculatememoryconsumption", () =>
{
    var memory = Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024 * 1024.0);
    return Results.Ok(Math.Round(memory, 2));
});

app.Run(); // Runs the application.

// Endpoint to return a random European country.
app.MapGet("/randomeuropeancountry", () =>
{
    var countries = new[]
    {
        new { Country = "Spain", IsoCode = "ES" },
        new { Country = "France", IsoCode = "FR" },
        new { Country = "Germany", IsoCode = "DE" },
        new { Country = "Italy", IsoCode = "IT" },
        new { Country = "Portugal", IsoCode = "PT" },
        new { Country = "Netherlands", IsoCode = "NL" },
        new { Country = "Belgium", IsoCode = "BE" },
        new { Country = "Sweden", IsoCode = "SE" },
        new { Country = "Norway", IsoCode = "NO" },
        new { Country = "Denmark", IsoCode = "DK" }
    };

    var random = new Random();
    var randomCountry = countries[random.Next(countries.Length)];
    return Results.Ok(randomCountry);
});

app.Run(); // Runs the application.

// Needed to be able to access this type from the MinimalAPI.Tests project.
public partial class Program
{ }