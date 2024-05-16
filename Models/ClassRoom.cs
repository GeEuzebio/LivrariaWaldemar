using System.ComponentModel;

namespace LibraryApp.Models
{
    public enum ClassRoom
    {
        [Description("1º Ano A")]
        _1A,
        [Description("1º Ano B")]
        _1B,
        [Description("1º Ano C")]
        _1C,
        [Description("1º Ano D")]
        _1D,
        [Description("2º Ano A")]
        _2A,
        [Description("2º Ano B")]
        _2B,
        [Description("2º Ano C")]
        _2C,
        [Description("3º Ano A")]
        _3A,
        [Description("3º Ano B")]
        _3B,
        [Description("EJA A")]
        EJA_A,
        [Description("EJA B")]
        EJA_B,
        [Description("EJA C")]
        EJA_C
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
}