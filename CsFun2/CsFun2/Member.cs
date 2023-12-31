namespace CsFun2;

public class Member
{
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public string FullName => FirstName + " " + LastName;
    public String Gender { get; set; }
    public DateTime Dob { get; set; }
    public String PhoneNumber { get; set; }
    public String BirthPlace { get; set; }
    public int Age => DateTime.Now.Year - Dob.Year;
    public bool IsGraduated => Age > 22;

    public Member(string firstName, string lastName, string gender, DateTime dob, string phoneNumber, string birthPlace)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Dob = dob;
        PhoneNumber = phoneNumber;
        BirthPlace = birthPlace;
    }
    public override string ToString()
    {
        return "First Name:" + FirstName + " | Last Name:" + LastName + " | Gender:" + Gender + " | Dob:" + Dob.ToString("dd/MM/yyyy") +
               " | Phone Number:" + PhoneNumber + " | Birth Place:" + BirthPlace + " | Age:" + Age +
               " | Is Graduated:" + (IsGraduated ? "Yes" : "No");

    }
}