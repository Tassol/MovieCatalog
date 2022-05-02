var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

app.UseSwagger();
app.UseHttpsRedirection();

var movies = new List<Movie>();
movies.Add(new Movie { Id = 1, Name = "The Godfather", Genre = "Crime", Rating = 90 });
movies.Add(new Movie { Id = 2, Name = "Titanic", Genre = "Romance", Rating = 85 });
movies.Add(new Movie { Id = 3, Name = "The Exorcist", Genre = "Horror", Rating = 80 });


app.MapGet("/getallmovies", () =>
{
    return movies;
});

app.MapPost("", (Movie movie) =>
{
    movies.Add(movie);
    return movies;
});

app.MapPut("", (Movie movie) =>
{
    var index = movies.FindIndex(m => m.Id == movie.Id);
    movies[index] = movie;
    return movies;
});

app.MapDelete("/{id}", (int id) =>
{
    var index = movies.FindIndex(m => m.Id == id);
    movies[index] = null;
    return movies;
});

app.UseSwaggerUI();
app.Run();


