namespace LibraryManagement.Models;

public class BookBorrowingRequestDetails
{
    public int RequestDetailsId { get; set; }
    public int RequestId { get; set; }
    
    public int BookId { get; set; }
    
    public ICollection<Book> Books { get; set; } = new List<Book>();
    public BookBorrowingRequest? BookBorrowingRequest { get; set; }
}