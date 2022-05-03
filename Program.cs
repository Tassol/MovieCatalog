var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

app.UseSwagger();
app.UseHttpsRedirection();


var movies = new List<Movie>();
movies.Add(new Movie(1, "The Godfather", "Crime", 90));
movies.Add(new Movie(2, "Titanic", "Romance", 85));
movies.Add(new Movie(3, "The Exorcist", "Horror", 80));


app.MapGet("/movies/{pageSize}/{page}", (int pageSize, int page) =>
{
    List<Movie> result = null;
    try
    {
        result = movies
           .Skip(pageSize * (page - 1))
           .Take(pageSize)
           .ToList();

    }
    catch
    {
        return Results.BadRequest("Invalid Page Information");
    }

    return Results.Ok(result);

});

app.MapPost("", (Movie movie) =>
{
    try
    {

        bool movieExists = movies.Any(m => m.Id == movie.Id);

        if (movieExists)
        {
            throw new ApplicationException("Movie already exists");
        }

        movies.Add(movie);

    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
    return Results.Ok(movies);
});

app.MapPut("", (Movie movie) =>
{
    try
    {

        var existingMovie = movies.FirstOrDefault(m => m.Id == movie.Id);

        if (existingMovie is null)
        {
            throw new ApplicationException("Cannot find movie");
        }

        existingMovie = movie;

    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }

    return Results.Ok(movies);
});

app.MapDelete("/{id}", (int id) =>
{

    try
    {

        var movie = movies.FirstOrDefault(m => m.Id == id);

        if (movie is null)
        {
            throw new ApplicationException("Cannot find movie");
        }

        movies.Remove(movie);

    }
    catch (Exception ex)
    {

        return Results.BadRequest(ex.Message);
    }

    return Results.Ok(movies);
});

app.UseSwaggerUI();
app.Run();

public record Movie(int Id, string Name, string Genre, int Rating);

