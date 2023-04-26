using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public  interface IBatchDetail
    {
        Task<BDWithChapter> CreateBatchDetail(BDWithChapter batchDetails, ChatperBinding chatper);

        Task<BatchDetails> UpdateBatchDetail(BatchDetails model);
        Task<IEnumerable<dynamic>> GetStudentByBCode(string Bcode);
     //   Task<IEnumerable<dynamic>> GetDetailByBCHCode(string Bcode, string chapterCode);

        Task<BatchDetails> GetDetailByBCHCode(string Bcode, string chapterCode);

        Task<IEnumerable<dynamic>> GetDetailByBCode(string Bcode);
    }
}
