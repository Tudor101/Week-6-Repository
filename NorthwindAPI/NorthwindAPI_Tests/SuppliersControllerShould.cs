using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NorthwindAPI.Controllers;
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
    internal class SuppliersControllerShould
    {
        private SuppliersController? _sut;

        [Test]
        public void BeAbleToBeConstructed()
        {
            var mockService = new Mock<ISupplierService>();
            _sut = new SuppliersController(mockService.Object);
            Assert.That(_sut, Is.InstanceOf<SuppliersController>());
        }

        [Test]
        public void GetSuppliers_ReturnsExpected()
        {
            var mockService = new Mock<ISupplierService>();
            IEnumerable<SupplierDTO> expected = new List<SupplierDTO>()
            {
                new SupplierDTO() { CompanyName="TESTC"}
            };

            mockService.Setup(ms => ms.GetSuppliersAsync()).Returns(Task.FromResult(expected));

            _sut = new SuppliersController(mockService.Object);
            var result = _sut.GetSuppliers().Result.Value;

            Assert.That(_sut, Is.InstanceOf<SuppliersController>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().CompanyName, Is.EqualTo("TESTC"));

            mockService.Verify(cs => cs.GetSuppliersAsync(), Times.Once);
        }

        [Test]
        public void WhenGiven_ValidId_GetSupplier_ReturnsExpected()
        {
            var mockService = new Mock<ISupplierService>();
            var expected = new Supplier()
            {
                CompanyName = "TESTC"
            };

            mockService.Setup(ms => ms.GetSupplierByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(expected));

            _sut = new SuppliersController(mockService.Object);
            var result = _sut.GetSupplier(It.IsAny<int>()).Result.Value;

            Assert.That(_sut, Is.InstanceOf<SuppliersController>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CompanyName, Is.EqualTo("TESTC"));

            mockService.Verify(cs => cs.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void WhenGiven_InvalidId_GetSupplier_ReturnsNull()
        {
            var mockService = new Mock<ISupplierService>();

            mockService.Setup(ms => ms.GetSupplierByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((Supplier)null));

            _sut = new SuppliersController(mockService.Object);
            var result = _sut.GetSupplier(It.IsAny<int>()).Result.Value;

            Assert.That(_sut, Is.InstanceOf<SuppliersController>());
            Assert.That(result, Is.Null);

            mockService.Verify(cs => cs.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void WhenGiven_GetSupplierWithProducts_ReturnsNull()
        {
            var mockService = new Mock<ISupplierService>();
            IEnumerable<ProductDTO> expected = new List<ProductDTO>() {
                new ()
                {
                    ProductId = 9999,
                    ProductName="TESTPROD",
                    SupplierId=1
                }
            };
            mockService.Setup(ms => ms.SupplierExists(It.IsAny<int>()))
                            .Returns(true);
            mockService.Setup(ms => ms.GetSupplierProductsDtoAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(expected));
            _sut = new SuppliersController(mockService.Object);

            var result = _sut.GetSupplierWithProducts(It.IsAny<int>()).Result.Value;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Count(), Is.EqualTo(1));
            Assert.That(result!.First().ProductName, Is.EqualTo("TESTPROD"));

            mockService.Verify(cs => cs.SupplierExists(It.IsAny<int>()), Times.Once);
            mockService.Verify(cs => cs.GetSupplierProductsDtoAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void WhenGiven_InvalidId_GetSupplierWithProducts_ReturnsNull()
        {
            var mockService = new Mock<ISupplierService>();
            mockService.Setup(ms => ms.SupplierExists(It.IsAny<int>()))
                            .Returns(false);

            _sut = new SuppliersController(mockService.Object);
            var result = _sut.GetSupplierWithProducts(It.IsAny<int>()).Result.Value;

            Assert.That(result, Is.Null);
            mockService.Verify(cs => cs.SupplierExists(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void WhenGiven_InvalidInput_PutSupplier_Returns_400()
        {
            var mockService = new Mock<ISupplierService>();

            _sut = new SuppliersController(mockService.Object);
            StatusCodeResult result = (StatusCodeResult)_sut.PutSupplier(It.IsAny<int>(), new SupplierDTO()
            {
                SupplierId = 1234
            }).Result;

            Assert.That(result, Is.InstanceOf<BadRequestResult>());
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void WhenGiven_NonExistingId_PutSupplier_Returns_404()
        {
            var mockService = new Mock<ISupplierService>();
            mockService.Setup(ms => ms.SupplierExists(It.IsAny<int>()))
                            .Returns(false);

            _sut = new SuppliersController(mockService.Object);
            StatusCodeResult result = (StatusCodeResult)_sut.PutSupplier(It.IsAny<int>(), new SupplierDTO()
            {
                SupplierId = It.IsAny<int>()
            }).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            mockService.Verify(cs => cs.SupplierExists(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void WhenGiven_ExistingId_PutSupplier_Returns_404()
        {
            var mockService = new Mock<ISupplierService>();
            
            var supplier = new Supplier()
            {
                SupplierId = It.IsAny<int>(),
            };
            var supplierDTO = new SupplierDTO()
            {
                SupplierId = It.IsAny<int>(),
            };

            mockService.Setup(ms => ms.SupplierExists(It.IsAny<int>()))
                            .Returns(true);
            mockService.Setup(ms => ms.ModifyState(It.IsAny<Supplier>()));
            mockService.Setup(ms => ms.SaveChangesAsync());
            mockService.Setup(ms => ms.GetSupplierByIdAsync(It.IsAny<int>()))
                            .Returns(Task.FromResult(supplier));
            
            _sut = new SuppliersController(mockService.Object);
            StatusCodeResult result = (StatusCodeResult)_sut.PutSupplier(It.IsAny<int>(), supplierDTO).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(204));

            mockService.Verify(cs => cs.SupplierExists(It.IsAny<int>()), Times.Once);
            mockService.Verify(cs => cs.ModifyState(It.IsAny<Supplier>()), Times.Once);
            mockService.Verify(cs => cs.SaveChangesAsync(), Times.Once);
            mockService.Verify(cs => cs.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void WhenGiven_ExistingId_PutSupplier_Throws_AggregateException()
        {
            var mockService = new Mock<ISupplierService>();

            var supplier = new Supplier()
            {
                SupplierId = It.IsAny<int>(),
            };
            var supplierDTO = new SupplierDTO()
            {
                SupplierId = It.IsAny<int>(),
            };

            mockService.Setup(ms => ms.SupplierExists(It.IsAny<int>()))
                            .Returns(true);
            mockService.Setup(ms => ms.ModifyState(It.IsAny<Supplier>()));
            mockService.Setup(ms => ms.SaveChangesAsync()).Throws<DbUpdateConcurrencyException>();
            mockService.Setup(ms => ms.GetSupplierByIdAsync(It.IsAny<int>()))
                            .Returns(Task.FromResult(supplier));

            _sut = new SuppliersController(mockService.Object);
            StatusCodeResult result = new(0);
            
            Assert.Throws<AggregateException>(() => {
                result = (StatusCodeResult)_sut.PutSupplier(It.IsAny<int>(), supplierDTO).Result;
            });

            mockService.Verify(cs => cs.SupplierExists(It.IsAny<int>()), Times.Exactly(2));
            mockService.Verify(cs => cs.ModifyState(It.IsAny<Supplier>()), Times.Once);
            mockService.Verify(cs => cs.SaveChangesAsync(), Times.Once);
            mockService.Verify(cs => cs.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void WhenGiven_ExistingId_PutSupplier_Branch_SupplierExists_Changes()
        {
            var mockService = new Mock<ISupplierService>();

            var supplier = new Supplier()
            {
                SupplierId = It.IsAny<int>(),
            };
            var supplierDTO = new SupplierDTO()
            {
                SupplierId = It.IsAny<int>(),
            };

            mockService.SetupSequence(ms => ms.SupplierExists(It.IsAny<int>()))
                .Returns(true)
                .Returns(false);

            mockService.Setup(ms => ms.ModifyState(It.IsAny<Supplier>()));
            mockService.Setup(ms => ms.SaveChangesAsync()).Throws<DbUpdateConcurrencyException>();
            mockService.Setup(ms => ms.GetSupplierByIdAsync(It.IsAny<int>()))
                            .Returns(Task.FromResult(supplier));

            _sut = new SuppliersController(mockService.Object);
            StatusCodeResult result = (StatusCodeResult)_sut.PutSupplier(It.IsAny<int>(), supplierDTO).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));

            mockService.Verify(cs => cs.SupplierExists(It.IsAny<int>()), Times.Exactly(2));
            mockService.Verify(cs => cs.ModifyState(It.IsAny<Supplier>()), Times.Once);
            mockService.Verify(cs => cs.SaveChangesAsync(), Times.Once);
            mockService.Verify(cs => cs.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        }

    }
}

