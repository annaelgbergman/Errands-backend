using issue.Contexts;
using issue.Models;
using issue.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace issue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SqlDataContext _context;

        public CustomersController(SqlDataContext context)
        {
            _context = context;
        }

        [HttpPost]

        public async Task<IActionResult> Create(CustomerRequest req)
        {
            try
            {
                if (!await _context.Customers.AnyAsync(x => x.Email == req.Email))
                {
                    var customerEntity = new CustomerEntity { FirstName = req.FirstName, LastName = req.LastName, Email = req.Email, PhoneNumber = req.PhoneNumber };
                    _context.Add(customerEntity);
                    await _context.SaveChangesAsync();

                    return new OkObjectResult(customerEntity);
                }
            }
            catch(Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = new List<CustomerModel>();
                foreach (var customer in await _context.Customers.ToListAsync())
                    customers.Add(new CustomerModel { Id = customer.Id, FirstName = customer.FirstName, LastName = customer.LastName, Email = customer.Email, PhoneNumber = customer.PhoneNumber });

                return new OkObjectResult(customers);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var customerEntity = await _context.Customers.FindAsync(id);
                if (customerEntity != null)
                    return new OkObjectResult(new CustomerModel
                    {
                        Id = customerEntity.Id,
                        FirstName = customerEntity.FirstName,
                        LastName = customerEntity.LastName,
                        Email = customerEntity.Email,
                        PhoneNumber = customerEntity.PhoneNumber
                    });
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

    }
}
