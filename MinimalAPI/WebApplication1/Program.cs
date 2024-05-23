using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPI;
using MinimalAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PersonDb>(opt => opt.UseInMemoryDatabase("PersonList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();
app.MapGet("/", () => "MinimalAPI");

app.MapGet("/person_items", async (PersonDb db) =>
    await db.Persons.ToListAsync());

app.MapGet("/person_items/{id}", async (int id, PersonDb db) =>
{ 
    var person = await db.Persons.FindAsync(id);
    if (person == null)
    {
        return Results.NotFound(new { Message = $"Person with ID {id} not found" });
    }
    return Results.Ok(person);
});

app.MapPost("/person_items", async (Person person, PersonDb db) =>
{
    var context = new ValidationContext(person, serviceProvider: null, items : null);
    var results = new List<ValidationResult>();

    if (!Validator.TryValidateObject(person, context, results, true))
    {
        var errors = results.Select(x => new { Field = x.MemberNames.First(), Message = x.ErrorMessage }).ToList();
        var errorDictonary = new Dictionary<string, string[]>();
        errors.ForEach(x =>
        {
            if (x.Field != null && x.Message != null)
                errorDictonary.Add(x.Field, new[] { x.Message });
        });
        return Results.BadRequest(errorDictonary);
    }

    db.Persons.Add(person);
    await db.SaveChangesAsync();

    return Results.Created($"/person_items/{person.Id}", person);
});

app.MapPut("/person_items/{id}", async (int id, Person inputPerson, PersonDb db) =>
{
    var context = new ValidationContext(inputPerson, serviceProvider: null, items: null);
    var results = new List<ValidationResult>();

    if (!Validator.TryValidateObject(inputPerson, context, results, true))
    {
        var errors = results.Select(x => new { Field = x.MemberNames.First(), Message = x.ErrorMessage }).ToList();
        var errorDictonary = new Dictionary<string, string[]>();
        errors.ForEach(x => {
        if (x.Field != null && x.Message != null)
            errorDictonary.Add(x.Field, new[] { x.Message });
        });
        return Results.BadRequest(errorDictonary);
    }


    var person = await db.Persons.FindAsync(id);

    if (person is null) return Results.NotFound();

    person.FirstName = inputPerson.FirstName;
    person.LastName = inputPerson.LastName;
    person.BirthDate = inputPerson.BirthDate;
    person.Address = inputPerson.Address;

    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapDelete("/person_items/{id}", async (int id, PersonDb db) =>
{
    var person = await db.Persons.FindAsync(id);
    if (person == null)
    {
        return Results.NotFound(new { Message = $"Person with ID {id} not found" });
    }

    db.Persons.Remove(person);
    await db.SaveChangesAsync();
    return Results.NoContent();

});


app.Run();