using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Batchs
{
 
        public class BatchDetailWithChapter
        {

            public int Id { get; set; }
            public string CourseCode { get; set; }
            public string BatchCode { get; set; }
            public string ChapterCode { get; set; }
            public string ChapterName { get; set; }
            public string ChapterDescription { get; set; }
            public DateTime? ExpectedDate { get; set; }
            public DateTime? CompletionDate { get; set; }
            public bool IsCompleted { get; set; }
            public string PresentStudent { get; set; }
            public string AbsentStudent { get; set; }
            public string MeetingLink { get; set; }
            public string RecordingLink { get; set; }
            public string Resource { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public string ModifiedBy { get; set; }
        }

    
}
