using System.ComponentModel;

namespace LibraryApp.Models
{
    public enum Professionals
    {
        [Description("Artenia")]
        Artenia,
        [Description("Carol")]
        Carol,
        [Description("Cristiane")]
        Cristiane,
        [Description("Davi")]
        Davi,
        [Description("Elias")]
        Elias,
        [Description("Fred")]
        Fred,
        [Description("Gabriela")]
        Gabriela,
        [Description("Gerlucio")]
        Gerlucio,
        [Description("Gleidson")]
        Gleidson,
        [Description("Iracema")]
        Iracema,
        [Description("Jessica")]
        Jessica,
        [Description("Laura")]
        Laura,
        [Description("Leones")]
        Leones,
        [Description("Ligonardo")]
        Ligonardo,
        [Description("Marcos")]
        Marcos,
        [Description("Nara")]
        Nara,
        [Description("ONHB")]
        ONHB,
        [Description("Otilia")]
        Otilia,
        [Description("Paulo")]
        Paulo,
        [Description("PC")]
        PC,
        [Description("Regiane")]
        Regiane,
        [Description("Renne")]
        Renne,
        [Description("Sandro")]
        Sandro
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