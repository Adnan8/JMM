using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

public class NullToDefaultActionFilter : IActionFilter
{
	public void OnActionExecuting(ActionExecutingContext context)
	{
		foreach (var argument in context.ActionArguments.Values.Where(arg => arg != null))
		{
			// Check if the argument is a simple type or not
			if (IsSimpleType(argument.GetType()))
			{
				// Skip simple types like int, string, etc.
				continue;
			}

			var properties = argument.GetType().GetProperties();

			foreach (var property in properties)
			{
				var propertyValue = property.GetValue(argument);
				if (propertyValue == null)
				{
					// Determine the appropriate default value for the property
					var defaultValue = GetDefaultValue(property.PropertyType);
					property.SetValue(argument, defaultValue);
				}
			}
		}
	}

	public void OnActionExecuted(ActionExecutedContext context)
	{
		// No implementation needed for this scenario
	}

	private static bool IsSimpleType(Type type)
	{
		return type.IsPrimitive || type.IsEnum || type.Equals(typeof(string)) || type.Equals(typeof(decimal)) || Convert.GetTypeCode(type) != TypeCode.Object || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
	}

	private static object GetDefaultValue(Type type)
	{
		if (type == typeof(string))
		{
			return string.Empty;
		}
		else if (type.IsValueType)
		{
			return Activator.CreateInstance(type);
		}
		else if (!type.IsValueType && type.GetConstructor(Type.EmptyTypes) != null)
		{
			return Activator.CreateInstance(type);
		}
		return null;
	}
}
