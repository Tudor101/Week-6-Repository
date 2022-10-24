using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Controllers;
using NorthwindAPI.Models.DTO;
using NorthwindAPI.Services;
using System.Collections.Generic;

namespace NorthwindAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSuppliers()
        {
            return new(await _supplierService.GetSuppliersAsync());
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDTO>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return Utils.ToSupplierDTO(supplier);
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetSupplierWithProducts(int id)
        {
            if (!SupplierExists(id))
                return NotFound();

            return new(await _supplierService.GetSupplierProductsDtoAsync(id));
        }

        // PUT: api/Suppliers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, SupplierDTO supplierDto)
        {
            if (id != supplierDto.SupplierId)
            {
                return BadRequest();
            }

            if (!SupplierExists(id)) return NotFound();

            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            Utils.SupplierFromDTO(supplierDto, supplier);
            _supplierService.ModifyState(supplier);

            try
            {
                await _supplierService.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Suppliers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SupplierDTO>> PostSupplier(SupplierDTO supplierDto)
        {
            var supplier = await _supplierService.AddSupplierAsync(supplierDto);
            supplierDto = await _supplierService.GetSupplierDtoAsync(supplier.SupplierId);
            return CreatedAtAction(nameof(GetSupplier), new { id = supplier.SupplierId }, supplierDto);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _supplierService.FindSupplierAsync(id);
            if (supplier == null)
                return BadRequest();

            _supplierService.RemoveSupplierAsync(supplier);
            return NoContent();
        }

        private bool SupplierExists(int id)
        {
            return _supplierService.SupplierExists(id);
        }
    }

}