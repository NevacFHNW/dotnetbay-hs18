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
    public partial class BidView : Window
    {
        private readonly Auction selectedAuction;

        private readonly AuctionService auctionService;

        public double YourBid { get; set; }

        public Auction SelectedAuction
        {
            get
            {
                return this.selectedAuction;
            }
        }

        public BidView(Auction selectedAuction)
        {
            this.selectedAuction = selectedAuction;
            this.InitializeComponent();

            this.DataContext = this;

            var app = Application.Current as App;

            if (app != null)
            {
                SimpleMemberService simpleMemberService  = new SimpleMemberService(app.MainRepository);
                this.auctionService = new AuctionService(app.MainRepository, simpleMemberService);
            }

            this.YourBid = Math.Max(this.SelectedAuction.CurrentPrice, this.SelectedAuction.StartPrice);
        }

        private void PlaceBidAuction_Click(object sender, RoutedEventArgs e)
        {
            // store new bid
            this.auctionService.PlaceBid(this.SelectedAuction, this.YourBid);

            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
