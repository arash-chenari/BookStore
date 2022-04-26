using BookStore.Entities;
using BookStore.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Categories.Contracts
{
    public interface CategoryRepository : Repository
    {
        void Add(Category category);
        IList<GetCategoryDto> GetAll();
        Category FindById(int id);
        bool IsCategoryTitleExist(string title);
        void Delete(Category category);
    }
}
