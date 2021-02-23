using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories
{
    public interface IRepository 
    {
        //Should not implement save method when used with unit of work pattern
    }
}