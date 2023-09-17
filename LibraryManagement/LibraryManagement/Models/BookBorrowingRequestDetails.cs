namespace LibraryManagement.Models;

public class BookBorrowingRequestDetails
{
    public int RequestId { get; set; }
    
    public int BookId { get; set; }
    
    public Book? Book { get; set; }
    public BookBorrowingRequest? BookBorrowingRequest { get; set; }
}