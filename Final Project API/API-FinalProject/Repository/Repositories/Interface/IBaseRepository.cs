using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Repository.Repositories.Interface
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
    }
}
