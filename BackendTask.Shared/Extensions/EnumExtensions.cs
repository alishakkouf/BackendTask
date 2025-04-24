using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Shared.ResultDtos;

namespace BackendTask.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription<TEnum>(this TEnum value, string defaultValue = null) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                return null;

            var type = value.GetType();
            var member = type.GetMember(value.ToString());
            var description = (DescriptionAttribute)member[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

            return description != null ?
                description.Description :
                defaultValue ?? value.ToString();
        }

        /// <summary>
        /// Get list of enum (Id, Name) (T,U) = (Enum type, Enum value type)
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <typeparam name="U">Enum value type</typeparam>
        /// <returns></returns>
        public static List<LiteModelDto<U>> GetListOfEnumValues<T, U>() where T : Enum
        {
            Type t = typeof(T);

            BindingFlags bf = BindingFlags.Static | BindingFlags.Public;
            FieldInfo[] fia = t.GetFields(bf);

            var models = new List<LiteModelDto<U>>();

            foreach (var enumValue in fia)
            {
                models.Add(new LiteModelDto<U>()
                {
                    Id = (U)enumValue.GetValue(null),
                    Name = enumValue.Name
                });
            }

            return models;
        }
    }
}
