    using System;
    using System.Linq;
    using System.Collections.Generic;
    namespace CsFun2;
    class Program{
        static void Main(string[] args)
        {
            List<Member> members = new List<Member>();

            members.Add(new Member("Nguyen Van","Nam","Nam",new DateTime(1999,06,02),"0945628812","VietNam",24,true));
            members.Add(new Member("Do Tuan","Duc","Nam",new DateTime(2000,11,08),"0938428762","Ha Noi",23,false));
            members.Add(new Member("Hoang Thanh","Huong","Nu",new DateTime(2002,4,20),"0948348712","VietNam",21,true));
            // Male members
            Console.WriteLine("Members are male:");
            var maleMembers = members.Where(member => member.Gender == "Nam");
            foreach (var member in maleMembers)
            {
                Console.WriteLine(member);
            }
            Console.WriteLine(" ");
            // Oldest member
            var oldestMember = members.MinBy(member => member.Dob);
            if (oldestMember != null)
            {
                Console.WriteLine("Member is the oldest:");
                Console.WriteLine(oldestMember);
                Console.WriteLine(" ");
            }
            // New list FullName = last + first
            Console.WriteLine("FullName List:");
            var memberInfos = members.Select(member => new
            {
                FullName = member.LastName + " " + member.FirstName,
                member.Gender,
                Dob = member.Dob.ToString("dd/MM/yyyy"),
                member.PhoneNumber,
                member.BirthPlace,
                member.Age,
                IsGraduated = (member.IsGraduated ? "Yes" : "No")
            });
            foreach (var memberInfo in memberInfos)
            {
                Console.WriteLine(memberInfo);
            }
            Console.WriteLine(" ");
            // Return 3 lists:
            // List of members who have birth year 2000
            // List of members who have birth year greater than 2000
            // List of members who have birth year less than 2000
            var membersBornIn2000 = members.Where(member => member.Dob.Year == 2000);
            var membersBornAfter2000 = members.Where(member => member.Dob.Year > 2000);
            var membersBornBefore2000 = members.Where(member => member.Dob.Year < 2000);

            Console.WriteLine("2000");
            foreach (var member in membersBornIn2000)
            {
                Console.WriteLine(member);
            }
            Console.WriteLine(" ");

            Console.WriteLine("tren 2000");
            foreach (var member in membersBornAfter2000)
            {
                Console.WriteLine(member);
            }
            Console.WriteLine(" ");

            Console.WriteLine("duoi 2000");
            foreach (var member in membersBornBefore2000)
            {
                Console.WriteLine(member);
            }
            Console.WriteLine(" ");
            // The first person born in Ha Noi
            var firstPersonFromHanoi = members.FirstOrDefault(member => member.BirthPlace == "Ha Noi");
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
