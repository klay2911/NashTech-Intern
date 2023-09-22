
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models;

public class BookBorrowingRequest
{
    // public BookBorrowingRequest()
    // {
    //     
    // }
    // public BookBorrowingRequest(int userId, DateTime dateRequested, string status)
    // {
    //     UserId = userId;
    //     DateRequested = dateRequested;
    //     Status = status;
    // }
    public int RequestId { get; set; }
    public int UserId { get; set; }
    
    //[DataType(DataType.Date)]
    public DateTime DateRequested { get; set; }
    public string Status { get; set; }
    public int LibrarianId { get; set; }
    public User? User { get; set; }
    public ICollection<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; } = new List<BookBorrowingRequestDetails>();
}