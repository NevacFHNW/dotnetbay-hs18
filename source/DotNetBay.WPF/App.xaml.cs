using DotNetBay.Core.Execution;
using DotNetBay.Data.Provider.FileStorage;
using DotNetBay.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using DotNetBay.Core;
using DotNetBay.Data.Entity;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public readonly IMainRepository MainRepository;
        public readonly AuctionRunner AuctionRunner;

        public App()
        {
            MainRepository = new FileSystemMainRepository("repository/data/rep");
            AuctionRunner = new AuctionRunner(MainRepository);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AuctionRunner.Start();
            InitializeDemoRepository();

        }

        private void InitializeDemoRepository()
        {
            var memberService = new SimpleMemberService(this.MainRepository);
            var service = new AuctionService(this.MainRepository, memberService);

            if (!service.GetAll().Any())
            {
                var me = memberService.GetCurrentMember();

                service.Save(new Auction
                {
                    Title = "My First Auction",
                    StartDateTimeUtc = DateTime.UtcNow.AddSeconds(10),
                    EndDateTimeUtc = DateTime.UtcNow.AddDays(14),
                    StartPrice = 72,
                    Seller = me
                });
            }
        }
    }
}
