
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models;

public class BookBorrowingRequest
{
    public int RequestId { get; set; }
    public int UserId { get; set; }
    public DateTime DateRequested { get; set; }
    public string Status { get; set; }
    public int LibrarianId { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public User? User { get; set; }
    public ICollection<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; } = new List<BookBorrowingRequestDetails>();
}