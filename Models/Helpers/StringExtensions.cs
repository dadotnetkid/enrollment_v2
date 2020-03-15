using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Helpers
{
    public static class StringExtensions
    {

        public static DayOfWeek? ToDayOfWeek(this string day)
        {

            DayOfWeek dayOfWeek = DayOfWeek.Monday;
            if (day.ToLower() == "m")
            {
                dayOfWeek = DayOfWeek.Monday;
            }
            if (day.ToLower() == "t")
            {
                dayOfWeek =DayOfWeek.Tuesday;
            }
            if (day.ToLower() == "w")
            {
                dayOfWeek = DayOfWeek.Wednesday;
            }
            if (day.ToLower() == "th")
            {
                dayOfWeek = DayOfWeek.Thursday;
            }
            if (day.ToLower() == "f")
            {
                dayOfWeek = DayOfWeek.Friday;
            }
            if (day.ToLower() == "sat")
            {
                dayOfWeek = DayOfWeek.Saturday;
            }
            if (day.ToLower() == "sun")
            {
                dayOfWeek = DayOfWeek.Sunday;
            }
            return dayOfWeek;
        }

        public static int ToInt(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static decimal ToDecimal(this object obj)
        {
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static string ImageToBase64(byte[] array)
        {
            try
            {
                var base64 = Convert.ToBase64String(array);
                return $"data:image/png;base64,{base64}";
            }
            catch (Exception e)
            {
                var ms = new MemoryStream();
                var img = Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/admin-lte/img/avatar2.png"));
                img.Save(ms, ImageFormat.Png);
                var base64 = Convert.ToBase64String(ms.ToArray());
                return $"data:image/png;base64,{base64}";
            }

        }
    }
}
