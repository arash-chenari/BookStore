using BookStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Test.Tools.categories
{
    public static class CategoryFactory
    {
        public static Category CreateCategory(string title)
        {
            return new Category
            {
                Title = title
            };
        }
    }
}
