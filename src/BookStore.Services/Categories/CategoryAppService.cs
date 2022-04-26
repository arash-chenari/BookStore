using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Services.Categories.Contracts;
using BookStore.Services.Categories.Exceptions;
using System.Collections.Generic;

namespace BookStore.Services.Categories
{
    public class CategoryAppService : CategoryService
    {
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        
            public CategoryAppService(
                CategoryRepository repository ,
                UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddCategoryDto dto)
        {
            var category = new Category
            {
                Title = dto.Title,
            };

            bool isCategoryExist = _repository
                .IsCategoryTitleExist(dto.Title);
           
            if(isCategoryExist)
            {
                throw new CategoryIsAlreadyExistException();
            }
           
            _repository.Add(category);

            _unitOfWork.Commit();
        }

        public IList<GetCategoryDto> GetAll()
        {
          return _repository.GetAll();
        }

        public void Update(int id, UpdateCategoryDto dto)
        {
            var category = _repository.FindById(id);
            
            PreventUpdateWhenCategoryWithGivenIdDoesNotExist(category);

            category.Title = dto.Title;

            _unitOfWork.Commit();
        }

        private static void PreventUpdateWhenCategoryWithGivenIdDoesNotExist(Category category)
        {
            if (category == null)
            {
                throw new CategoryNotFoundException();
            }
        }

        public void Delete(int id)
        {
            var category = _repository.FindById(id);
            if(category == null)
            {
                throw new CategoryNotFoundException();
            }

            _repository.Delete(category);
            _unitOfWork.Commit();
        }


    }
}
