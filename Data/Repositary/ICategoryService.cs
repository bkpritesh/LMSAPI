using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
   public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategoryByID(string id);
   

        Task<AddCategoriesOnly> CreateCategory(AddCategoriesOnly categories);
        Task<Category> UpdateCategory(Category category);
    
        Task<IEnumerable<Category>> DeleteCategory(string id);
    
    }
}
