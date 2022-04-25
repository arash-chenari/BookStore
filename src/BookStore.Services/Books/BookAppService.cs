using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Services.Books.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Books
{
    public class BookAppService : BookService
    {
        private readonly BookRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        
        public BookAppService(
            BookRepository repositoy,
            UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repositoy;
        }

        public void Add(AddBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                Pages = dto.Pages,
                CategoryId = dto.CategoryId,
            };

            _repository.Add(book);
            _unitOfWork.Commit();
        }
    }
}
