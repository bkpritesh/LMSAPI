using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Assistment
{
    public class AssesstmentCodeANDCourseCode
    {
      
        public string CourseCode { get; set; }

        public IFormFile File { get; set; }
    }
}
