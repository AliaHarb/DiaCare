using AutoMapper;
using DiaCare.Application.DTOS;
using DiaCare.Application.Interfaces;
using DiaCare.Domain.Entities;
using DiaCare.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Application.Services
{
    public class ArticleService : BaseService, IArticleService
    {
        private readonly IBaseRepository<Article> _baseRepository;

        public ArticleService(IBaseRepository<Article> baseRepository, IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _baseRepository = baseRepository;
        }

        // Refactoring 3: Inline Temp & Replace Temp with Query 
        // Code Smell: Unnecessary Temporary Variable
        //--------------------------------------------------------

        /*  public async Task<IEnumerable<ArticleDto>> GetArticlesAsync()
          {
              // [BEFORE]: Using a temporary variable 'articles' just to return it
              var articles = await _baseRepository.GetAllAsync();
              return _mapper.Map<IEnumerable<ArticleDto>>(articles);
          }
         */

        // After Refactoring
        // Improvement: Removed temp variable to make code more concise
        public async Task<IEnumerable<ArticleDto>> GetArticlesAsync()
        {
            // [AFTER]: Returning the result directly (Inlining the expression)
            return _mapper.Map<IEnumerable<ArticleDto>>(await _baseRepository.GetAllAsync());
        }

        // Refactoring 4: Rename Method 
        // Code Smell: Uncommunicative Name (Vague method name)
        //// It's not clear "What" we are getting by ID from the name
        //[BEFORE] :: public async Task<ArticleDto> GetByIdAsync(int id)

        // After Refactoring
        // Improvement: Meaningful and self-documenting name
        public async Task<ArticleDto> GetArticleByIdAsync(int id) // Now the name clearly describes the returned object
        {
            return _mapper.Map<ArticleDto>(await _baseRepository.GetByIdAsync(id));
        }

        
        public async Task<ArticleDto> AddArticleAsync(ArticleDto dto)
        {
            var article = _mapper.Map<Article>(dto);
            await _baseRepository.AddAsync(article);

            // After Refactoring::
            //Using a private helper to consolidate the fragment
            await CommitChangesAsync();
            return _mapper.Map<ArticleDto>(article);
        }

       

        //Refactoring 5:: Replace Nested Conditional with Guard Clauses
        // Code Smell: Nested Conditional (Complex structure)
        //--------------------------------------------------------

        /*
        public async Task<ArticleDto> UpdateArticleAsync(int id, ArticleDto dto)
        {
            var existingArticle = await _baseRepository.GetByIdAsync(id);

            // [BEFORE]: The main logic is nested inside the if-block
            if (existingArticle != null)
            {
                _mapper.Map(dto, existingArticle);
                _baseRepository.Update(existingArticle);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<ArticleDto>(existingArticle);
            }
            else
            {
                // Negative case is hidden down here
                return null;
            }
        }
        */


        // After Refactoring
        // Improvement: Simplified logic and removed 'else' (Flat Structure)
        public async Task<ArticleDto> UpdateArticleAsync(int id, ArticleDto dto)
        {
            var existingArticle = await _baseRepository.GetByIdAsync(id);

            // [AFTER]: Guard Clause - Handling the edge case early
            if (existingArticle == null) return null;

            // The "Happy Path" (Main Logic) is now clean and not nested
            _mapper.Map(dto, existingArticle);
            _baseRepository.Update(existingArticle);

            // Consolidate Duplicate Fragments could also be applied here
            await CommitChangesAsync();

            return _mapper.Map<ArticleDto>(existingArticle);
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            var article = await _baseRepository.GetByIdAsync(id);
            if (article == null) return false;

            _baseRepository.Delete(article);
            await CommitChangesAsync();
            return true;
        }

        // Private helper method
        private async Task CommitChangesAsync() => await _unitOfWork.SaveChangesAsync();


    }

}