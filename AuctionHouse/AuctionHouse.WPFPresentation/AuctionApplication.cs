using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AuctionHouse.Domain;
using AuctionHouse.WPFPresentation.View;





namespace AuctionHouse.WPFPresentation
{
    public class AuctionApplication
    {
        private readonly DomainController _domainController;

        public AuctionApplication(DomainController domainController)
        {
            _domainController = domainController
                ?? throw new ArgumentNullException(nameof(domainController));

            // Hier maak je het eerste venster aan
            var window = new MainWindow(_domainController);
            window.Show();
        }
    }
}
