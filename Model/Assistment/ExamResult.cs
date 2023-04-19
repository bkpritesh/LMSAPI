using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Assistment
{
    public class ExamResult
    {
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public float Percentage { get; set; }

        public bool IsPass { get; set; }
    }
}
