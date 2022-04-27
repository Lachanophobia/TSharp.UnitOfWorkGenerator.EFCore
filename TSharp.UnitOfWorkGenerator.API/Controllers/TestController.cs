using Microsoft.AspNetCore.Mvc;
using TSharp.UnitOfWorkGenerator.API.Entyties;
using GeneratedUnitOfWork;
using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TSharp.UnitOfWorkGenerator.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IPostRepository _postRepo;
        private readonly IUnitOfWork _unitOfWork;

        public TestController(IPostRepository postRepo, IUnitOfWork unitOfWork)
        {
            _postRepo = postRepo;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.Post.GetAllAsync());
        }



        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _unitOfWork.Blog.GetAllAsync(includeProperties: "Posts");
            return Ok(blogs);
        }
    }
}
