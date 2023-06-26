namespace CsFun2;

public class MemberManager
{
    // Male members
    public void PrintMaleMembers(List<Member> members)
    {
        Console.WriteLine("Members are male:");
        var maleMembers = members.Where(member => member.Gender == "Nam");
        foreach (var member in maleMembers)
        {
            Console.WriteLine(member);
        }

        Console.WriteLine(" ");
    }

    // Oldest member
    public void FindOldestMember(List<Member> members)
    {
        var oldestMember = members.MinBy(member => member.Dob);
        if (oldestMember != null)
        {
            Console.WriteLine("Member is the oldest:");
            Console.WriteLine(oldestMember);
            Console.WriteLine(" ");
        }
    }

    // New list FullName = last + first
    public void PrintMemberDetails(List<Member> members)
    {
        Console.WriteLine("FullName List:");
        var memberInfos = members.Select(member => new
        {
            FullName = member.LastName + " " + member.FirstName,
            member.Gender,
            Dob = member.Dob.ToString("d"),
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
    }

    // Return 3 lists:
            // List of members who have birth year 2000
            // List of members who have birth year greater than 2000
            // List of members who have birth year less than 2000
            public void PrintMembersByYear(List<Member> members)
            {
                var membersBornIn2000 = members.Where(member => member.Dob.Year == 2000);
                var membersBornAfter2000 = members.Where(member => member.Dob.Year > 2000);
                var membersBornBefore2000 = members.Where(member => member.Dob.Year < 2000);

                Console.WriteLine("2000");
                foreach (var member in membersBornIn2000)
                {
                    Console.WriteLine(member);
                }

                Console.WriteLine(" ");

                Console.WriteLine("above 2000");
                foreach (var member in membersBornAfter2000)
                {
                    Console.WriteLine(member);
                }

                Console.WriteLine(" ");

                Console.WriteLine("under 2000");
                foreach (var member in membersBornBefore2000)
                {
                    Console.WriteLine(member);
                }

                Console.WriteLine(" ");
            }

            // The first person born in Ha Noi
            public void FindFirstPersonFromHanoi(List<Member> members)
            {
                var firstPersonFromHanoi = members.FirstOrDefault(member => member.BirthPlace == "Ha Noi");
                if (firstPersonFromHanoi != null)
                {
                    Console.WriteLine("The first person born in Ha Noi:");
                    Console.WriteLine("First Name: " + firstPersonFromHanoi.FirstName + " | Last Name: " +
                                      firstPersonFromHanoi.LastName);
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