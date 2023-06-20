namespace CsFun3_Clock;
class Program
{
    public class Publisher
    {
        public event Action OnChange = delegate { };

        public void Raise()
        {
            OnChange();
        }
    }

    static void Main(string[] args)
    {
        // Khởi tạo publisher
        var pub = new Publisher();

        pub.OnChange += () => Console.WriteLine(DateTime.Now.ToString("T"));
        for (int i = 0; i < 1000; ++i)
        {
            pub.Raise();
            Thread.Sleep(1000);
        }
    }
}
