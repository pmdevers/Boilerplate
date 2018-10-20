using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMDEvers.Boilerplate.Abstractions
{
    public interface IBoilerplateClient
    {
	    Task<IBoilerplate> GetAsync(CancellationToken cancellation = default(CancellationToken));
    }
}
