using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
namespace FinalProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class PaymentController: ControllerBase
    {
       
        private readonly ApiDbContext _context;

        public PaymentController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _context.Items.ToListAsync();
            return new OkObjectResult(new { GetDataSuccess = "Get Data Berhasil", Status = "Sukses", items });
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(PaymentDetails data)
        {
            if (ModelState.IsValid)
            {
                await _context.Items.AddAsync(data);
                await _context.SaveChangesAsync();

                var itemData = await _context.Items.FirstOrDefaultAsync(x => x.paymentDetailId == data.paymentDetailId);

                return new OkObjectResult(new { InsertDataSuccess = "Insert Data Berhasil", Sukses = "sukses", itemData});
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetItem(int id)
        {
            var itemData = await _context.Items.FirstOrDefaultAsync(x => x.paymentDetailId == id);

            if (itemData == null)
                return NotFound();

            return new OkObjectResult(new { GetDataSuccess = $"Get data berhasil", Status = "sukses", itemData });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, PaymentDetails item)
        {
            if (id != item.paymentDetailId)
            {
                return BadRequest();
            }

            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.paymentDetailId
            == id);

            if (existItem == null)
            {
                return NotFound();
            }

            existItem.cardOwnerName= item.cardOwnerName;
            existItem.cardNumber = item.cardNumber;
            existItem.expirationDate = item.expirationDate;
            existItem.securityCode= item.securityCode;
           
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { UpdateSuccess = $"Updated Dengan data berhasil", Status = "sukses", item });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.paymentDetailId == id);

            if (existItem == null)
            {
                return NotFound();
            }

            _context.Items.Remove(existItem);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new { DeleteSuccess = $"Delete data Berhasil", Sukses = "sukses", existItem });
        }
    }
}
