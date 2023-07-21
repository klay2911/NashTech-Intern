using MVC3.Interfaces;
using MVC3.Models;

namespace MVC3.Service;

public class PersonService : IPerson
{
    private List<Person> _people = new();


    public PersonService()
    {
        _people.Add(new Person("Nguyen Van", "Nam", "Nam", new DateTime(1999, 06, 02), "0945628812", "Nam Dinh"));
        _people.Add(new Person("Do Tuan", "Duc", "Nam", new DateTime(2000, 11, 08), "0938428762", "Ha Noi"));
        _people.Add(new Person("Hoang Thanh", "Huong", "Nu", new DateTime(2002, 4, 20), "0948348712", "VietNam"));
    }

    public List<Person> GetAll()
    {
        return _people;
    }

    public Person GetById(int id)
    {
        
        return _people.FirstOrDefault(person =>person.Id == id);
    }

    public void Create(Person person)
    {
        _people.Add(person);
    }

    public void Update(int id, Person updatedPerson)
    {
        var person = GetById(id);
        person.FirstName = updatedPerson.FirstName;
        person.LastName = updatedPerson.LastName;
        person.Gender = updatedPerson.Gender;
        person.Dob = updatedPerson.Dob;
        person.PhoneNumber = updatedPerson.PhoneNumber;
        person.BirthPlace = updatedPerson.BirthPlace;
    }

    public void Delete(int id)
    {
        var person = GetById(id);
        _people.Remove(person);
    }
    
}