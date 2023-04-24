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


        //get the Assesstment bY THE COURSE ID 
        Task<dynamic> GetAssesstmentByCourseId(string CourseCode);
      
        //get the Question bY THE Assessment CODE 
        Task<dynamic> GetQuestionsByAssesstmentId(string AssessmentCode);


       // Task<ExamResult> SubmitQuizResults(Dictionary<string, string> quizData, AssesstANDStudCode AstANDStud);
        //    Task<int> SubmitQuizResults(Dictionary<string, string[]> quizData);


        Task<ExamResult> SubmitQuizResults(SubmitQuizModel submitQuizModel);
    }
}
