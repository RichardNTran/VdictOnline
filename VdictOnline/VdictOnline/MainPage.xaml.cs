using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using VdictOnline.Resources;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Tasks;
namespace VdictOnline
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        private string PageDictionary;
        private string PageTranslate;
             private string PageMyword;
        public MainPage()
        {
          
                InitializeComponent();
                PageMyword = "/Myword.xaml";
                PageDictionary = "/Dictionary.xaml";
                PageTranslate = "/Translate.xaml";
                FeedbackOverlay.VisibilityChanged += FeedbackOverlay_VisibilityChanged;
            
           
       
        }

        private void FeedbackOverlay_VisibilityChanged(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
            MainLayOut.IsHitTestVisible = (FeedbackOverlay.Visibility != Visibility.Visible);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
   //         if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType !=
   //Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None) 
                base.OnNavigatedTo(e);
            //NetworkInformationUtility.GetNetworkTypeCompleted += GetNetworkTypeCompleted;
            //NetworkInformationUtility.GetNetworkTypeAsync(10000); // Timeout of 3 seconds

        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            NetworkInformationUtility.GetNetworkTypeCompleted -= GetNetworkTypeCompleted;
        }
        private void GetNetworkTypeCompleted(object sender, NetworkTypeEventArgs networkTypeEventArgs)
        {
            string message;

            if (networkTypeEventArgs.HasTimeout)
            {
                message = "The timeout occurred";
                Dispatcher.BeginInvoke(() => MessageBox.Show(message));
            }
            else if (networkTypeEventArgs.HasInternet)
            {
                message = "The Internet connection type is: " + networkTypeEventArgs.Type.ToString();
            }
            else
            {
                message = "There is no Internet connection";
                Dispatcher.BeginInvoke(() => MessageBox.Show(message));
            }

            // Always dispatch on the UI thread
           
        }

        private void BtnDictionary_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(PageDictionary))
            {
                this.NavigationService.Navigate(new Uri(PageDictionary, UriKind.Relative));
            }
        }

  
     

        private void BtnTranslate_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(PageTranslate))
            {
                this.NavigationService.Navigate(new Uri(PageTranslate, UriKind.Relative));
            }
         
        }

        

        private void MyWord_Click(object sender, RoutedEventArgs e)
        {

            if (!String.IsNullOrWhiteSpace(PageMyword))
            {
                this.NavigationService.Navigate(new Uri(PageMyword, UriKind.Relative));
            }
            //String message="We will update this functionality in the future";
            //MessageBox.Show(message);
        }

        private void Grammar_Click(object sender, RoutedEventArgs e)
        {
            String message = "We will update this functionality in the future";
            MessageBox.Show(message);
        }

        private void Btn_Rate_Click(object sender, RoutedEventArgs e)
        {
         
            
         
       
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            
           marketplaceReviewTask.Show();
           // RateMyApp.WP.Core.RateMyAppControl control = new RateMyApp.WP.Core.RateMyAppControl();

            // debug = true + 'Debug build' => 
            // Review will apear after 10 sec. regardless of previous state 
            // Callback: this metod is called everytime the Control changes "review" state.
            // Exposure Pattern: in this sample the Rate control will be shown 
            // the first 6 times that app has been used for more than 10 sec. 
            // or until the review has been completed. 
            //control.Start(true, new RateMyApp.Core.Rate.RateControlLanguagePack
            //{
            //    RateMyAppCancelText = "No",
            //    RateMyAppFeedbackEmail = "trannguyenlong1991@gmail.com",
            //    RateMyAppHeader = "Enjoying Keep Calm?",
            //    RateMyAppText = "We'd love you to rate our app 5 stars",
            //    RateMyAppYesText = "yes"
            //}, null, new int[] { 1, 2, 3, 4, 5, 6 });
        

        }
   
       

     
    }
}