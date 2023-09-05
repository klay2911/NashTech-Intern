using EFCore2.Interfaces;
using EFCore2.Models;

namespace EFCore2.Services;

public class ProductService : IProductService
{
    private readonly UnitOfWork _unitOfWork;

    public ProductService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _unitOfWork.ProductRepository.GetAllAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _unitOfWork.ProductRepository.GetByIdAsync(id);
    }

    public async Task CreateProductAsync(Product product)
    {
        /*await _unitOfWork.CreateTransaction();
        try
        {*/
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(product.CategoryId);
            if (category == null)
            {
                throw new Exception($"The CategoryId {product.CategoryId} does not exist.");
            }
            await _unitOfWork.ProductRepository.CreateAsync(product);

           /* await _unitOfWork.CommitAsync();
            
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackAsync();
            throw;
        }*/
    }
    public async Task UpdateProductAsync(Product product)
    {
        var existingProduct = await _unitOfWork.ProductRepository.GetByIdAsync(product.ProductId);
            if (existingProduct == null)
            {
                throw new Exception($"The product with Id {product.ProductId} does not exist.");
            }
            
            existingProduct.ProductName = product.ProductName;
            existingProduct.Manufacture = product.Manufacture;
            existingProduct.CategoryId = product.CategoryId;

            //current unit of work to db
            await _unitOfWork.SaveAsync();
    }
    
    public async Task DeleteProductAsync(int id)
    {
        await _unitOfWork.ProductRepository.DeleteAsync(id);
    }
}