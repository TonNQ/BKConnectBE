using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BKConnectBE.Common
{
    public static class Helper
    {
        public static string GetDomainName(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}";
        }

        public static string RemoveUnicodeSymbol(string word)
        {
            try
            {
                if (string.IsNullOrEmpty(word))
                {
                    return "";
                }
                string temp = word.Normalize(NormalizationForm.FormD);
                string result = Regex.Replace(temp, @"\p{M}", string.Empty);
                result = result.Replace('đ', 'd').Replace('Đ', 'D').ToLower();
                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static TEnum ToEnum<TEnum>(this string value) where TEnum : Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    return attribute.Description;
                }
            }
            return value.ToString();
        }
    }
}