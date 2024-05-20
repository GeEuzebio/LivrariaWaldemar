using System.ComponentModel;

namespace LibraryApp.Models
{
    public enum Professionals
    {
        [Description("Renê")]
        Rene,
        [Description("Laura")]
        Laura
    }
}
public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        var descriptionAttributes = (DescriptionAttribute[])fieldInfo!.GetCustomAttributes(
            typeof(DescriptionAttribute), false);

        return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : value.ToString();
    }
}