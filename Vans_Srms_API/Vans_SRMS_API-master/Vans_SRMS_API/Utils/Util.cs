using Microsoft.AspNetCore.Http;
using System.Linq;
using Vans_SRMS_API.Models;
using Vans_SRMS_API.Database;
using System.Collections.Generic;

namespace Vans_SRMS_API.Utils
{
    public static class Util
    {
        public static string _DEVICE_ID_HEADER_FIELD = "DeviceKey";

        public enum ShoeType
        {
            Men = 0,
            Women = 1,
            Youth = 2
        }

        public static float ConvertWomensToMensSize(float womensSize)
        {
            return womensSize - 1.5F;
        }
        public static float ConvertMensToWomensSize(float mensSize)
        {
            return mensSize + 1.5F;
        }

        public static float getSizeFromCode(string codedSize)
        {
            switch (codedSize)
            {
                case "010":
                    return 1.0F;
                case "015":
                    return 1.5F;
                case "020":
                    return 2.0F;
                case "025":
                    return 2.5F;
                case "030":
                    return 3.0F;
                case "035":
                    return 3.5F;
                case "040":
                    return 4.0F;
                case "045":
                    return 4.5F;
                case "050":
                    return 5.0F;
                case "055":
                    return 5.5F;
                case "060":
                    return 6.0F;
                case "065":
                    return 6.5F;
                case "070":
                    return 7.0F;
                case "075":
                    return 7.5F;
                case "080":
                    return 8.0F;
                case "085":
                    return 8.5F;
                case "090":
                    return 9.0F;
                case "095":
                    return 9.5F;
                case "100":
                    return 10.0F;
                case "105":
                    return 10.5F;
                case "110":
                    return 11.0F;
                case "115":
                    return 11.5F;
                case "120":
                    return 12.0F;
                case "130":
                    return 13.0F;
                case "140":
                    return 14.0F;
                case "150":
                    return 15.0F;
                case "160":
                    return 16.0F;
                default:
                    return -1F;
            }
        }

        public static ShoeType? getShoeType(string departmentCode)
        {
            switch (departmentCode)
            {
                case "10":
                case "30":
                    return ShoeType.Men;
                case "12":
                    return ShoeType.Women;
                case "11":
                case "13":
                case "14":
                case "15":
                case "31":
                case "32":
                case "33":
                case "34":
                case "35":
                case "36":
                    return ShoeType.Youth;
                default:
                    return null;
            }
        }

        public static int getDeviceId(HttpContext httpContext)
        {
            if (httpContext.Items.ContainsKey(_DEVICE_ID_HEADER_FIELD))
                return int.Parse(httpContext.Items[_DEVICE_ID_HEADER_FIELD].ToString());

            return -1;
        }

        public static int getDefaultStore(SRMS_DbContext context)
        {
            Store store = context.Stores.FirstOrDefault(s => s.IsDefault);

            if (store == null)
                throw new System.Exception("No default store set.");

            return store.StoreId;
        }

        public static List<string> padGTINs(List<string> gtins)
        {
            return gtins
                .Select(g => padGTIN(g))
                .ToList();
        }

        public static string padGTIN(string gtin)
        {
            if (gtin.Length >= 2 && gtin.Substring(0, 2) != "00")
                return $"00{gtin}";

            return gtin;
        }
        public static string unpadGTIN(string gtin)
        {
            if (gtin.Length >= 3 && gtin.Substring(0, 2) == "00")
                return gtin.Substring(2, gtin.Length - 2);

            return gtin;
        }
    }
}
