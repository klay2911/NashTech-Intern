using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC3.Interfaces;
using MVC3.Models;
using X.PagedList;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace MVC3.Controllers;

public class PersonController : Controller
{
    private readonly IPerson _people;
    private readonly ILogger<PersonController> _logger;
    
    public PersonController(IPerson people, ILogger<PersonController> logger)
    {
        _people = people;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult Index(int? page)
    {
        page ??= 1;
        int pageSize = 3;
        int pageNumber = (int)page;
        var people = _people.GetAll().OrderBy(p => p.FullName).ToPagedList(pageNumber, pageSize);
        return View(people);
    }


    public IActionResult Privacy()
    {
        var result = Get("DeletedPersonName");
        return Content(result);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Person person)
    {
        _people.Create(person);
        TempData["Message"] = $"Person {person.FullName} was added from the list successfully!";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var person = _people.GetById(id);
        return View(person);
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var person = _people.GetById(id);
        TempData["Message"] = $"Person {person.FullName} was modified successfully!";
        return View(person);
    }

    [HttpPost]
    public IActionResult Edit(int id, Person updatedPerson)
    {
        if (id != updatedPerson.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            _people.Update(id, updatedPerson);
            return RedirectToAction("Index");
        }
        return View(updatedPerson);
    }


    [HttpPost]
    public IActionResult Delete(int id)
    {
        var person = _people.GetById(id);

        _people.Delete(id);
        
        TempData["Message"] = $"Person {person.FullName} was removed from the list successfully!";

        Set("DeletedPersonName", person.FullName);

        return RedirectToAction("Index");
    }

    private void Set(String key, string value)
    {
        CookieOptions options = new CookieOptions();
        Response.Cookies.Append(key,value, options);
    }

    private String Get(string key)
    {
        return Request.Cookies[key]!;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}