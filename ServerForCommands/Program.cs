using IziHardGames.YandexApi;

Console.OutputEncoding = System.Text.Encoding.Unicode; 
Console.InputEncoding = System.Text.Encoding.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();

//app.MapDefaultControllerRoute();

app.MapGet("/yandexapi/debug", async (x) =>
{
    await x.Response.WriteAsync($"This is Request from:[{x.GetEndpoint()!.DisplayName}]. Path:[{x.Request.Path}]").ConfigureAwait(false);
});

app.Run();
