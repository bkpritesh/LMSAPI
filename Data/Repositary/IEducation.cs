﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public interface  IEducation
    {
        Task<IEnumerable<dynamic>> GetEducaton();

        Task<IEnumerable<dynamic>> GetSkills();
    }
}
 