using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public  interface IBatch
    {
        Task<IEnumerable<Batch>> GetBatch();

        Task<Batch> UpdateBatch(Batch Batch);

        Task<Batch> CreateBatch(Batch Batch);

        Task<ActionResult<string>> GetLastBatchID();
    }
}
