using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text;
using HtmlAgilityPack;
using Windows.Phone.Speech.Synthesis;
using System.ServiceModel.Web;
namespace VdictOnline
{
    public partial class Translate : PhoneApplicationPage
    {
        String[] Country = { "English","French","Italia","Viet Nam"};
        private static string strTextToTranslate = "";
        String language_source = "en", language_des = "vi";
        string clientID;
        string clientSecret;
       // String LinkRequest;
        public Translate()
        {
            InitializeComponent();
            this.lpkCountrySource.ItemsSource = Country;
            this.lpkCountryDes.ItemsSource = Country;
        }
        private Uri MakeUrirequest_Dictionary(string linkrequest, string keyword)
        {

            System.Uri targetUri = new System.Uri(linkrequest+keyword);
            return targetUri;
        }
        public void Loading_Translate(String Linkrequest, String Keyword)
        {

            System.Uri targetUri = MakeUrirequest_Dictionary(Linkrequest, Keyword);
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(targetUri);

        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            HtmlDocument Document = new HtmlDocument();
            try {
                Document.LoadHtml(e.Result);
                String c=Document.ToString();
                HtmlNode divNode_Output = Document.DocumentNode.SelectSingleNode("//div[@class='t0']");
                TextBox_Output.Text = divNode_Output.InnerText.ToString();
            }
            catch (Exception exx)
            {
            
            }
        }
        private string convert_UTF8(String input)
        {
            string convertstring = string.Empty;
            byte[] utf8_Bytes = new byte[input.Length];
            for (int i = 0; i < input.Length; ++i)
            {
                utf8_Bytes[i] = (byte)input[i];
            }
            convertstring = Encoding.UTF8.GetString(utf8_Bytes, 0, utf8_Bytes.Length);
          
            return convertstring;
        }
        private void Click_Btn_Translate(object sender, RoutedEventArgs e)
        {
           
           
            // Initialize the strTextToTranslate here, while we're on the UI thread
            strTextToTranslate = TextBox_Input.Text;
            if (!(strTextToTranslate.Length == 0))
            {
                Check_Language();
                TextBox_Output.Text = "";
                customIndeterminateProgressBar.Visibility = Visibility.Visible;
                customIndeterminateProgressBar.IsIndeterminate = true;
                // STEP 1: Create the request for the OAuth service that will
                // get us our access tokens.
                String strTranslatorAccessURI =
                      "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
                System.Net.WebRequest req = System.Net.WebRequest.Create(strTranslatorAccessURI);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                // Important to note -- the call back here isn't that the request was processed by the server
                // but just that the request is ready for you to do stuff to it (like writing the details)
                // and then post it to the server.
                IAsyncResult writeRequestStreamCallback =
                  (IAsyncResult)req.BeginGetRequestStream(new AsyncCallback(RequestStreamReady), req);
            }

        }

        private void RequestStreamReady(IAsyncResult ar)
        {

            // STEP 2: The request stream is ready. Write the request into the POST stream
            GetClientInfo(language_source);
           
            String strRequestDetails = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientID), HttpUtility.UrlEncode(clientSecret));
           
