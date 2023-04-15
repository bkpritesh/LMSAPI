using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BatchDetails
    {
        public string? CourseCode { get; set; }
        public string? BatchCode { get; set; }
        public IFormFile File { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool? IsCompleted { get; set; }
        public int? PresentStudentsCount { get; set; }
        public int? AbsentStudentsCount { get; set; }


        public string? MeetingLink { get; set; }
        public string? RecordingLink { get; set; }
        public string? Resource { get; set; }
    }
}
