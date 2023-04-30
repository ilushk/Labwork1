using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Labwork1.MainWindow;
using System.Drawing;
using System.IO;
using System.Xml.Linq;


namespace Labwork1
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private void GenerateQRCodeButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение ввода текста из текстовых полей
            string text1 = Name.Text;
            string text2 = Price.Text;
            string text3 = Description.Text;

            // Объедините вводимые тексты в одну строку
            string text = $"{text1} {text2} {text3}";

            // Сгенерируйте растровое изображение QR-кода с помощью библиотеки QRCoder
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic(20);

            // Преобразование растрового изображения QR-кода в растровое изображение WPF
            BitmapImage qrCodeImage = new BitmapImage();
            qrCodeImage.BeginInit();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            qrCodeBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            qrCodeImage.StreamSource = memoryStream;
            qrCodeImage.EndInit();

            // Отображение изображения QR-кода в элементе управления изображением WPF
            QRCodeImage.Source = qrCodeImage;
        }
        public User User { get; private set; }
        public UserWindow(User user)
        {
            InitializeComponent();
            User = user;
            DataContext = User;
            string a = User.Name + " " + User.Price + " " + User.Description;
            QRCodeGenerator qr = new QRCodeGenerator();
            QRCodeData data = qr.CreateQrCode(a, QRCoder.QRCodeGenerator.ECCLevel.L);
            QRCode code = new QRCode(data);
            Bitmap bitmap = code.GetGraphic(20);
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                User.ImgDynamic = bitmapimage;
            }
        }


        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
