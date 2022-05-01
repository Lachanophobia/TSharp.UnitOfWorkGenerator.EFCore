using Microsoft.AspNetCore.Mvc;
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
        [Route("GetPosts")]
        public async Task<IActionResult> GetPosts()
        {
            return Ok(await _unitOfWork.Post.GetFirstOrDefaultAsync(x => x.BlogId!= 1));
        }

        [HttpGet]
        [Route("GetPostsFromPartialClass")]
        public async Task<IActionResult> GetPostsFromPartialClass()
        {
            return Ok(await _unitOfWork.Post.GetPostsFromPartialClass());
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _unitOfWork.Blog.GetAllAsync(includeProperties: x => x.Posts);
            return Ok(blogs);
        }
    }
}
