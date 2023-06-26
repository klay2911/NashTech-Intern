namespace CsFun;

public class MemberManager
{
    // Male member
    public void PrintMaleMembers(List<Member> members)
        {
            Console.WriteLine("Members are male:");
            foreach (Member member in members)
            {
                if (member.Gender == "Nam")
                    Console.WriteLine(member);
            }
            Console.WriteLine(" ");
        }
    // Oldest member
    public void FindOldestMember(List<Member> members)
    {
        Member oldestMember = members[0];

        for (int i = 1; i < members.Count; i++)
        {
            if (members[i].Dob < oldestMember.Dob)
            {
                oldestMember = members[i];
            }
        }

        Console.WriteLine("Oldest member:");
        Console.WriteLine(oldestMember);
        Console.WriteLine(" ");
    }
    
    //FullName list
    public void PrintMemberDetails(List<Member> members)
    {
        Console.WriteLine("Fullname List:");
        foreach (Member member in members)
        {
            Console.WriteLine("Full Name: " + member.FullName);
            Console.WriteLine("Gender: " + member.Gender);
            Console.WriteLine("Date of Birth: " + member.Dob.ToShortDateString());
            Console.WriteLine("Phone Number: " + member.PhoneNumber);
            Console.WriteLine("Birth Place: " + member.BirthPlace);
            Console.WriteLine("Age: " + member.Age);
            Console.WriteLine("Is Graduated: " + member.IsGraduated);
            Console.WriteLine(" ");
        }
    }
    //Return 3 lists:
    //List of members who has birth year is 2000
    //List of members who has birth year greater than 2000
    //List of members who has birth year less than 2000
    public void PrintMembersByYear(List<Member> members)
    {
        List<Member> year2000 = new List<Member>();
        List<Member> above2000 = new List<Member>();
        List<Member> below2000 = new List<Member>();

        foreach (Member member in members)
        {
            var year = member.Dob.Year;
            if (year == 2000)
            {
                year2000.Add(member);
            }
            else if (year > 2000)
            {
                above2000.Add(member);
            }
            else
            {
                below2000.Add(member);
            }
        }

        Console.WriteLine("Members born in 2000:");
        foreach (Member member in year2000)
        {
            Console.WriteLine(member);
        }

        Console.WriteLine("\nMembers born after 2000:");
        foreach (Member member in above2000)
        {
            Console.WriteLine(member);
        }

        Console.WriteLine("\nMembers born before 2000:");
        foreach (Member member in below2000)
        {
            Console.WriteLine(member);
        }
    }
    
    //The first person born in Ha Noi
    public void FindFirstPersonFromHanoi(List<Member> members)
    {
        Member firstPersonFromHanoi = null;
        foreach (var member in members)
        {
            if (member.BirthPlace == "Ha Noi")
            {
                firstPersonFromHanoi = member;
                break;
            }
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
    }

}