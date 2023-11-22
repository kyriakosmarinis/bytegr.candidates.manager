using bytegr.candidates.manager.data.DbContexts;
using bytegr.candidates.manager.data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICandidatesRepository, CandidatesRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDatabase"));
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(10));

#region Sqlite InMemory
/*builder.Services.AddDbContext<AppDbContext>(options =>
{
    var liteConn = new SqliteConnection("DataSource=:memory:");
    liteConn.Open();
    options
        .UseSqlite(liteConn)
        .ConfigureWarnings(warnings =>
        {
            warnings.Throw(RelationalEventId.QueryClientEvaluationWarning);
            warnings.Log(RelationalEventId.ExecutedCommand);
        });
});*/
#endregion

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
        bytegr.candidates.manager.data.DbSeeds.AppDbSeeder.SeedData(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while obtaining the in memory database context.");
        Console.WriteLine(ex.Message);
    }
}

app.Run();

