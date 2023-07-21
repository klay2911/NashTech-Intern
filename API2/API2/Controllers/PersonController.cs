using API2.Interfaces;
using API2.Models;
using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers;


[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private static IPerson _people;
    public PersonController(IPerson people)
    {
        _people = people;
    }
    // 1. Bulk add multiple tasks in one request
    [HttpPost("bulk")]
    public IActionResult CreateBulk(List<Person> people)
    {
        _people.Create(people);
        return NoContent();
    }
    
    // 2. Edit the title or completion of a specific task
    [HttpPut("{id}")]
    public IActionResult Update(int id, Person updatedPerson)
    { 
        _people.Update(id, updatedPerson);
        return NoContent();
    }
    
    // 3. Bulk delete multiple tasks in one request
    [HttpDelete("bulk")]
    public IActionResult DeleteBulk(List<int> ids)
    {
        _people.Delete(ids);
        return NoContent();
    }
    // 4. List people
    [HttpGet]
    public IActionResult Get()
    {
        var person = _people.GetAll();
        return Ok(person);
    }

    // 5. Get a specific task
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var person = _people.GetById(id);
        return Ok(person);
    }

    [HttpGet("filter")]
    public IActionResult FilterNames([FromQuery] Dictionary<string, string> filters)
    {
        var people =  _people.GetByFilter(filters);
        return Ok(people);
    }
}