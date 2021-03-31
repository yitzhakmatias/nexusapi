using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.api.context;
using web.api.Model;

namespace web.api.Controllers
{
    [Route("billing")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly NexusContext _context;

        public ClientsController(NexusContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }


        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<BillDto>>> GetClient([FromQuery] int clientId)
        {
            var bills =  _context.Bills.Where(p => p.State.Contains( "Pending") && p.ClientId == clientId);



            var billDtoList = new List<BillDto>();
            foreach (var bill in bills)
            {
                billDtoList.Add(new BillDto()
                {
                    category = bill.Category,
                    period = decimal.Parse(bill.Period.ToString("yyyyMM"), CultureInfo.InvariantCulture),
                    clientId = bill.ClientId
                });
            }


            return Ok(billDtoList);
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BillDto>>> SearchBill([FromQuery] string category)
        {
            var billing =  _context.Bills.Where(p => p.Category == category);

            var billDtoList = new List<BillDto>();
            foreach (var bill in billing)
            {
                billDtoList.Add(new BillDto()
                {
                    category = bill.Category,
                    period = decimal.Parse(bill.Period.ToString("yyyyMM"), CultureInfo.InvariantCulture) ,
                    clientId = bill.ClientId
                });
            }


            return Ok(billDtoList);
        }
        // PUT: api/Clients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("pay")]
        public async Task<IActionResult> PutClient(ClientDto clientDto)
        {
            var client = await _context.Clients.FindAsync(clientDto.ClientId);
            if (!ClientExists(clientDto.ClientId))
            {
                return NotFound();
            }

            var bills = _context.Bills.Where(p =>
                p.Client.Id == clientDto.ClientId && p.Category == clientDto.Category);

            foreach (var bill in bills)
            {
                bill.Client = client;
                bill.Category = clientDto.Category;
                bill.Period = DateTime.ParseExact(Convert.ToString(clientDto.Period, CultureInfo.InvariantCulture),
                    "yyyyMM", CultureInfo.InvariantCulture);
                bill.State = "Paid";
                _context.Bills.Update(bill);
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(clientDto.ClientId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return Ok();
        }


        [HttpPost("bills")]
        public async Task<ActionResult> PostBill(BillDto billDto)
        {
            var client = await _context.Clients.FindAsync(billDto.clientId);
            var bill = new Bill()
            {
                Client = client,
                Category = billDto.category,
                Period = DateTime.ParseExact(Convert.ToString(billDto.period, CultureInfo.InvariantCulture), "yyyyMM",
                    CultureInfo.InvariantCulture),
                State = "Pending",
                Amount = 100
            };
            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();

            return Ok();
        }


        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}