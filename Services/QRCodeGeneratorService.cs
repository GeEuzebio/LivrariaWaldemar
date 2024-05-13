using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace LibraryApp.Services
{
    public class QRCodeGeneratorService
    {
        public QRCodeGeneratorService() { }
        public static byte[] QRCodeCreator(string url)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }
        public static string ImageToBase64(byte[] img)
        {
            return "data:image/png;base64," + Convert.ToBase64String(img);
        }
    }
}
