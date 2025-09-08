using CommerceBack.Data.Implementations;
using CommerceBack.Data.Interfaces;
using CommerceBack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceBack.Services
{
    public class ArticleService
    {
       

        private IArticleRepository _articleRepository;

        public ArticleService()
        {
            _articleRepository = new ArticleRepository();
        }

        public List<Article> GetArticles()
        {
            return _articleRepository.GetAll();
        }

        public Article? GetArticle(int id)
        {
            return _articleRepository.GetById(id);
        }

        public bool SaveArticle(Article article)
        {
            return _articleRepository.Save(article);
        }

        public bool DeleteArticle(int id)
        {

            var articleInDB = _articleRepository.GetById(id);

            return articleInDB != null ? _articleRepository.Delete(id) : false;
        }
    }
}
