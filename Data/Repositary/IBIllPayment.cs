﻿using Model.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public interface  IBIllPayment
    {
        Task<BillPayment> BillPayment(BillPayment billPayment);
    }
}
