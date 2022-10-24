using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;
using NorthwindAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NorthwindAPI_Tests
{
    public class ServiceTests
    {
        private NorthwindContext _context;
        private ISupplierService _sut;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: "NorthwindDB").Options;
            _context = new NorthwindContext(options);
            _sut = new SupplierService(_context);
            _sut.AddSupplierAsync(new SupplierDTO
            {
                SupplierId = 1,
                CompanyName = "Sparta Global",
                Country = "UK",
                ContactName = "Nish Mandal",
                ContactTitle = "Manager",
            }).Wait();

            _sut.AddSupplierAsync(new SupplierDTO
            {
                SupplierId = 2,
                CompanyName = "Nintendo",
                Country = "Japan",
                ContactName = "Shigeru Miyamoto",
                ContactTitle = "CEO"
            }).Wait();
        }

        [Test]
        public void Default_Constuctor()
        {
            var result = new SupplierService();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<SupplierService>());
            Assert.That(result, Is.InstanceOf<SupplierService>());
        }

        [Test]
        public void GivenValidID_GetSupplierById_ReturnsCorrectSupplier()
        {
            var result = _sut.GetSupplierByIdAsync(1).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Supplier>());
            Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
        }

        [Test]
        public void GivenValidID_GetSuppliersAsync_ReturnsCorrectSuppliers()
        {
            var result = _sut.GetSuppliersAsync().Result.ToList();
            Assert.That(result, Is.Not.Null);

            Assert.That(result[0], Is.Not.Null);
            Assert.That(result[0], Is.TypeOf<SupplierDTO>());
            Assert.That(result[0].CompanyName, Is.EqualTo("Sparta Global"));

            Assert.That(result[1], Is.Not.Null);
            Assert.That(result[1], Is.TypeOf<SupplierDTO>());
            Assert.That(result[1].CompanyName, Is.EqualTo("Nintendo"));
        }

        [Test]
        public void ModifyState_DoesExpected()
        {
            var supplier = _sut.GetSupplierByIdAsync(1).Result;
            _context.Entry(supplier).State = EntityState.Detached;
            _sut.ModifyState(supplier);
            var result = _context.Entry(supplier).State;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<EntityState>());
            Assert.That(result, Is.EqualTo(EntityState.Modified));
        }

        [Test]
        public void GivenValid_AddSupplierAsync_InsertsCorrect()
        {
            _sut.AddSupplierAsync(new SupplierDTO { SupplierId = 3, CompanyName = "TEST", Country = "TEST", ContactName = "TEST", ContactTitle = "TEST" }).Wait();

            var result = _sut.GetSupplierByIdAsync(3).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Supplier>());
            Assert.That(result.CompanyName, Is.EqualTo("TEST"));

            _sut.RemoveSupplierAsync(result);
        }

        [Test]
        public void GivenValid_AddProductsAsync_InsertsExpected()
        {
            var sup = _sut.GetSupplierByIdAsync(1).Result;

            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    ProductId = 9999,
                    ProductName="TESTPROD",
                    SupplierId=1,
                    Supplier= sup
                }
            };
            _sut.AddProductsAsync(products).Wait();
            _sut.SaveChangesAsync();

            var result = _sut.GetSupplierProductsAsync(1).Result
                .ToList();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result[0], Is.TypeOf<Product>());
            Assert.That(result[0].ProductName, Is.EqualTo("TESTPROD"));

            _sut.RemoveProducts(products);
        }

        [Test]
        public void GivenValidId_FindSupplierAsync_InsertsCorrect()
        {
            var result = _sut.FindSupplierAsync(1).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Supplier>());
            Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
        }

        [Test]
        public void GivenValidID_GetSupplierDtoAsync_ReturnsCorrectSupplier()
        {
            var result = _sut.GetSupplierDtoAsync(1).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<SupplierDTO>());
            Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
        }

        [Test]
        public void GivenValidID_GetSupplierProductsDtoAsync_ReturnsCorrectProducts()
        {
            var sup = _sut.GetSupplierByIdAsync(1).Result;
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    ProductId = 9999,
                    ProductName="TESTPROD",
                    SupplierId=1,
                    Supplier= sup
                }
            };
            _sut.AddProductsAsync(products).Wait();
            _sut.SaveChangesAsync();

            var result = _sut.GetSupplierProductsDtoAsync(1).Result.ToList();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result[0], Is.TypeOf<ProductDTO>());
            Assert.That(result[0].ProductName, Is.EqualTo("TESTPROD"));

            _sut.RemoveProducts(products);
        }

        [Test]
        public void GivenValidID_RemoveSupplierAsync_ReturnsCorrectSupplier()
        {
            _sut.AddSupplierAsync(new SupplierDTO { SupplierId = 3, CompanyName = "TEST", Country = "TEST", ContactName = "TEST", ContactTitle = "TEST" }).Wait();
            var supplier = _sut.GetSupplierByIdAsync(3).Result;
            _sut.RemoveSupplierAsync(supplier);
            _sut.SaveChangesAsync();
            var result = _sut.SupplierExists(3);

            Assert.That(result, Is.EqualTo(false));
        }
    }
}