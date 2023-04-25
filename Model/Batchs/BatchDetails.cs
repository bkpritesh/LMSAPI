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
        public string BatchCode { get; set; }
        public string ChapterCode { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public bool IsCompleted { get; set; }
        public int PresentStudent { get; set; }
        public int AbsentStudent { get; set; }
        public string MeetingLink { get; set; }
        public string RecordingLink { get; set; }
        public string Resource { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }

}
