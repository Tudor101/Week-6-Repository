using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Controllers;
using NorthwindAPI.Models.DTO;
using NorthwindAPI.Models;
using NorthwindAPI.Services;

public class SupplierService : ISupplierService
{
    private readonly NorthwindContext _context;
    public SupplierService() => _context = new();
    public SupplierService(NorthwindContext context) => _context = context;

    public async Task<Supplier?> AddSupplierAsync(SupplierDTO entity)
    {
        var products = new List<Product>();
        entity.Products.ToList().ForEach(p => products.Add(Utils.ToProduct(p)));

        var supplier = Utils.ToSupplier(entity);

        await _context.AddRangeAsync(products);
        await _context.SaveChangesAsync();
        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();

        return await _context.Suppliers
              .Where(s => s.SupplierId == supplier.SupplierId)
              .Include(x => x.Products)
              .FirstOrDefaultAsync();
    }

    public async Task AddProductsAsync(IEnumerable<Product> entities) => await _context.AddRangeAsync(entities);
    
    public async Task<Supplier> FindSupplierAsync(int id) => await _context.Suppliers.FindAsync(id);
    
    public async Task<Supplier?> GetSupplierByIdAsync(int id) =>
        await _context.Suppliers
                        .Include(s => s.Products)
                        .Where(x => x.SupplierId == id)
                        .FirstOrDefaultAsync();
    public async Task<SupplierDTO?> GetSupplierDtoAsync(int id) =>
        await _context.Suppliers
            .Where(s => s.SupplierId == id)
            .Include(x => x.Products)
            .Select(x => Utils.ToSupplierDTO(x))
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Product>> GetSupplierProductsAsync(int id) =>
        await _context.Products
            .Include(p => p.Supplier)
            .Where(p => p.SupplierId == id)
            .ToListAsync();

    public async Task<IEnumerable<ProductDTO>> GetSupplierProductsDtoAsync(int id) =>
        await _context.Products
            .Where(p => p.SupplierId == id)
            .Select(x => Utils.ToProductDTO(x))
                .ToListAsync();

    public async Task<IEnumerable<SupplierDTO>> GetSuppliersAsync() =>
        await _context.Suppliers
            .Include(x => x.Products)
            .Select(x => Utils.ToSupplierDTO(x))
                .ToListAsync();

    public void ModifyState(Supplier supplier) => _context.Entry(supplier).State = EntityState.Modified;

    public void RemoveProducts(IEnumerable<Product> products) => _context.Products.RemoveRange(products);

    public async void RemoveSupplierAsync(Supplier supplier)
    {
        var products = await GetSupplierProductsAsync(supplier.SupplierId);
        _context.Products.RemoveRange(products);
        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public bool SupplierExists(int id) => _context.Suppliers.Any(e => e.SupplierId == id);
}