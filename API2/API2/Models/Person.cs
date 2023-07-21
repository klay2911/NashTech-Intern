using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API2.Models;

public class Person
{
    private static int _lastId ;
    [Key]
    public int Id { get; set; }
    [DisplayName("First Name")]
    [BindRequired]
    [MinLength(2)]
    public String FirstName { get; set; }
    [BindRequired]
    [MinLength(2)]
    public String LastName { get; set; }
    public string FullName => FirstName + " " + LastName;
    
    //[Required(ErrorMessage = "Gender is required")]
    public String Gender { get; set; }
    public String BirthPlace { get; set; }

    public Person()
    {
        Id = ++_lastId;
    }
    public Person( string firstName, string lastName, string gender, string birthPlace)
    {
        Id = ++_lastId;
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        BirthPlace = birthPlace;
    }

    public override string ToString()
    {
        return "Id" + Id + " | First Name:" + FirstName + " | Last Name:" + LastName + " | Gender:" + Gender +  " | Birth Place:" + BirthPlace;

    }
}