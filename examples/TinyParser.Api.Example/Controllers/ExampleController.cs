using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TinyParser.Api.Example.Controllers;

[ApiController]
[Route("[controller]")]
public class ExampleController : Controller
{
    [HttpGet]
    [UseFilter<Person>]
    public IActionResult Get([BindNever] Func<Person, bool> filter)
    {
        var people = new List<Person>()
        {
            new Person() { Name = "Matthew", Age = 22 },
            new Person() { Name = "Sarah", Age = 25 },
            new Person() { Name = "Diego", Age = 27 },
            new Person() { Name = "Lerry", Age = 30 },
            new Person() { Name = "Lobato", Age = 21 },
        };

        return Ok(people.Where(filter));
    }
}

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
