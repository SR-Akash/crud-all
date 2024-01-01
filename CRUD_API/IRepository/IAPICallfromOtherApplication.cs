using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.IRepository
{
    public interface IAPICallfromOtherApplication
    {
        public Task<string> Create();
    }
}
