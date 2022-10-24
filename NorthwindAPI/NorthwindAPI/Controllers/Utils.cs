using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;

namespace NorthwindAPI.Controllers
{
    public class Utils
    {
        public static Supplier ToSupplier(SupplierDTO source) =>
            new Supplier
            {
                SupplierId = source.SupplierId,
                CompanyName = source.CompanyName,
                ContactName = source.ContactName,
                ContactTitle = source.ContactTitle,
                Country = source.Country,
                Products = source.Products.Select(x => ToProduct(x)).ToList()
            };

        public static Product ToProduct(ProductDTO source) =>
            new Product
            {
                ProductId = source.ProductId,
                ProductName = source.ProductName,
                SupplierId = source.SupplierId,
                CategoryId = source.CategoryId,
                UnitPrice = source.UnitPrice,
            };

        public static SupplierDTO ToSupplierDTO(Supplier source) =>
            new SupplierDTO
            {
                SupplierId = source.SupplierId,
                CompanyName = source.CompanyName,
                ContactTitle = source.ContactTitle,
                ContactName = source.ContactName,
                Country = source.Country,
                TotalProducts = source.Products.Count,
                Products = source.Products.Select(x => ToProductDTO(x)).ToList()
            };

        public static ProductDTO ToProductDTO(Product source) =>
            new ProductDTO
            {
                ProductId = source.ProductId,
                ProductName = source.ProductName,
                SupplierId = source.SupplierId,
                CategoryId = source.CategoryId,
                UnitPrice = source.UnitPrice,
            };

        public static void SupplierFromDTO(SupplierDTO supplierDto, Supplier supplier)
        {
            supplier.ContactTitle = supplierDto.ContactTitle;
            supplier.Country = supplierDto.Country;
            supplier.ContactName = supplierDto.ContactName;
            supplier.CompanyName = supplierDto.CompanyName;
            supplier.Products = ProductFromDTO(supplierDto.Products.ToList(), supplier.Products.ToList());
        }

        public static ICollection<Product> ProductFromDTO(IList<ProductDTO> productDtos, List<Product> products)
        {
            for (int i = 0; i < productDtos.Count; i++)
            {
                products[i].SupplierId = productDtos[i].SupplierId;
                products[i].ProductName = productDtos[i].ProductName;
                products[i].ProductId = productDtos[i].ProductId;
                products[i].UnitPrice = productDtos[i].UnitPrice;
                products[i].CategoryId = productDtos[i].CategoryId;
            }
            return products;
        }
    }
}
