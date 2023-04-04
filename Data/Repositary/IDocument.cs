using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{

    public interface IDocument
    {
        ////Task<string> Upload(Document document);
        //Task<Document> AddDocumentAsync(Document document, Stream stream);
        ////  Task<FileUploadResult> UploadFileAsync(IFormFile file, int id, string name, string type);



        Task<int> AddDocument(string filePath, string DocumentType, string AccountId);
    }
}
