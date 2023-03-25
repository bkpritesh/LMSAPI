using Dapper;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Services.DocumentService;
using Microsoft.AspNetCore.Hosting;
using Data.Repositary;
using System.Data.Common;

namespace Data.Services
{
    public class DocumentService : IDocument
    {

        private readonly IDbConnection _dbConnection;

        public DocumentService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }


        //public async Task<Document> AddDocumentAsync(Document document, Stream stream)
        //{
        //    var fileName = Path.GetFileName(document.DocumentPath);
        //    var filePath = Path.Combine("//Uploads//Images//", fileName);

        //    // Create the directory if it doesn't exist
        //    if (!Directory.Exists("//Uploads//Images//"))
        //    {
        //        Directory.CreateDirectory("//Uploads//Images//");
        //    }

        //    // Save the file to disk
        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await stream.CopyToAsync(fileStream);
        //    }
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@DocumentPath", filePath);
        //    // Save the file path to the database

        //        await _dbConnection.ExecuteAsync("AddDocument", new { FilePath = filePath },
        //            commandType: CommandType.StoredProcedure);

        //       // document.DocumentPath = filePath;


        //    return document;
        //}











        public async Task<int> AddDocument(string filePath, string DocumentType, string AccountId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AccountId",AccountId);
            parameters.Add("@DocumentType", DocumentType);
            parameters.Add("@DocumentPath", filePath);

            var result = await _dbConnection.ExecuteAsync("AddDocument", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }












    }
}
