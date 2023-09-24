namespace LibraryManagement.Models;

public class BookViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int CategoryId { get; set; }
    public string Status { get; set; }
}