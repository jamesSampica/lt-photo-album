using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("photos", httpClient => httpClient.BaseAddress = new("https://jsonplaceholder.typicode.com/photos"));

var app = builder.Build();

app.MapGet("/{id?}", async (HttpContext context, IHttpClientFactory clientFactory, int? id, CancellationToken token) =>
{
    context.Response.ContentType = "text/html";

    IEnumerable<Photo> photos = Enumerable.Empty<Photo>();
    if (id.HasValue)
    {
        var albumClient = clientFactory.CreateClient("photos");

        var albumResponse = await albumClient.GetAsync($"?albumId={id}", token);
        photos = (await albumResponse.Content.ReadFromJsonAsync<IEnumerable<Photo>>(cancellationToken: token))!;
    }

    return $"""
           <!DOCTYPE html>
           <html>
            <body>
                <a data-testid='luckyBtn' href="/" onclick="this.href = '/' + Math.trunc(Math.random() * 100 + 1);">Press Me :)</a><br>
                <div data-testid='photo' style="margin-top:10px">
                    {string.Join(" ", photos.Select(PhotoDisplay))}
                </div>  
            </body>
           </html>
           """;

    static string PhotoDisplay(Photo p) => $"<div style='margin-top:10px;'><img src='{p.ThumbnailUrl}' /><br/> [{p.Id}] {p.Title}</div>";
});

await app.RunAsync();

record Photo([property: JsonPropertyName("id")] int Id,
             [property: JsonPropertyName("title")] string Title,
             [property: JsonPropertyName("thumbnailUrl")] string ThumbnailUrl);

// Make Program.cs public so the tests can see it, otherwise they error :(
public partial class Program { }