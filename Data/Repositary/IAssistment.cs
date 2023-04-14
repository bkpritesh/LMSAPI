using Model.Assistment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public  interface IAssistment
    {
        Task<Assessment> CreateAssessment(Assessment assessment, AssesstmentCodeANDCourseCode file,string AssesstmentCode);
        Task<string> GetAssesstmentCode();
    }
}
