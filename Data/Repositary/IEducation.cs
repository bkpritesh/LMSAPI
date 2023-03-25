using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public interface  IEducation
    {
        Task<IEnumerable<Education>> GetSkills();

        Task<IEnumerable<Education>> GetQualification();
    }
}
