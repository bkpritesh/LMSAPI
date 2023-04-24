using System;
using System.Collections.Generic;

namespace Model.Assistment
{
    public class SubmitQuizModel
    {
        public List<QuizQuestion> QuizQuestions { get; set; }
        public AssesstANDStudCode AssesstANDStudCode { get; set; }
    }

    public class QuizQuestion
    {
        public string? Question { get; set; }
        public string? Answer { get; set; }
    }
}
