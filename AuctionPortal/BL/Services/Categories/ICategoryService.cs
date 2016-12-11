using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Auctions;
using BL.DTOs.Categories;

namespace BL.Services.Categories
{
    public interface ICategoryService
    {
        void CreateCategory(CategoryDTO categoryDTO);
        void EditCategory(CategoryDTO categoryDTO);
        void DeleteCategory(long categoryId);
        CategoryDTO GetCategory(long categoryId);
        long GetCategoryId(string name);
        IEnumerable<CategoryDTO> GetAllCategories();
    }
}
