using Practica01.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica01.Data.Interfaces
{
    public interface IArticleRepository
    {
        List<Article> GetAll();
        Article? GetById(int id);
        bool Save(Article article);
        bool Delete(int id);
    }
}
