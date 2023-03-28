using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Data.Repositary;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Data.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IDbConnection _dbConnection;

        public CategoryService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var results = await _dbConnection.QueryAsync<Category>("GetCategory", commandType: CommandType.StoredProcedure);
            return results;
        }
        public async Task<Category> GetCategoryByID(string id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", id);

            var results = await _dbConnection.QueryAsync<Category>("GetCategory", parameters, commandType: CommandType.StoredProcedure);
            return results.FirstOrDefault();
        }

        public async Task<AddCategoriesOnly> CreateCategory(AddCategoriesOnly categories)
        {
        

            var NewCId = await _dbConnection.ExecuteScalarAsync<string>("SELECT TOP 1 CategoryId FROM TBLCategory ORDER BY CategoryId DESC");

            int NewCategoryId;
            if (!string.IsNullOrEmpty(NewCId))
            {
                if (!int.TryParse(NewCId.Substring(5), out NewCategoryId))
                {
                    NewCategoryId = 0;
                }            
            }else
            {
                NewCategoryId = 0;
            }
            var nextCategoryId = $"CTID-{NewCategoryId + 1:D4}";


            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", nextCategoryId);
            parameters.Add("@Name", categories.Name);
            parameters.Add("@Description", categories.Description);

            var results = await _dbConnection.QueryAsync<AddCategoriesOnly>("CreateCategory", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", category.CategoryId);
            parameters.Add("@Name", category.Name);
            parameters.Add("@Description", category.Description);

            var results = await _dbConnection.QueryAsync<Category>("UpdateCategory", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }

      
        public async Task<IEnumerable<Category>> DeleteCategory(string id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", id);

            var results = await _dbConnection.QueryAsync<Category>("DeleteCategory", parameters, commandType: CommandType.StoredProcedure);
            return results;
        }


        public async Task<IEnumerable<dynamic>> GetCourseByCategoryID(string CategoryId)
        {

            var results = await _dbConnection.QueryAsync("[GetCoursebyCategoryID]", new { CategoryId }, commandType: CommandType.StoredProcedure);

            return results;
        }
    }
}
