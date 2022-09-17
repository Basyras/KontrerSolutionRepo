using System;
using System.Linq.Expressions;

namespace Basyc.Shared.Helpers
{
	public static class TypeExtensions
	{
		public static object GetDefaultValue(this Type type)
		{
			if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}

		public static object Cast(this Type Type, object data)
		{
			var DataParam = Expression.Parameter(typeof(object), "data");
			var Body = Expression.Block(Expression.Convert(Expression.Convert(DataParam, data.GetType()), Type));

			var Run = Expression.Lambda(Body, DataParam).Compile();
			var ret = Run.DynamicInvoke(data);
			return ret;
		}
	}
}