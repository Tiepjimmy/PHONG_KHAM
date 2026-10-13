using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ClinicManagement.Model.Helper
{
    public static class EnumHelper<T>
    {
        public static IList<T>? GetValues(System.Enum value)
        {
            List<T> enumValues = [];
            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)System.Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static T? Parse(string value)
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(System.Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string>? GetDisplayValues(System.Enum value) =>
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

        private static string? LookupResource(Type resourceManagerProvider, string resourceKey)
        {
            foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    System.Resources.ResourceManager resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    if (resourceManager != null) return resourceManager.GetString(resourceKey);
                }
            }
            return resourceKey;
        }

        public static string? GetDisplayValue(T? value)
        {
            FieldInfo? fieldInfo = value?.GetType().GetField(value.ToString()!);
            if (fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), false) is DisplayAttribute[] descriptionAttributes && descriptionAttributes.Length != 0)
            {
                if (descriptionAttributes[0].ResourceType != null)
#pragma warning disable CS8604 // Possible null reference argument.
                    return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);
#pragma warning restore CS8604 // Possible null reference argument.
                return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value?.ToString();
            }
            else
            {
                return fieldInfo?.Name;
            }
        }

        public static T? GetValueFromName(string name)
        {
            Type type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DisplayAttribute)) is DisplayAttribute attribute)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    if (attribute.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        return (T)field.GetValue(null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
                else
                {
                    if (field.Name == name)
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        return (T)field.GetValue(null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                }
            }
            throw new ArgumentOutOfRangeException(nameof(name));
        }
    }

    public static class EnumHelper
    {
        public static T? GetValueFromDescription<T>(this string description) where T : System.Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        return (T)field.GetValue(null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        return (T)field.GetValue(null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    }
                }
            }
            return default;
        }

        public static string? GetDescription<T>(this T enumValue)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                return null;
            }
            string? description = enumValue.ToString();
#pragma warning disable CS8604 // Possible null reference argument.
            FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
            if (fieldInfo != null)
            {
                object[] attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return description;
        }

        public static string GetEnumDescription(System.Enum value)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            FieldInfo fi = value.GetType().GetField(value.ToString());
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable IDE0019 // Use pattern matching
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            DescriptionAttribute[] attributes = fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore IDE0019 // Use pattern matching

            if (attributes != null && attributes.Length != 0)
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}
