using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Students
{
    public  class BillPayment
    {
        
            public int InvoiceNO { get; set; }
            public string AccountID { get; set; }
            public string StudentCode { get; set; }
            public decimal Amount { get; set; }
            public string PaymentType { get; set; }
            public bool IsPaid { get; set; }
            public string CourseCode { get; set; }
        

    }
}
