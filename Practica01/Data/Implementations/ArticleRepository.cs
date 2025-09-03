using Practica01.Data.Helpers;
using Practica01.Data.Interfaces;
using Practica01.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Parameter = Practica01.Data.Helpers.Parameter;

namespace Practica01.Data.Implementations
{
    public class ArticleRepository : IArticleRepository
    {
        public bool Delete(int id)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter()
                {
                    Name = "@id",
                    Value = id
                }
            };

            return DataHelper.GetInstance().ExecuteSpDML("SP_DEACTIVATE_ARTICLE", parameters);
        }

        public List<Article> GetAll()
        {
            List<Article> list = new List<Article>();

            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_ARTICLES");

            foreach (DataRow row in dt.Rows)
            {
                Article a = new Article();

                a.Id = (int)row["ID"];
                a.Name = (string)row["Name"];
                a.UnitPrice = (decimal)row["unitprice"];
                a.IsActive = (bool)row["IsActive"];

                list.Add(a);
            }

            return list;
        }

        public Article? GetById(int id)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter()
                {
                    Name = "@Id",
                    Value = id
                }
            };

            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_ARTICLE_BY_ID", parameters);

            if(dt != null && dt.Rows.Count > 0)
            {
                Article a = new Article()
                {
                    Id = (int)dt.Rows[0]["ID"],
                    Name = (string)dt.Rows[0]["Name"],
                    UnitPrice = (decimal)dt.Rows[0]["UnitPrice"],
                    IsActive = (bool)dt.Rows[0]["IsActive"]
                };

                return a;
            }

            return null;
        }

        public bool Save(Article article)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("@Id", article.Id),
                new Parameter("@Name", article.Name),
                new Parameter("@UnitPrice", article.UnitPrice)
            };
            return DataHelper.GetInstance().ExecuteSpDML("SP_SAVE_ARTICLE", parameters);
        }
    }
}
