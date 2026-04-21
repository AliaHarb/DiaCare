using DiaCare.Application.DTOS;
using DiaCare.Application.Helpers;
using DiaCare.Application.Interfaces;
using DiaCare.Application.Services;
using DiaCare.Domain.Interfaces;
using DiaCare.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiaCare.WebAPI.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService,IUnitOfWork unitOfWork)
        {
            _articleService=articleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _articleService.GetArticlesAsync();

            return Ok(Result<IEnumerable<ArticleDto>>.Success(articles, "Articles retrieved successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var article = await _articleService.GetByIdAsync(id);

            if (article == null)
            {
            
                return NotFound(Result<ArticleDto>.Failure("Article not found", 404));
            }

            return Ok(Result<ArticleDto>.Success(article));
        }

      
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ArticleDto dto)
        {
           
            var result = await _articleService.AddArticleAsync(dto);

            return Ok(Result<ArticleDto>.Success(result, "Article created successfully"));
        }

        
        [HttpDelete("{id}")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _articleService.DeleteArticleAsync(id);

            if (!success)
            {
                
                return NotFound(Result<bool>.Failure("Article not found", 404));
            }

          
            return Ok(Result<bool>.Success(true, "Article deleted successfully"));
        }

    }
}
