using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;
using Rocket.API.Entities;
using System.Threading.Tasks;

namespace NorthwindAPI.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDTO>> GetSuppliersAsync();
        Task<IEnumerable<ProductDTO>> GetSupplierProductsDtoAsync(int id);
        Task<SupplierDTO> GetSupplierDtoAsync(int id);
        Task<IEnumerable<Product>> GetSupplierProductsAsync(int id);
        void RemoveProducts(IEnumerable<Product> products);
        bool SupplierExists(int id);
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task<Supplier> FindSupplierAsync(int id);
        Task<Supplier> AddSupplierAsync(SupplierDTO entity);
        void RemoveSupplierAsync(Supplier supplier);
        Task AddProductsAsync(IEnumerable<Product> entities);
        Task<int> SaveChangesAsync();
        void ModifyState(Supplier entity);
    }
}
