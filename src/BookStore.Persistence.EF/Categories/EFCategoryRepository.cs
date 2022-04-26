using BookStore.Entities;
using BookStore.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Persistence.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private readonly EFDataContext _dataContext;
        public EFCategoryRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Category category)
        {
            _dataContext.Categories.Add(category);
        }

        public void Delete(Category category)
        {
            _dataContext.Categories.Remove(category);
        }

        public Category FindById(int id)
        {
            return _dataContext.Categories.Find(id);
        }

        public IList<GetCategoryDto> GetAll()
        {
            return _dataContext.Categories
                .Select(_ => new GetCategoryDto
                {
                    Id = _.Id,
                    Title = _.Title
                }).ToList();
        }

        public bool IsCategoryTitleExist(string title)
        {
            return _dataContext.Categories
                 .Any(_ => _.Title == title);

        }
    }
}
