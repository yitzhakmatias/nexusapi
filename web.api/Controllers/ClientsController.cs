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
        public async Task<ActionResult<ClientDto>> GetClient([FromQuery] int clientId)
        {
            var client = await _context.Clients.FindAsync(clientId);


            if (client == null)
            {
                return NotFound();
            }

            return null;
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }
        [HttpPost("bills")]
        public async Task<ActionResult<BillDto>> PostBill(BillDto client)
        {
     
            var bill = new Bill()
            {
                Category = client.category,
                Period = DateTime.ParseExact(Convert.ToString(client.period, CultureInfo.InvariantCulture), "yyyyMM",CultureInfo.InvariantCulture)

            };
            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostBill", new { id = bill.Id }, bill);
        }
        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return client;
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
