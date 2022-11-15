using issue.Contexts;
using issue.Models.Entities;
using issue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace issue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly SqlDataContext _context;

        public IssuesController(SqlDataContext context)
        {
            _context = context;
        }

        [HttpPost]

        public async Task<IActionResult> Create(IssueRequest req)
        {
            try
            {
                var dateTime = DateTime.Now;
                var issueEntity = new IssueEntity
                {
                    Subject = req.Subject,
                    Description = req.Description,
                    CustomerId = req.CustomerId,
                    Created = dateTime,
                    Modefied = dateTime,
                    StatusId = 1
                };

                _context.Add(issueEntity);
                await _context.SaveChangesAsync();

                return new OkObjectResult(issueEntity);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var issues = new List<IssueModel>();
                foreach (var issueEntity in await _context.Issues.Include(x => x.Status).Include(x => x.Customer).ToListAsync())
                    issues.Add(new IssueModel 
                    {
                        Id = issueEntity.Id,
                        Subject = issueEntity.Subject,
                        Description = issueEntity.Description,
                        Created = issueEntity.Created,
                        Modefied = issueEntity.Modefied,
                        Status = new StatusModel
                        {
                            Id = issueEntity.Status.Id,
                            Status = issueEntity.Status.Status
                        },
                        Customer = new CustomerModel
                        {
                            Id = issueEntity.Customer.Id,
                            FirstName = issueEntity.Customer.FirstName,
                            LastName = issueEntity.Customer.LastName,
                            Email = issueEntity.Customer.Email,
                            PhoneNumber = issueEntity.Customer.PhoneNumber
                        }

                    });

                return new OkObjectResult(issues);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var issueEntity = await _context.Issues.Include(x => x.Status).Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == id);
                if (issueEntity != null)
                    return new OkObjectResult(new IssueModel
                    {
                        Id = issueEntity.Id,
                        Subject = issueEntity.Subject,
                        Description = issueEntity.Description,
                        Created = issueEntity.Created,
                        Modefied = issueEntity.Modefied,
                        Status = new StatusModel
                        {
                            Id = issueEntity.Status.Id,
                            Status = issueEntity.Status.Status
                        },
                        Customer = new CustomerModel
                        {
                            Id = issueEntity.Customer.Id,
                            FirstName = issueEntity.Customer.FirstName,
                            LastName = issueEntity.Customer.LastName,
                            Email = issueEntity.Customer.Email,
                            PhoneNumber = issueEntity.Customer.PhoneNumber
                        }

                    });
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }
    }

}
