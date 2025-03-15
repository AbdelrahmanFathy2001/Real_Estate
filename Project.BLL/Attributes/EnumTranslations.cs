using Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Attributes
{
    public static class EnumTranslations
    {
        private static readonly Dictionary<Enum, string> Translations = new Dictionary<Enum, string>
        {
            { RequestType.Direct, "مباشر" },
            { RequestType.Indirect, "طرف ثالث" },
            { UnitType.Land, "أرض" },
            { UnitType.Floor, "دور" },
            { UnitType.Apartment, "شقة" },
            { UnitType.Villa, "فيلا" },
            { UnitType.VillaDuplex, "دوبلكس" },
            { UnitType.ApartmentDuplex, "شقة دوبلكس" },
            { UnitType.Building, "عمارة" },
            { UnitType.Tower, "برج" },
            { UnitType.Complex, "مجمع" },
            { UnitType.Kiosk, "كشك" },
            { UnitType.Shop, "محل" },
            { UnitType.Exhibition, "معرض" },
            { DealType.Rent, "تأجير" },
            { DealType.Sell, "بيع" },
            { DealType.Buy, "شراء" }
        };

        public static TEnum GetEnumFromTranslation<TEnum>(string arabicValue) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(arabicValue))
                throw new ArgumentException("القيمة المدخلة فارغة.");

            var enumValue = Translations
                .Where(kv => kv.Value == arabicValue)
                .Select(kv => kv.Key)
                .FirstOrDefault();

            if (enumValue == null)
                throw new ArgumentException($"القيمة '{arabicValue}' غير معروفة أو غير مدعومة.");

            if (enumValue is TEnum validEnum)
                return validEnum;

            throw new InvalidOperationException($"القيمة '{arabicValue}' غير متطابقة مع النوع {typeof(TEnum).Name}.");
        }

        public static string GetTranslation(Enum value)
        {
            return Translations.TryGetValue(value, out var translation) ? translation : value.ToString();
        }
    }
}
