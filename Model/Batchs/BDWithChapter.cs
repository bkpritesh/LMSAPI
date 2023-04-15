using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BDWithChapter
    {
        public string? CourseCode { get; set; }
        public string? BatchCode { get; set; }
        public IFormFile? File { get; set; }
     
    }
}