            // note, this isn't a new request -- the original was passed to beginrequeststream, so we're pulling a reference to it back out. It's the same request

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)ar.AsyncState;
            // now that we have the working request, write the request details into it
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strRequestDetails);
            System.IO.Stream postStream = request.EndGetRequestStream(ar);
            postStream.Write(bytes, 0, bytes.Length);
            postStream.Close();
            // now that the request is good to go, let's post it to the server
            // and get the response. When done, the async callback will call the
            // GetResponseCallback function
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);

        }

        private void GetClientInfo(String p)
        {
            switch (p)
            {

                case "vi":
                    clientID = "VidictOnline2";
                    clientSecret = "+lnaqIUo7uMfVEJtMVBKSNDpiMvMGLg3KbrVokBTbyg=";
                  //  clientID = "SearchOnline2";
                //    clientSecret = "EI08+HKrXxoiGd97J+7D4tqKjcj0B4bCORmhynsBwNM=";
                    break;
                case "en":
                    clientID = "Vdictonline1";
                    clientSecret = "r3trqAvWdIjGJQvnQNI0xQB0ikL2V2baCpHFi1YCph8=";
                    break;
                case "fr":
                    clientID = "Vdictonline";
             clientSecret = "hG6oTcJ87M8R6zEFv5I15/Hj2LmflYP8CnJ+klZ27YQ=";
                    break;
                case "it":
                     clientID = "Vdictonline";
             clientSecret = "hG6oTcJ87M8R6zEFv5I15/Hj2LmflYP8CnJ+klZ27YQ=";
                    break;
                default:
                     clientID = "Vdictonline";
             clientSecret = "hG6oTcJ87M8R6zEFv5I15/Hj2LmflYP8CnJ+klZ27YQ=";
                    break;
            }
        }

        private void GetResponseCallback(IAsyncResult ar)
        {
            try
            {
                // STEP 3: Process the response callback to get the token
                // we'll then use that token to call the translator service
                // Pull the request out of the IAsynch result
                HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
                // The request now has the response details in it (because we've called back having gotten the response
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
                // Using JSON we'll pull the response details out, and load it into an AdmAccess token class (defined earlier in this module)
                // IMPORTANT (and vague) -- despite the name, in Windows Phone, this is in the System.ServiceModel.Web library,
                // and not the System.Runtime.Serialization one -- so you will need to have a reference to System.ServiceModel.Web

                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new
                    System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(AdmAccessToken));
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(response.GetResponseStream());
               
                string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Net.HttpUtility.UrlEncode(strTextToTranslate) + "&from" + language_source + "=en&to=" + language_des;
                System.Net.WebRequest translationWebRequest = System.Net.HttpWebRequest.Create(uri);
                // The authorization header needs to be "Bearer" + " " + the access token
                string headerValue = "Bearer " + token.access_token;
                translationWebRequest.Headers["Authorization"] = headerValue;
                // And now we call the service. When the translation is complete, we'll get the callback
                IAsyncResult writeRequestStreamCallback = (IAsyncResult)translationWebRequest.BeginGetResponse(new AsyncCallback(translationReady), translationWebRequest);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            
                Deployment.Current.Dispatcher.BeginInvoke(() => TextBox_Output.Text = "Not Found.Please check internet");
                // Do nothing
            }
        }

        private void Check_Language()
        {

           language_des = String.Format("{0}", lpkCountryDes.SelectedItem);
           language_source = String.Format("{0}", lpkCountrySource.SelectedItem);

          

           language_des =SC_Language( language_des);
            language_source = SC_Language(language_source);
        }

        private string SC_Language(string p)
        {
            String language = null;
            switch (p)
            {
                   
                case "Viet Nam": 
                    language = "vi";
                   
                    break;
                case "English": 
                    language = "en";
                    break;
                case "French":
                    language = "fr";
                    break;
                case "Italia": 
                    language = "it"; 
                    break;
                default:
                    language = "en"; 
                    break;
            }
            return language;
        }

        private void translationReady(IAsyncResult ar)
        {
            try
            {
                // STEP 4: Process the translation
                // Get the request details
                HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
                // Get the response details
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
                // Read the contents of the response into a string
                System.IO.Stream streamResponse = response.GetResponseStream();
                System.IO.StreamReader streamRead = new System.IO.StreamReader(streamResponse);
                string responseString = streamRead.ReadToEnd();
                // Translator returns XML, so load it into an XDocument
                // Note -- you need to add a reference to the System.Linq.XML namespace
                System.Xml.Linq.XDocument xTranslation =
                  System.Xml.Linq.XDocument.Parse(responseString);
                string strTest = xTranslation.Root.FirstNode.ToString();
                // We're not on the UI thread, so use the dispatcher to update the UI
                Deployment.Current.Dispatcher.BeginInvoke(() => TextBox_Output.Text = strTest);
                Deployment.Current.Dispatcher.BeginInvoke(() => customIndeterminateProgressBar.IsIndeterminate = false);
                Deployment.Current.Dispatcher.BeginInvoke(() => customIndeterminateProgressBar.Visibility = Visibility.Collapsed);
            }
            catch (Exception exxx)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => TextBox_Output.Text = "Not Found.Please check internet");
            }
        }

        private void CheckBox_EV_Checked(object sender, RoutedEventArgs e)
        {
            language_des = "vi";
            language_source = "en";
        }

        private void CheckBox_VE_Checked(object sender, RoutedEventArgs e)
        {
            language_des = "en";
            language_source = "vi";
        }
    }
}