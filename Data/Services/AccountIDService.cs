using Data.Repositary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public  class AccountIDService: IAccountID
    {
       public  Guid AccountId { get; set; }

        public string StudentID { get; set; }
    }
}
