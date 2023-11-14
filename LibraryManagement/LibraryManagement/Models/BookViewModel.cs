namespace LibraryManagement.Models;

public class BookViewModel
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int CategoryId { get; set; }
    public string Status { get; set; }
    public string? Description { get; set; }
    public string Cover{ get; set; }
    public string PdfFilePath { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int LastReadPageNumber { get; set; }

}
