using API2.Models;

namespace API2.Interfaces;

public interface IPerson
{
    List<Person> GetAll(); 
    Person GetById(int id);
    void Create(List<Person> people);
    void Update(int id, Person updatedPerson);
    void Delete(List<int> ids);
    public List<Person> GetByFilter(Dictionary<string, string> filters);
}