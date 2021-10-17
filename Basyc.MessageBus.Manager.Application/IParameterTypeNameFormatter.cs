using System;

namespace Basyc.MessageBus.Manager.Application
{
    public interface IParameterTypeNameFormatter
    {
        string GetCustomTypeName(Type type);
    }
}