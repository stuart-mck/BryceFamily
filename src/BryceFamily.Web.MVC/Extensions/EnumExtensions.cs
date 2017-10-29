using BryceFamily.Web.MVC.Models;

namespace BryceFamily.Web.MVC.Extensions
{
    public static class EnumExtensions
    {
        public static int ToInt(this PersonImport value)
        {
            return (int)value;
        }
    }
}
