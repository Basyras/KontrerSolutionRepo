using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Shared
{
    public static class TypedToSimpleConverter
    {
        public static string ConvertTypeToSimple(Type type)
        {           
            return type.AssemblyQualifiedName!;
        }

        public static string ConvertTypeToSimple<TType>()
        {
            return ConvertTypeToSimple(typeof(TType));
        }

        public static Type ConvertSimpleToType(string requestType)
        {
            return Type.GetType(requestType)!;
        }
    }
}
