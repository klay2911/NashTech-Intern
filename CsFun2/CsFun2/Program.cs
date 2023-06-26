namespace CsFun2;

class Program
{
    static void Main(string[] args)
    {
        List<Member> members = new List<Member>
        {
            new Member("Nguyen Van", "Nam", "Nam", new DateTime(1999, 06, 02), "0945628812", "VietNam"),
            new Member("Do Tuan", "Duc", "Nam", new DateTime(2000, 11, 08), "0938428762", "Ha Noi"),
            new Member("Hoang Thanh", "Huong", "Nu", new DateTime(2002, 4, 20), "0948348712", "VietNam")
        };
        bool exit = false;
        while (!exit)
        {
            MemberManager manager = new MemberManager();
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Male members");
            Console.WriteLine("2. Oldest member");
            Console.WriteLine("3. FullName members");
            Console.WriteLine("4. 3 lists member sort by year of birth");
            Console.WriteLine("5. First born in Ha Noi: ");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");

            string? input = Console.ReadLine();
            int choice;
            if (int.TryParse(input, out choice))
            {
                if (choice == 1)
                {
                    manager.PrintMaleMembers(members);
                }
                else if (choice == 2)
                {
                    manager.FindOldestMember(members);
                }
                else if (choice == 3)
                {
                    manager.PrintMemberDetails(members);
                }
                else if (choice == 4)
                {
                    manager.PrintMembersByYear(members);
                }
                else if (choice == 5)
                {
                    manager.FindFirstPersonFromHanoi(members);
                }
                else if (choice == 6)
                {
                    // Exit the program
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
            }

        }
    }
}


