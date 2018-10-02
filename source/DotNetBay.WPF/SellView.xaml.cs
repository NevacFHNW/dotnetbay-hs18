using DotNetBay.Core;
using DotNetBay.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for SellView.xaml
    /// </summary>
    public partial class SellView : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly Regex Regex = new Regex("[^0-9.]+");
        public string AuctionTitle { get; set; }
        public string Description { get; set; }
        private double _startPrice;
        public double StartPrice
        {
            get => _startPrice;
            set
            {
                if (IsTextAllowed(value.ToString()))
                {
                    _startPrice = value;
                }
            }
        }

        private string _image;
        public string Image
        {
            get => _image;
            set
            {
                _image = value;
                NotifyPropertyChanged("Image");
            }

        }

        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        

        public SellView()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Add_Auction_Button_Click(object sender, RoutedEventArgs e)
        {
            var app = (App) Application.Current;
            var memberService = new SimpleMemberService(app.MainRepository);
            var service = new AuctionService(app.MainRepository, memberService);
            var me = memberService.GetCurrentMember();
            var sellView = this;

            // Load Image
            byte[] image = null;
            if (Image != null)
            {
                var uri = new Uri(Image);
                image = getJPGFromImageControl(new BitmapImage(uri));
            }

            try
            {
                var startTimeString = string.Format("{0:yyyy-MM-dd HH:mm:ss}", sellView.StartTime.Value);
                DateTime startTime = DateTime.ParseExact(startTimeString, "yyyy-MM-dd HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture);
                var endTimeString = string.Format("{0:yyyy-MM-dd HH:mm:ss}", sellView.EndTime.Value);
                DateTime endTime = DateTime.ParseExact(endTimeString, "yyyy-MM-dd HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture);

                service.Save(new Auction
                {
                    Id = 1,
                    Title = sellView.AuctionTitle,
                    Description = sellView.Description,
                    StartDateTimeUtc = startTime,
                    EndDateTimeUtc = endTime,
                    StartPrice = sellView.StartPrice,
                    Image = image,
                    Seller = me
                });

                Close();
            }
            catch (System.FormatException ex)
            {
                // Do Something here
            }
        }

        private byte[] getJPGFromImageControl(BitmapImage image)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Image_Choose_Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Image"; // Default file name
            dlg.DefaultExt = ".jpg"; // Default file extension
            dlg.Filter = "Images (.jpg)|*.jpg" +
                         "|All Types|*"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                Image = dlg.FileName;
            }
            else
            {
                Image = "";
            }
        }

        private static bool IsTextAllowed(string text)
        {
            return !Regex.IsMatch(text);
        }
    }
}
