using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailsController : ControllerBase
    {
        private readonly PaymentDetailContext _context;
        public PaymentDetailsController(PaymentDetailContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<PaymentDetail>> GetPaymentDetails()
        {
            return await this._context.PaymentDetails.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetail>> GetPaymentDetail(int id)
        {
            var paymentDetail = await this._context.PaymentDetails.FindAsync(id);
            return paymentDetail != null ? paymentDetail : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDetail>> PostPaymentDetail(PaymentDetail paymentDetail)
        {
            this._context.PaymentDetails.Add(paymentDetail);
            await this._context.SaveChangesAsync();
            return CreatedAtAction("GetPaymentDetails", new { id = paymentDetail.PaymentDetailId, paymentDetail });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentDetail>> PutPaymentDetail(int id, PaymentDetail paymentDetail)
        {
            if (id != paymentDetail.PaymentDetailId)
                return BadRequest();

            this._context.Entry(paymentDetail).State = EntityState.Modified;

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentDetailExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentDetail>> DeletePaymentDetail(int id)
        {
            var paymentDetail = await this._context.PaymentDetails.FindAsync(id);
            if (paymentDetail == null)
                return NotFound();

            this._context.PaymentDetails.Remove(paymentDetail);
            await this._context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentDetailExists(int id)
        {
            return this._context.PaymentDetails.Any(detail => detail.PaymentDetailId == id);
        }
    }
}
