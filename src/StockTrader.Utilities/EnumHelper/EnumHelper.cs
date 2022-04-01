using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StockTrader.Utilities.EnumHelper
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets the description attribute of an enum.
        /// </summary>
        /// <typeparam name="TEnum">Generic that is constrained to Enumerators only.</typeparam>
        /// <param name="original">Original Enum.</param>
        /// <returns>
        /// Description atrribute as string.
        /// </returns>
        public static string ToDescription<TEnum>(this TEnum original) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            // Get the Description attribute value for the enum value
            FieldInfo fi = original.GetType().GetField(original.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi?.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes?.Length > 0)
                return attributes[0].Description;
            else
                return original.ToString();
        }

        /// <summary>
        /// Gets the enum of an description attribute.
        /// </summary>
        /// <typeparam name="TEnum">Generic that is constrained to Enumerators only.</typeparam>
        /// <param name="description">Description attribute</param>
        /// <returns>
        /// The enum. 
        /// </returns>
        public static TEnum ToEnum<TEnum>(this string description) where TEnum : struct, IConvertible
        {
            foreach (FieldInfo field in typeof(TEnum).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (TEnum)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (TEnum)field.GetValue(null);
                }
            }

            return default(TEnum);
        }

    }
}
