using DotNetBay.Core;
using DotNetBay.Data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Auction> auctions = new ObservableCollection<Auction>();
        public ObservableCollection<Auction> Auctions => auctions;
        private AuctionService auctionService;

        public MainWindow()
        {
            InitializeComponent();
            App app = (App) Application.Current;
            auctionService = new AuctionService(app.MainRepository, new SimpleMemberService(app.MainRepository));
            auctions = new ObservableCollection<Auction>(auctionService.GetAll());
            DataContext = this;
        }
    }
}
