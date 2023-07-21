using MVC3.Models;

namespace MVC3.Interfaces;

public interface IPerson
{
    List<Person> GetAll(); 
    Person GetById(int id);
    void Create(Person person);
    void Update(int id, Person updatedPerson);
    void Delete(int id);
    
}