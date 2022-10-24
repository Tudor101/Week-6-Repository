using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Controllers;
using NorthwindAPI.Models;
using NorthwindAPI.Services;

namespace NorthwindAPI.Services
{
    public class SuppliersService : ISupplierService

    {
        private readonly NorthwindContext _context;



        public SuppliersService(NorthwindContext context)
        {
            _context = context;
        }



        public async Task CreateSupplierAsync(Supplier s)
        {
            _context.Suppliers.Add(s);
            await _context.SaveChangesAsync();
        }

      

        public List<Supplier> GetSupplier(int id)
        {
            return _context.Suppliers.Where(x => x.SupplierId == id).ToList();
        }



        public List<Product> GetSupplierWithProducts(int id)
        {
           return _context.Products.Where(x => x.SupplierId == id).ToList();
        }



        public async Task RemoveSupplierAsync(Supplier item)
        {
            _context.Suppliers.Remove(item);
            await _context.SaveChangesAsync();
        }

        

        public async Task SaveSupplierChangesAsync()
        {
            await _context.SaveChangesAsync();
        }



        public bool SupplierExists(long id)
        {
            return _context.Suppliers.Any(e => e.SupplierId == id);
        }

        //Task<Supplier> ISupplierService.GetSupplierByIdAsync(long id)
        //{
        //    throw new NotImplementedException();
        //}

        //List<Supplier> ISupplierService.GetSupplier()
        //{
        //    throw new NotImplementedException();
        //}
    }
}