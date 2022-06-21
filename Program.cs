global using Microsoft.EntityFrameworkCore;
global using MinimalAPI.Data;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContextClass>(options => options.UseSqlServer("name=ConnectionStrings:VOORBEELDConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/HaalOpEigenaren", async (DbContextClass dbContextClass) =>
{
    try
    {
        return Results.Ok(await dbContextClass.EIGENAAR.ToListAsync());
    }
    catch (Exception ex)
    {
        return Results.Problem("Er is iets fout gegaan met minimal API HaalOpEigenaren - " + ex.Message);
    }
});

app.MapGet("HaalOpEigenaar/{ID}", async (DbContextClass dbContextClass, int ID) =>
{
    try
    {
        return Results.Ok(await dbContextClass.EIGENAAR.FindAsync(ID) is EIGENAAR eigenaar ? Results.Ok(eigenaar) : Results.NotFound("Niks gevonden"));
    }
    catch (Exception ex)
    {
        return Results.Problem("Er is iets fout gegaan met minimal API HaalOpEigenaar - " + ex.Message);
    }
});

app.MapPost("/VoegToe", async (DbContextClass dbContextClass, EIGENAAR eigenaar) =>
{
    try
    {
        dbContextClass.EIGENAAR.Add(eigenaar);
        await dbContextClass.SaveChangesAsync();
        return Results.Ok(await dbContextClass.EIGENAAR.FirstOrDefaultAsync(x => x.ID == eigenaar.ID));
    }
    catch (Exception ex)
    {
        return Results.Problem("Er is iets fout gegaan met minimal API VoegToe - " + ex.Message);
    }
});

app.MapPut("/Muteer", async (DbContextClass dbContextClass, EIGENAAR eigenaar) =>
{
    try
    {
        var eigenaarOrg = await dbContextClass.EIGENAAR.FindAsync(eigenaar.ID);
        if (eigenaarOrg == null) return Results.NotFound("Niks gevonden");

        eigenaarOrg.Omschrijving = eigenaar.Omschrijving;
        eigenaarOrg.Voornaam = eigenaar.Voornaam;
        eigenaarOrg.Achternaam = eigenaar.Achternaam;
        eigenaarOrg.Regio = eigenaar.Regio;

        await dbContextClass.SaveChangesAsync();

        return Results.Ok(await dbContextClass.EIGENAAR.FirstOrDefaultAsync(x => x.ID == eigenaar.ID));
    }
    catch (Exception ex)
    {
        return Results.Problem("Er is iets fout gegaan met minimal API Muteer - " + ex.Message);
    }
});

app.MapDelete("Verwijder/{ID}", async (DbContextClass dbContextClass, int ID) =>
{
    try
    {
        var eigenaarOrg = await dbContextClass.EIGENAAR.FindAsync(ID);
        if (eigenaarOrg == null) return Results.NotFound(false);

        dbContextClass.EIGENAAR.Remove(eigenaarOrg);
        await dbContextClass.SaveChangesAsync();

        return Results.Ok(true);
    }
    catch (Exception ex) 
    {
        return Results.Problem("Er is iets fout gegaan met minimal API Verwijder - " + ex.Message);
    }
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();