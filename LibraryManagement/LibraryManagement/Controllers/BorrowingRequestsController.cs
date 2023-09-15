using System.Security.Claims;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers;

public class BorrowingRequestsController: Controller
{
    private readonly LibraryContext _context;
    public BorrowingRequestsController(LibraryContext context)
    {
        _context = context;
    }
    [HttpPost]
    [Authorize(Roles = "NormalUser")]
    public IActionResult BorrowBooks(BookBorrowingRequest request, BookBorrowingRequestDetails requestDetails)
    {
        
            var userId = Int32.Parse(User.FindFirstValue(ClaimTypes.Name) ?? string.Empty);

            var requestsCount = _context.BorrowingRequests
                .Count(r => r.UserId == userId && r.DateRequested.Month == DateTime.Now.Month);

            if (requestsCount < 3)
            {
                if (requestDetails.BookId.Equals(5))
                {
                    request.UserId = userId;
                    request.DateRequested = DateTime.Now;

                    _context.BorrowingRequests.Add(request);

                    _context.SaveChanges();

                    return Json(new { message = "Your request has been submitted successfully." });
                }
                else
                {
                    ModelState.AddModelError("", "You can only request up to 5 books per request.");
                }
            }
            else
            {
                ModelState.AddModelError("", "You can only make up to 3 requests per month.");
            }
            return View(request);
    }


    /*[HttpGet]
    [Authorize(Roles = "NormalUser")]
    public IActionResult ViewBorrowedBooks()
    {
        var userId = Int32.Parse(User.FindFirstValue(ClaimTypes.Name) ?? string.Empty);

        var requests = _context.BorrowingRequests
            .Include(r => r.BookBorrowingRequestDetails)
            .ThenInclude(b => b.Books)
            .Where(r => r.UserId == userId)
            .ToList();

        return View(requests);
    }*/
}
/*[HttpGet]
[Authorize(Roles = "SuperUser")]
public IActionResult ViewRequests()
{
// Query the BookBorrowingRequests table using Entity Framework Core to get all requests 
var requests = _context.BookBorrowingRequests
.Include(r => r.User)
.Include(r => r.Books)
.ThenInclude(b => b.Book)
.ToList();

// Return the ViewRequests view with the requests as the model data
return View(requests);
}
[HttpPost]
[Authorize(Roles = "SuperUser")]
public IActionResult ApproveRequest(int id)
{
// Query the BookBorrowingRequests table using Entity Framework Core to get the request by id
var request = _context.BookBorrowingRequests
.Include(r => r.Books)
.FirstOrDefault(r => r.RequestId == id);

// Check if the request exists
if (request != null)
{
// Get the current user's id from the token claims
var userId = User.FindFirstValue(ClaimTypes.Name);

// Set the status and librarian id properties of the request
request.Status = "Approved";
request.LibrarianId = userId;

// Update the request in the BookBorrowingRequests table using Entity Framework Core
_context.BookBorrowingRequests.Update(request);
_context.SaveChanges();

// Return a success message to the user
return Json(new { message = "The request has been approved successfully." });
}
else
{
// Display an error message if the request does not exist
ModelState.AddModelError("", "The request does not exist.");
}

// Return the ViewRequests view with the model state errors
return View("ViewRequests", ModelState);
}
[HttpPost]
[Authorize(Roles = "SuperUser")]
public IActionResult RejectRequest(int id)
{
// Query the BookBorrowingRequests table using Entity Framework Core to get the request by id
var request = _context.BookBorrowingRequests
.Include(r => r.Books)
.FirstOrDefault(r => r.RequestId == id);

// Check if the request exists
if (request != null)
{
// Get the current user's id from the token claims
var userId = User.FindFirstValue(ClaimTypes.Name);

// Set the status and librarian id properties of the request
request.Status = "Rejected";
request.LibrarianId = userId;

// Update the request in the BookBorrowingRequests table using Entity Framework Core
_context.BookBorrowingRequests.Update(request);
_context.SaveChanges();

// Return a success message to the user
return Json(new { message = "The request has been rejected successfully." });
}
else
{
// Display an error message if the request does not exist
ModelState.AddModelError("", "The request does not exist.");
}

// Return the ViewRequests view with the model state errors
return View("ViewRequests", ModelState);
}*/