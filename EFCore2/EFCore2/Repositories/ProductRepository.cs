using EFCore2.Interfaces;
using EFCore2.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore2.Repositories;

public class ProductRepository: IProductStore<Product>
{
    //repo return Inumberable
    //services only query
    private readonly StoreContext _context;

    public ProductRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return (await _context.Products.FindAsync(id))!;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Check if the CategoryId exists
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                // If the CategoryId does not exist, rollback the transaction and throw an exception
                throw new Exception($"The CategoryId {product.CategoryId} does not exist.");
            }

            // If the CategoryId exists, create the new product
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            //commit vs commitAsync
            await transaction.CommitAsync();
            return product;
        }
        catch (Exception)
        {
            // If an error occurs, rollback the transaction and rethrow the exception
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task UpdateAsync(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}