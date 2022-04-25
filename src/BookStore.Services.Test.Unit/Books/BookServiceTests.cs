using BookStore.Infrastructure.Application;
using BookStore.Infrastructure.Test;
using BookStore.Persistence.EF;
using BookStore.Persistence.EF.Books;
using BookStore.Services.Books;
using BookStore.Services.Books.Contracts;
using BookStore.Test.Tools.categories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Services.Test.Unit.Books
{
    public class BookServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly BookService _sut;
        private readonly BookRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public BookServiceTests()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _repository = new EFBookRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new BookAppService(_repository,_unitOfWork);
        }

        [Fact]
        public void Add_adds_book_properly()
        {
            var category = CategoryFactory.CreateCategory("Dummy");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            
            AddBookDto dto = GenerateAddCategoryDto(category);
            _sut.Add(dto);

            var expected = _dataContext.Books
                .Include(_ => _.Category)
                .FirstOrDefault();
            expected.Category.Id.Should().Be(category.Id);
            expected.Title.Should().Be(dto.Title);
            expected.Author.Should().Be(dto.Author);
            expected.Description.Should().Be(dto.Description);
            expected.Pages.Should().Be(dto.Pages);
        }

        private static AddBookDto GenerateAddCategoryDto(Entities.Category category)
        {
            return new AddBookDto
            {
                Title = "dummy",
                Author = "dummyAuthor",
                Description = "a book for dummies",
                Pages = 10,
                CategoryId = category.Id
            };
        }
    }

}
