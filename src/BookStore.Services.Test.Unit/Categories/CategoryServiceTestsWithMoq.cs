using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Services.Categories;
using BookStore.Services.Categories.Contracts;
using BookStore.Services.Categories.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Services.Test.Unit.Categories
{
    public class CategoryServiceTestsWithMoq
    {
        Mock<CategoryRepository> repositroy;
        Mock<UnitOfWork> unitOfWork;
        CategoryService _sut;
        public CategoryServiceTestsWithMoq()
        {
            repositroy
                = new Mock<CategoryRepository>();
            unitOfWork =
                new Mock<UnitOfWork>();
            _sut = new CategoryAppService(repositroy.Object,
                unitOfWork.Object);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            var dto = new AddCategoryDto
            {
                Title = "Dummy"
            };


            _sut.Add(dto);

            repositroy.Verify(_ =>
            _.Add(It.Is<Category>(_ => _.Title == dto.Title)));

            unitOfWork.Verify(_ => _.Commit());
        }


        [Fact]
        public void Add_throw_CategoryIsAlreadyExistException_when_category_with_same_title_is_Exist()
        {
            var dto = new AddCategoryDto
            {
                Title = "Dummy"
            };

            repositroy
                .Setup(_ => _.IsCategoryTitleExist(dto.Title))
                .Returns(true);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<CategoryIsAlreadyExistException>();

        }


        [Fact]
        public void Update_updates_category_properly()
        {
            var dto = new UpdateCategoryDto
            {
                Title = "EditedDummy"
            };
            var category = new Category
            {
                Title = "Dummy",
                Id = 1
            };

            repositroy.Setup(_ => _.FindById(category.Id))
                .Returns(category);

            _sut.Update(category.Id, dto);

            unitOfWork.Verify(_ => _.Commit());
        }

        [Fact]
        public void Delete_deletes_category_properly()
        {
            var category = new Category
            {
                Title = "Dummy",
                Id = 1
            };
            repositroy.Setup(_ => _.FindById(category.Id))
                .Returns(category);

            _sut.Delete(category.Id);

            repositroy.Verify(_ =>
            _.Delete(It.Is<Category>
            (_ => _.Id == category.Id)));
        }

        [Fact]
        public void Delete_throw_CategoryNotFoundException_when_category_with_given_id_dose_not_exist()
        {
            var dummyId = 5;
            Action expected = () => _sut.Delete(dummyId);
            expected.Should().Throw<CategoryNotFoundException>();
        }

        [Fact]
        public void GetAll_returns_all_categories()
        {
            repositroy.Setup(_ => _.GetAll())
                .Returns(new List<GetCategoryDto>
                { new GetCategoryDto
                    {
                        Id = 1, 
                        Title = "dummy"
                    }
                });

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Title == "dummy**");
            expected.Should().Contain(_ => _.Id == 1);
            expected.FirstOrDefault(_ => _.Id == 1)
                .Title.Should().Be("dummy**");

        }
    }
}
