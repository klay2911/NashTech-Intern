using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC3.Models;
//[RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
//public String? FirstName { get; set; }
//[Required(AllowEmptyStrings = true)]
//[MaxLength(10)]
//[Range(10,15)]
//validate
public class Person
{
    private static int _lastId;
    public int Id { get; set; }
    [DisplayName("First Name")]
    public String FirstName { get; set; }
    [Required]
    public String LastName { get; set; }
    public string FullName => FirstName + " " + LastName;
    public String Gender { get; set; }
    public DateTime Dob { get; set; }
    public String PhoneNumber { get; set; }
    public String BirthPlace { get; set; }
    public int Age => DateTime.Now.Year - Dob.Year;
    public bool IsGraduated => Age > 22;
    
    public Person()
    {
        Id = ++_lastId;
    }
    public Person( string firstName, string lastName, string gender, DateTime dob, string phoneNumber, string birthPlace)
    {
        Id = ++_lastId;
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Dob = dob;
        PhoneNumber = phoneNumber;
        BirthPlace = birthPlace;
    }
    public override string ToString()
    {
        return "Id" + Id + " | First Name:" + FirstName + " | Last Name:" + LastName + " | Gender:" + Gender + " | Dob:" + Dob.ToString("dd/MM/yyyy") +
               " | Phone Number:" + PhoneNumber + " | Birth Place:" + BirthPlace + " | Age:" + Age +
               " | Is Graduated:" + (IsGraduated ? "Yes" : "No");

    }
}