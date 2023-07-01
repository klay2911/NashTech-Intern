using System.Data;
using System.Diagnostics;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using FileResult = Microsoft.AspNetCore.Mvc.FileResult;


namespace MVC.Controllers;

public class RookiesController : Controller
{
    private readonly ILogger<RookiesController> _logger;
    public RookiesController(ILogger<RookiesController> logger)
    {
        _logger = logger;
    }
    
    static List<Person> _people = new()
   {
        new Person("Nguyen Van", "Nam", "Nam", new DateTime(1999, 06, 02), "0945628812", "Nam Dinh"),
        new Person("Do Tuan", "Duc", "Nam", new DateTime(2000, 11, 08), "0938428762", "Ha Noi"),
        new Person("Hoang Thanh", "Huong", "Nu", new DateTime(2002, 4, 20), "0948348712", "VietNam")
    };
    
    public IActionResult Index()
    {
        return View(_people);
    }
    
    public IActionResult MalePersons()
    {
        // Filter the list to include only male members
        var malePersons = _people.Where(person => person.Gender == "Nam").ToList();

        // Return the filtered list
        return View(malePersons);
    }

    public IActionResult OldestPerson()
    {
        // Find the oldest member based on date of birth
        var oldestPerson = _people.OrderBy(person => person.Dob).FirstOrDefault();

        // Return the oldest member
        return View(oldestPerson);
    }

    public IActionResult FullNames()
    {
        // Create a new list that contains all Person objects
        var persons = _people.ToList();

        // Return the list of Person objects
        return View(persons);
    }
    
    public IActionResult Redirect(int year = 1999)
    {
        switch (year)
        {
            case 2000:
                return RedirectToAction("BirthIs2000");
            case >2000:
                return RedirectToAction("BirthGreaterThan2000");
            case <2000:
                return RedirectToAction("BirthLessThan2000");
        }
    }
    public IActionResult BirthIs2000()
    {
        var persons = _people.Where(p => p.Dob.Year == 2000).ToList();
        return View(persons);
    }
    public IActionResult BirthGreaterThan2000()
    {
        var persons = _people.Where(p => p.Dob.Year > 2000).ToList();
        return View(persons);
    }
    public IActionResult BirthLessThan2000()
    {
        var persons = _people.Where(p => p.Dob.Year < 2000).ToList();
        return View(persons);
    }

    [HttpPost]
    public FileResult Export()
    {
        DataTable dt = new DataTable("Grid");
        dt.Columns.AddRange(new[]
        {
            new DataColumn("FirstName"),
            new DataColumn("LastName"),
            new DataColumn("Age"),
            new DataColumn("Birth Place")
        });

        foreach (var persons in _people)
        {
            dt.Rows.Add(persons.FirstName, persons.LastName, persons.Age, persons.BirthPlace);
        }

        using XLWorkbook wb = new XLWorkbook();
        wb.Worksheets.Add(dt); 
        //saved to a MemoryStream object
        //research using and stream, memoryStream
        using (MemoryStream stream = new MemoryStream())
        {
            wb.SaveAs(stream);
            //converted to Byte Array and exported and downloaded as Excel file using the File function
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
        }
    }
    //create and display in list
    public IActionResult Create()
    {
        return View();
    } 
    [HttpPost]
    public IActionResult Create(Person person)
      {
          _people.Add(person);
          return RedirectToAction("Index");
      }
    [HttpPost]
    public IActionResult AddMore(Person person)
    {
        _people.Add(person);
        TempData["Message"] = $"Person {person.FullName} created successfully";
        return RedirectToAction("Create");
    }
    public ActionResult Edit(int id)
    {
        var person = _people.FirstOrDefault(s => s.Id == id);
        return View(person);
    }
    [HttpPost]
    public ActionResult EditSuccess(int id, Person updatedPerson)
    {
        var person = _people.FirstOrDefault(s => s.Id == id);
        if (person != null)
        {
            person.FirstName = updatedPerson.FirstName;
            person.LastName = updatedPerson.LastName;
            person.Gender = updatedPerson.Gender;
            person.Dob = updatedPerson.Dob;
            person.PhoneNumber = updatedPerson.PhoneNumber;
            person.BirthPlace = updatedPerson.BirthPlace;
        }
        return RedirectToAction("Index");
    }
    public ActionResult Details(int id)
    {
      var person = _people.FirstOrDefault(s => s.Id == id);
      return View(person);
    }
    public ActionResult Delete(int id) 
    {
      var person = _people.FirstOrDefault(s => s.Id == id);
      return View(person);
    }
    //Case null cant find id of the person 
    [HttpPost]
    public ActionResult DeleteConfirm(int id) 
    { 
        var person = _people.FirstOrDefault(s => s.Id == id);
        if (person != null) _people.Remove(person);
        return RedirectToAction("Index");
    }
  
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}