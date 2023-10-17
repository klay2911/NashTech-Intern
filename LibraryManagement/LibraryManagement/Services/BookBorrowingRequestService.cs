using System.Drawing;
using System.Drawing.Imaging;
using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using Newtonsoft.Json;
using Spire.Pdf;
using X.PagedList;

namespace LibraryManagement.Services;

public class BookBorrowingRequestService : IBookBorrowingRequestService
{
    private readonly UnitOfWork _unitOfWork;

    public BookBorrowingRequestService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IPagedList<BookBorrowingRequest>> GetAllBorrowingRequests(int pageNumber, int pageSize, string searchTerm = "")
    {
        var requests = await _unitOfWork.BookBorrowingRequestRepository.GetAll();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            requests = requests.Where(r => r.BookBorrowingRequestDetails.Any(d => d.Book.Title.Contains(searchTerm)));
        }

        int totalCount = requests.Count();

        var pagedRequests = requests.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new StaticPagedList<BookBorrowingRequest>(pagedRequests, pageNumber, pageSize, totalCount);
    }


    public async Task<BookBorrowingRequest> GetRequest(int requestId)
    {
        return await _unitOfWork.BookBorrowingRequestRepository.GetByIdAsync(requestId);
    }
    public async Task<IPagedList<BookViewModel>> GetUserBorrowedBooks(int userId, int pageNumber, int pageSize, string searchTerm)
    {
        var borrowingRequests = await _unitOfWork.BookBorrowingRequestRepository.GetRequestsByUser(userId);

        var borrowedBooks = new List<BookViewModel>();

        foreach (var request in borrowingRequests)
        {
            foreach (var detail in request.BookBorrowingRequestDetails)
            {
                var book = await _unitOfWork.BookRepository.GetByIdAsync(detail.BookId);
                var bookViewModel = new BookViewModel
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    ISBN = book.ISBN,
                    CategoryId = book.CategoryId,
                    Status = request.Status,
                    ExpiryDate = request.ExpiryDate,
                    PdfFilePath = book.PdfFilePath
                };
                if (request.Status == "Approved")
                {
                    request.ExpiryDate = DateTime.Now.AddDays(10);
                    _unitOfWork.BookBorrowingRequestRepository.Update(request);
                    await _unitOfWork.SaveAsync();
                }
                borrowedBooks.Add(bookViewModel);
            }
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            borrowedBooks = borrowedBooks.Where(b => b.Title.Contains(searchTerm)).ToList();
        }

        int totalCount = borrowedBooks.Count();

        var pagedBooks = borrowedBooks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new StaticPagedList<BookViewModel>(pagedBooks, pageNumber, pageSize, totalCount);
    }
// if (request.Status == "Approved" && book.PdfFilePath != null)
    // {
    //     var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.PdfFilePath.TrimStart('/'));
    //     await using var pdfStream = System.IO.File.OpenRead(pdfPath);
    //     var pdf = new PdfDocument();
    //     pdf.LoadFromStream(pdfStream);
    //
    //     var totalHeight = 0;
    //     for (var i = 0; i < pdf.Pages.Count; i++)
    //     {
    //         totalHeight += Convert.ToInt32(pdf.Pages[i].Size.Height);
    //     }
    //     var combinedImage = new Bitmap(1100, totalHeight);
    //
    //     using (var g = Graphics.FromImage(combinedImage))
    //     {
    //         int currentHeight = 0;
    //         for (int i = 0; i < pdf.Pages.Count; i++)
    //         {
    //             var pageImage = pdf.SaveAsImage(i);
    //             g.DrawImage(pageImage, 0, currentHeight);
    //             currentHeight += pageImage.Height;
    //         }
    //     }
    //
    //     using var msImage = new MemoryStream();
    //     combinedImage.Save(msImage, ImageFormat.Png);
    //     var base64Image = Convert.ToBase64String(msImage.ToArray());
    //
    //     bookViewModel.ImagesBase64 = base64Image;
    //
    //     bookViewModel.ExpiryDate = DateTime.Now.AddDays(10); 
    // }

    public async Task CreateBorrowingRequestAsync(BookBorrowingRequest borrowingRequest, string bookIdsInRequestJson)
    {
        if (string.IsNullOrEmpty(bookIdsInRequestJson))
        {
            throw new Exception("You have not added any book to the request.");
        }

        var bookIdsInRequest = JsonConvert.DeserializeObject<List<int>>(bookIdsInRequestJson);
        if (bookIdsInRequest == null || !bookIdsInRequest.Any())
        {
            throw new Exception("You have not added any book to the request.");
        }

        await _unitOfWork.BookBorrowingRequestRepository.CreateAsync(borrowingRequest);
    }
    public async Task<bool> CheckUserRequestLimit(int userId)
    {
        var userRequestsThisMonth = await _unitOfWork.BookBorrowingRequestRepository.GetRequestsByUserThisMonth(userId);
        return userRequestsThisMonth >= 3;
    }
    public async Task UpdateRequestStatus(int requestId, string status, int librarianId)
    {
        await _unitOfWork.BookBorrowingRequestRepository.UpdateRequestStatus(requestId, status, librarianId);
    }
}
