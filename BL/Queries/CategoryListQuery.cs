using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Categories;
using BL.DTOs.Filters;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class CategoryListQuery : AppQuery<CategoryDTO>
    {
        public CategoryFilter Filter { get; set; }
        public CategoryListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<CategoryDTO> GetQueryable()
        {
            IQueryable<Category> query = Context.Categories;
            if (!string.IsNullOrEmpty(Filter?.Name))
            {
                query = query.Where(c => c.Name.ToLower().Contains(Filter.Name.ToLower()));
            }

            return query.ProjectTo<CategoryDTO>();
        }
    }
}
