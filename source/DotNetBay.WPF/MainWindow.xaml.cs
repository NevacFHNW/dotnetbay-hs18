using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DotNetBay.Core;
using DotNetBay.Data.Entity;
using System.Windows.Data;
using System;
using System.Globalization;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Auction> Auctions
        {
            get => this.auctions;

            private set
            {
                this.auctions = value;
                this.OnPropertyChanged();
            }
        }

        private readonly AuctionService auctionService;

        ObservableCollection<Auction> auctions = new ObservableCollection<Auction>();

        public MainWindow()
        {
            var app = Application.Current as App;

            InitializeComponent();

            this.DataContext = this;

            // get list of auctions
            if (app != null)
            {
                this.auctionService = new AuctionService(app.MainRepository, new SimpleMemberService(app.MainRepository));
                this.auctions = new ObservableCollection<Auction>(this.auctionService.GetAll());
            }
        }

        private void newAuctionBtn_Click(object sender, RoutedEventArgs e)
        {
            var sellView = new SellView();
            sellView.ShowDialog(); // Blocking

            var allAuctionsFromService = this.auctionService.GetAll();

            /* Option A: Full Update via INotifyPropertyChanged, not performant */
            /* ================================================================ */
            this.Auctions = new ObservableCollection<Auction>(allAuctionsFromService);

            /////* Option B: Let WPF only update the List and detect the additions */
            /////* =============================================================== */
            ////var toAdd = allAuctionsFromService.Where(a => !this.auctions.Contains(a));
            ////foreach (var auction in toAdd)
            ////{
            ////    this.auctions.Add(auction);
            ////}
        }

        private void bidAuctionBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedAuction = (Auction)AuctionsDataGrid.SelectedItem;
            var bidView = new BidView(selectedAuction);
            bidView.ShowDialog();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BooleanToStatusTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value)
                return "closed";
            else
                return "open";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            switch (value.ToString().ToLower())
            {
                case "closed":
                    return true;
                case "open":
                    return false;
            }
            return false;
        }
    }
}
