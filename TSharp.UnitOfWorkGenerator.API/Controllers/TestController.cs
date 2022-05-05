using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using TSharp.UnitOfWorkGenerator.API.Entities;
using TSharp.UnitOfWorkGenerator.API.Models;
using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TSharp.UnitOfWorkGenerator.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetPosts")]
        public async Task<IActionResult> GetPosts(CancellationToken cancellationToken)
        {
            return Ok(await _unitOfWork.Post.GetFirstOrDefaultAsync(x => x.BlogId!= 1, cancellationToken));
        }

        [HttpGet]
        [Route("GetPostsFromPartialClass")]
        public async Task<IActionResult> GetPostsFromPartialClass(CancellationToken cancellationToken)
        {
            return Ok(await _unitOfWork.Post.GetPostsFromPartialClass(cancellationToken));
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var blogs = await _unitOfWork.Blog.GetAllAsync(cancellationToken: cancellationToken,includeProperties: x => x.Posts);
            return Ok(blogs);
        }

        [HttpPost]
        [Route("AddNewEmployee")]
        public async Task<IActionResult> AddNewEmploy(EmployeeRequest employeeRequest, CancellationToken cancellationToken)
        {
            var employee = new Employee()
            {
                Age = employeeRequest.Age,
                Address = employeeRequest.Address,
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
            };

            await _unitOfWork.Employee.AddAsync(employee, cancellationToken);
            await _unitOfWork.SaveAsync();

            return Ok(employee);
        }
    }
}
