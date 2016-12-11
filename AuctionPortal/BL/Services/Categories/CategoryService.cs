using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.DTOs.Categories;
using BL.DTOs.Filters;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;

namespace BL.Services.Categories
{
    public class CategoryService : BaseService, ICategoryService
    {

        #region Dependencies

        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryListQuery _categoryListQuery;

        public CategoryService(CategoryRepository categoryRepository, CategoryListQuery categoryListQuery)
        {
            _categoryRepository = categoryRepository;
            _categoryListQuery = categoryListQuery;
        }

        #endregion

        public void CreateCategory(CategoryDTO categoryDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var category = Mapper.Map<Category>(categoryDTO);
                _categoryRepository.Insert(category);
                uow.Commit();
            }
        }

        public void EditCategory(CategoryDTO categoryDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var category = _categoryRepository.GetById(categoryDTO.ID);
                Mapper.Map(categoryDTO, category);

                _categoryRepository.Update(category);
                uow.Commit();
            }
        }

        public void DeleteCategory(long categoryId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _categoryRepository.Delete(categoryId);
                uow.Commit();
            }
        }

        public CategoryDTO GetCategory(long categoryId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var category = _categoryRepository.GetById(categoryId);
                return category == null ? null : Mapper.Map<CategoryDTO>(category);
            }
        }

        public long GetCategoryId(string name)
        {
            using (UnitOfWorkProvider.Create())
            {
                _categoryListQuery.Filter = new CategoryFilter() {Name = name};
                var category = _categoryListQuery.Execute()?.FirstOrDefault();
                return category?.ID ?? 0;
            }
        }

        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            using (UnitOfWorkProvider.Create())
            {
                _categoryListQuery.Filter = null;
                return _categoryListQuery.Execute() ?? new List<CategoryDTO>();
            }
        }
    }
}
