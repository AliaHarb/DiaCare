using DiaCare.Application.DTOS;
using DiaCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Application.Interfaces
{
    public interface IArticleService
    {
        public Task<IEnumerable<ArticleDto>> GetArticlesAsync();
        public Task<ArticleDto> GetArticleByIdAsync(int id);
        public Task<ArticleDto> AddArticleAsync(ArticleDto dto);

        public  Task<ArticleDto> UpdateArticleAsync(int id, ArticleDto dto);

        public Task<bool> DeleteArticleAsync(int id);

    }
}
