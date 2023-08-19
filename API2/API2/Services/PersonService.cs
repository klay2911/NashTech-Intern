using API2.Interfaces;
using API2.Models;
using Microsoft.EntityFrameworkCore;

namespace API2.Services;

public class PersonService : IPerson
{
    private readonly List<Person> _people = new();
    private static int _lastId = 3 ;

    private static class FilterNames
    {
        public const string Name = "Name";
        public const string Gender = "Gender";
        public const string BirthPlace = "BirthPlace";
    }

    public PersonService()
    {
        _people.Add(new Person("Nguyen Van", "Nam", "Nam", "Nam Dinh"));
        _people.Add(new Person("Do Tuan", "Duc", "Nam", "Ha Noi"));
        _people.Add(new Person("Hoang Thanh", "Huong", "Nu", "VietNam"));
    }

    public void Create(List<Person> people)
    {
        foreach (var person in people)
        {
            person.Id = ++_lastId;
            _people.Add(person);
        }
    }

    public void Update(int id, Person updatedPerson)
    {
        var person = GetById(id);
        person.FirstName = updatedPerson.FirstName;
        person.LastName = updatedPerson.LastName;
        person.Gender = updatedPerson.Gender;
        person.BirthPlace = updatedPerson.BirthPlace;
    }

    public void Delete(List<int> ids)
    {
        _people.RemoveAll(person => ids.Contains(person.Id));
    }

    public List<Person> GetAll()
    {
        return _people;
    }

    public Person GetById(int id)
    {
        
        return _people.FirstOrDefault(person =>person.Id == id);
    }
    public List<Person> GetByFilter( Dictionary<string, string> filters)
    {
        var query = _people.AsQueryable();
        
        if (filters.ContainsKey(FilterNames.Name))
        {
            query = query.Where(p => (p.FirstName.Contains(filters[FilterNames.Name])) || (p.LastName.Contains(filters[FilterNames.Name])));
        }

        if (filters.TryGetValue(FilterNames.Gender, out var filter))
        {
            query = query.Where(p => p.Gender == filter);
        }

        if (filters.TryGetValue(FilterNames.BirthPlace, out var filter1))
        {
            query = query.Where(p => p.BirthPlace == filter1);
        }

        return query.ToList();
    }

}