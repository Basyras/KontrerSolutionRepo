using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client;

public interface IMessage
{
}

public interface IMessage<TResponse>
    where TResponse : class
{
}
