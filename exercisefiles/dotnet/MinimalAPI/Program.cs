using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ADD NEW ENDPOINTS HERE
app.MapGet("/", () => "Hello World!");

app.Run();

// DaysBetweenDates
app.MapGet("/daysbetweendates", (DateTime date1, DateTime date2) =>
{
    var daysBetween = (date2 - date1).TotalDays;
    return Results.Ok(daysBetween);
});

// validatephonenumber
// receive by querystring a parameter called phoneNumber
// validate phoneNumber with Spanish format, for example +34666777888
// if phoneNumber is valid return true
// if phoneNumber is not valid return false
app.MapGet("/validatephonenumber", (string phoneNumber) =>
{
    var isValid = Regex.IsMatch(phoneNumber, @"^\+[1-9]{1}[0-9]{3,14}$");
    return Results.Ok(isValid);
});

// validatespanishdni
// receive by querystring a parameter called dni
// calculate DNI letter
// if DNI is valid return "valid"
// if DNI is not valid return "invalid"
app.MapGet("/validatespanishdni", (string dni) =>
{
    var dniNumber = int.Parse(dni.Substring(0, 8));
    var dniLetter = dni.Substring(8, 1).ToUpper();
    var letter = "TRWAGMYFPDXBNJZSQVHLCKE";
    var valid = dniLetter == letter[dniNumber % 23].ToString();
    return Results.Ok(valid ? "valid" : "invalid");
});

// returncolorcode
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

app.Run();

public class Color
{
    public string Name { get; set; }
    public string Rgba { get; set; }
    public string Hex { get; set; }
}

// tellmeajoke
// Make a call to the joke api and return a random joke
app.MapGet("/tellmeajoke", async () =>
{
    var client = new HttpClient();
    var response = await client.GetAsync("https://official-joke-api.appspot.com/random_joke");
    var joke = await response.Content.ReadAsStringAsync();
    return Results.Ok(joke);
});

// moviesbytitle
// Receive by querystring a parameter called director
// Make a call to the movie api and return a list of movies of that director
// Return the full list of movies
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

// parseurl
// Retrieves a parameter from querystring called someurl
// Parse the url and return the protocol, host, port, path, querystring and hash
// Return the parsed host
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

app.Run();

// listfiles
// Get the current directory
// Get the list of files in the current directory
// Return the list of files
app.MapGet("/listfiles", () =>
{
    var files = Directory.GetFiles(Directory.GetCurrentDirectory());
    return Results.Ok(files);
});

app.Run();

// calculatememoryconsumption
// Return the memory consumption of the process in GB, rounded to 2 decimals
app.MapGet("/calculatememoryconsumption", () =>
{
    var memory = Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024 * 1024.0);
    return Results.Ok(Math.Round(memory, 2));
});

app.Run();

// randomeuropeancountry
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

app.Run();



// Needed to be able to access this type from the MinimalAPI.Tests project.
public partial class Program
{ }
