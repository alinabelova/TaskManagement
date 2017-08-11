using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.MVVM.Utils
{
    public static class EnumHelper
    {
        /// <summary>
        /// Получает коллекцию пар: значение - DisplayNameAttribute
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetAllValuesAndDisplayAttributes<TEnum>() where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an Enumeration type");
            }

            return from e in Enum.GetValues(typeof(TEnum)).Cast<Enum>()
                select new KeyValuePair<string, string>(e.ToString(), e.GetAttributeOfType<DisplayAttribute>().Name);
        }
    }
}
