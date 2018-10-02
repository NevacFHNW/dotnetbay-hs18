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
    public partial class BidView : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly Regex Regex = new Regex("[^0-9.]+");
        public Auction auction;
        public string AuctionTitle { get; set; }
        public string Description { get; set; }
        public double StartPrice { get; set; }
        public double CurrentPrice { get; set; }
        public byte[] Image { get; set; }
        private double _bid;
        public double Bid
        {
            get => _bid;
            set
            {
                if (IsTextAllowed(value.ToString()))
                {
                    _bid = value;
                }
            }
        }

        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        

        public BidView(Auction auction)
        {
            InitializeComponent();
            DataContext = this;

            this.auction = auction;
            AuctionTitle = auction.Title;
            Description = auction.Description;
            StartPrice = auction.StartPrice;
            CurrentPrice = auction.CurrentPrice;
            Image = auction.Image;
        }

        private void Place_Bid_Button_Click(object sender, RoutedEventArgs e)
        {

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

        private static bool IsTextAllowed(string text)
        {
            return !Regex.IsMatch(text);
        }
    }
}
