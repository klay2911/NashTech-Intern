
namespace CsFun;
    class Program{
        static void Main(string[] args)
        {
            List<Member> members = new List<Member>();

            members.Add(new Member("Nguyen Van","Nam","Nam",new DateTime(1999,06,02),"0945628812","VietNam",24,true));
            members.Add(new Member("Do Tuan","Duc","Nam",new DateTime(2000,11,08),"0938428762","Ha Noi",23,false));
            members.Add(new Member("Hoang Thanh","Huong","Nu",new DateTime(2002,4,20),"0948348712","VietNam",21,true));
            // Male member
            Console.WriteLine("Members are male:");
            foreach (Member member in members)
            {
                if (member.Gender =="Nam")
                Console.WriteLine(member);
            }
            Console.WriteLine(" ");
            // Oldest memmber
            DateTime earliestDob = DateTime.MaxValue;
            Member oldestMember = null;

            foreach (Member member in members)
            {
                if (member.Dob < earliestDob)
                {
                    earliestDob = member.Dob;
                    oldestMember = member;
                }
            }

            if (oldestMember != null)
            {
                Console.WriteLine("Member is the oldest:");
                Console.WriteLine(oldestMember);
                Console.WriteLine(" ");
            }
            /*Member oldestMember = members[0];
            foreach (var member in members)
            {
                if (member.Dob < oldestMember.Dob)
                {
                    oldestMember = member;
                }
            }*/
            //New list FullName= last + first
            Console.WriteLine("FullName List:");
            var memberInfos = new List<dynamic>();
            foreach (var member in members)
            {
                var memberInfo = new
                {
                    FullName = member.LastName + " " + member.FirstName,
                    member.Gender,
                    Dob = (member.Dob.ToString("dd/MM/yyyy")),
                    member.PhoneNumber,
                    member.BirthPlace,
                    member.Age,
                    IsGraduated = (member.IsGraduated ? "Yes" : "No")
                };
                memberInfos.Add(memberInfo);
                Console.WriteLine(memberInfo);
            }
            Console.WriteLine(" ");
            //Return 3 lists:
            //List of members who has birth yearis 2000
            //List of members who has birth year greater than 2000
            //List of members who has birth year less than 2000
            foreach (Member member in members)
            {
                var year = member.Dob.Year;
                switch (year)
                {
                    case 2000:
                        Console.WriteLine("2000");
                        Console.WriteLine(member);
                        Console.WriteLine(" ");
                        break;
                    case >2000:
                        Console.WriteLine("tren 2000");
                        Console.WriteLine(member);
                        Console.WriteLine(" ");
                        break;
                    default:
                        Console.WriteLine("duoi 2000");
                        Console.WriteLine(member);
                        Console.WriteLine(" ");
                        break;
                }
            }
            //The first person born in Ha Noi
            
            int index = 0;
            Member firstPersonFromHanoi = null;
            while (index < members.Count && firstPersonFromHanoi == null)
            {
                if (members[index].BirthPlace == "Ha Noi")
                {
                    firstPersonFromHanoi = members[index];
                }
                index++;
            }

            if (firstPersonFromHanoi != null)
            {
                Console.WriteLine("The first person born in Ha Noi:");
                Console.WriteLine("First Name: " + firstPersonFromHanoi.FirstName + " | Last Name: " + firstPersonFromHanoi.LastName);
                Console.WriteLine("Gender: " + firstPersonFromHanoi.Gender);
                Console.WriteLine("Dob: " + firstPersonFromHanoi.Dob.ToString("dd/MM/yyyy"));
                Console.WriteLine("Phone Number: " + firstPersonFromHanoi.PhoneNumber);
                Console.WriteLine("Birth Place: " + firstPersonFromHanoi.BirthPlace);
                Console.WriteLine("Age: " + firstPersonFromHanoi.Age);
                Console.WriteLine("Is Graduated: " + (firstPersonFromHanoi.IsGraduated ? "Yes" : "No"));
            }
            else
            {
                Console.WriteLine("No member was born in Ha Noi.");
            }
            Console.ReadKey();
        }
    }

public class Member
{
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public String Gender { get; set; }
    public DateTime Dob { get; set; }
    public String PhoneNumber { get; set; }
    public String BirthPlace { get; set; }
    public Int32 Age { get; set; }
    public Boolean IsGraduated
    {
        get;
        set;
        /*{
            if (value = true)
            {
                IsGraduated = Yes;
            }
            else()
            {
                IsGraduated = "No;
            }
        }*/
    }

    public Member(string firstName, string lastName, string gender, DateTime dob, string phoneNumber, string birthPlace,
        int age, bool isGraduated)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Dob = dob;
        PhoneNumber = phoneNumber;
        BirthPlace = birthPlace;
        Age = age;
        IsGraduated = isGraduated;
    }
    
    public override string ToString()
    {
        return "First Name:" + FirstName + " | Last Name:" + LastName + " | Gender:" + Gender + " | Dob:" + Dob.ToString("dd/MM/yyyy") +
               " | Phone Number:" + PhoneNumber + " | Birth Place:" + BirthPlace + " | Age:" + Age +
               " | Is Graduated:" + (IsGraduated ? "Yes" : "No");

    }
}
