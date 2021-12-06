using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.Shared.Helpers
{
    public static class GenericsHelper
    {
        public static bool IsAssignableToGenericType(Type childType, Type parentType)
        {
            var interfaceTypes = childType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == parentType)
                    return true;
            }

            if (childType.IsGenericType && childType.GetGenericTypeDefinition() == parentType)
                return true;

            Type baseType = childType.BaseType;
            if (baseType == null)
                return false;

            return IsAssignableToGenericType(baseType, parentType);
        }

        public static bool IsAssignableToGenericType<TChildType>(Type parentType)
        {
            var childType = typeof(TChildType);
            return IsAssignableToGenericType(childType, parentType);


        }

        /// <summary>
        /// Get generic argument from base class
        /// </summary>
        /// <param name="childType"></param>
        /// <param name="parentType">parent type that should contain generic parameters</param>
        /// <returns></returns>

        public static Type[] GetTypeArgumentsFromParent(Type childType, Type parentType)
        {
            if (parentType.IsInterface)
            {
                var baseInterface = childType.GetInterface(parentType.Name);
                return baseInterface.GetGenericArguments();
            }

            while (childType.BaseType != null)
            {
                childType = childType.BaseType;
                if (childType.IsGenericType && childType.GetGenericTypeDefinition() == parentType)
                {
                    return childType.GetGenericArguments();
                }
            }
            if (childType != typeof(object))
            {
                throw new InvalidOperationException("Class does not have specified base class");
            }
            else
            {
                return new Type[0];
            }
        }
    }
}