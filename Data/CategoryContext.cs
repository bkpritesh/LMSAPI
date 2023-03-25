//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//public class CategoryContext
//{
    
//    private readonly IDbConnection _dbConnection;
      
//    public CategoryContext(IConfiguration configuration)
//    {
//        var connectionString = configuration.GetConnectionString("SqlConnection");
//        _dbConnection = new SqlConnection(connectionString);
//    }

//    public IDbConnection CreateConnection()
//    {
//        return _dbConnection;
//    }
//}
