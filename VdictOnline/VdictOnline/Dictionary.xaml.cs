using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using DotNetApp.Utilities;
using System.ComponentModel;
using Microsoft.Phone.BackgroundAudio;
using System.Windows.Threading;
using VdictOnline.Resources;
using System.Threading.Tasks;
using VdictOnline.Model;
using VdictOnline.ViewModel;
namespace VdictOnline
{
    public partial class Dictionary : PhoneApplicationPage
    {
       
        Uri link;
        private ObservableCollection<MyListLanguage> myLanguage;
        private GetDataOnline _currentData = new GetDataOnline();
        private ObservableCollection<MyListViewModel> myVM;
        private BackgroundWorker bw = new BackgroundWorker();
        bool newsearch = false,complete=true,havenetwork=true,textchange=true,saved=false;
        String key="";
        string meanword;
        public Dictionary()
        {
            InitializeComponent();
            AddDataLanguage();
            ApplicationBar.IsVisible = false;
        }
        private void BindData()
        {
            listData.ItemsSource = myVM;
            
        }
        private void AddDataLanguage()
        {
            myLanguage = new ObservableCollection<MyListLanguage>
            {
                new MyListLanguage
                { Language1="English-VietNam", LinkIcon="EtoV.png" },
              new MyListLanguage
                { Language1="VietNam-English",LinkIcon="VtoE.jpg" },
                 new MyListLanguage
                { Language1="France-VietNam",LinkIcon="FtoV.jpg" },
                 new MyListLanguage
                { Language1="VietNam-France",LinkIcon="VtoF.jpg" },
                 new MyListLanguage
                { Language1="English-English",LinkIcon="EtoE.jpg" }

            };
       
            ListLanguage.ItemsSource = myLanguage;

        }
     
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (complete == true)
            {
                ApplicationBar.IsVisible = false;
                saved = false;
                textchange = false;
                complete = false;
                 key = Textbox_Search.Text;
                if (!key.Trim().Equals(""))
                {
                    Textbox_Pronounciation.Text = "";
                    this.Button_Listens.IsEnabled = false;
                    myVM = new ObservableCollection<MyListViewModel>();
                    BindData();
                    //  if (CheckBox_EV.IsChecked == true) Loading_Dictionary("http://vdict.com/{0},1,0,0.html", key);
                    // if (CheckBox_VE.IsChecked == true) Loading_Dictionary("http://vdict.com/{0},2,0,0.html", key);

                    string link = ChoseLanguage();
                    Loading_Dictionary("http://vdict.com/{0}" + link, key);

                    newsearch = true;
                    ProgressBar2.IsIndeterminate = true;
                    ProgressBar2.Visibility = Visibility.Visible;
                    
                }
            }
     
        }

        private void RequestStreamReady(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        private string ChoseLanguage()
        {
            string link = null;
            int d = ListLanguage.SelectedIndex;
            switch (d)
            {
                case 0: link = ",1,0,0.html"; break;//EV
                case 1: link = ",2,0,0.html"; break;//VE
                case 2: link = ",5,0,0.html"; break;//FV
                case 3: link = ",4,0,0.html"; break;//VF
                case 4: link = ",7,0,0.html"; break;//EE
                default:link = ",1,0,0.html"; break;//EV
            }
            return link;
        }


   
        public Task<string> Loading_Dictionary(String Linkrequest, String Keyword)
        {
           
            System.Uri targetUri = MakeUrirequest_Dictionary(Linkrequest, Keyword);
          
            
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
           client.DownloadStringAsync(targetUri);
           return null;
        }

        private Uri MakeUrirequest_Dictionary(string linkrequest, string keyword)
        {

            System.Uri targetUri = new System.Uri(string.Format(linkrequest, keyword));
            return targetUri;
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            HtmlDocument Document = new HtmlDocument();

            try
            {

                Document.LoadHtml(e.Result);
                if (CheckHasResult(Document)==true)
                #region
                {
                    meanword = "";
                    HtmlNode divNode_Pronunce = Document.DocumentNode.SelectSingleNode("//div[@class='pronounce']");
                    Textbox_Pronounciation.Text = divNode_Pronunce.InnerText;
                    HtmlNodeCollection scriptsnode = Document.DocumentNode.SelectNodes("//script[@type='text/javascript'] ");
                    HtmlNodeCollection categoriesnode = Document.DocumentNode.SelectNodes("//div[@class='phanloai']|//ul[@class='list1']/li/b|//ul[@class='list2']");
                    List<HtmlNode> ListNode = new List<HtmlNode>();
                    foreach (HtmlNode node in scriptsnode)
                    {
                        ListNode.Add(node);

                    }
                    foreach (HtmlNode Node_LinkVoice in ListNode)
                    {
                        //HtmlNode Node_LinkVoice = ListNode.ElementAt(8);

                        string pattern = " |,|\" ";
                        Regex myRegex = new Regex(pattern);
                        string[] sKetQua = myRegex.Split(Node_LinkVoice.InnerText);

                        foreach (string subString in sKetQua)
                        {
                            if (Regex.IsMatch(subString, @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?(.mp3)"))
                            {
                                link = new Uri(subString.Substring(1, subString.Length - 2));
                                this.Button_Listens.IsEnabled = true;
                            }

                        }
                    }
                    List<MyListViewModel> listData = new List<MyListViewModel>();

                    MyListViewModel items = new MyListViewModel();
                    int d = 0;
                    int total_data = 0;
                    //check......................
                    //..................
                    //.............


                    if (categoriesnode.Count == 0)
                    {
                        items = new MyListViewModel();
                        items.Field1 = "Not Found";
                        myVM.Add(items); BindData();
                        return;
                    }
                    foreach (HtmlNode node in categoriesnode)
                    {
                        d++;
                       
                        if (checkNode(Document, node))
                        {
                            if (items.Field1 != null)
                            {
                               
                                myVM.Add(items);
                                total_data++;
                                BindData();
                                items = new MyListViewModel();
                            }
                            items.Field1 = node.InnerText;
                        }
                        else
                        {
                            if (checkNodeList1(Document, node)) { items.Field2 += "\n\t * " + node.InnerText; total_data++; }
                            if (checkNodeList2(Document, node)) { items.Field3 += "\n\t\t + " + Getstring(node); total_data++; }
                        }
                 
                        if (categoriesnode.Count == d) { myVM.Add(items); BindData(); }
                    }

                }
                #endregion
                else
                {
                    MyListViewModel items = new MyListViewModel();
                    items.Field1 = "Not Found";
                    myVM.Add(items); BindData();
                }
            }
            catch (Exception ex)
            {
                {
                    MyListViewModel items = new MyListViewModel();
                    items.Field1 = "Please check internet or server error";
                    havenetwork = false;
                    myVM.Add(items); BindData();
                }
                Console.Write(ex.Message);
            }
        
            ProgressBar2.IsIndeterminate = false;
            ProgressBar2.Visibility = Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
            complete = true;
        }

        private string Getstring(HtmlNode node)
        {
            try
            {
                String data = "";

                HtmlNode nodespan = node.SelectSingleNode("//span[@class='example-original']");
                HtmlNode node2 = node;
                // HtmlNode nodebr = node.SelectSingleNode("//ul/li[(preceding::br)]");
                if (nodespan != null) data += nodespan.InnerText;
                if (node2.HasChildNodes) node2.SelectSingleNode("//span[@class='example-original']").Remove();
                data += " " + node2.InnerText;
                return data;
            }
            catch (Exception ads)
                {
                 return node.InnerText;
                }
        }

        private bool CheckHasResult(HtmlDocument Document)
        {
            Boolean returncheck = true;
           
                HtmlNodeCollection nodepronounce = Document.DocumentNode.SelectNodes("//div[@id='main-contents']");
                if (nodepronounce!=null) returncheck = false;
          
            return returncheck;
        }

        private bool checkNode(HtmlDocument Document, HtmlNode node)
        {
            Boolean returncheck=false;
            HtmlNodeCollection node_0 = Document.DocumentNode.SelectNodes("//div[@class='phanloai']");
            if (node_0!=null)
            foreach (HtmlNode nodes in node_0)
            {
                if (node.InnerText.Equals(nodes.InnerText)) { returncheck = true; break; }
            }
            return returncheck;
        }
        private bool checkNodeList1(HtmlDocument Document, HtmlNode node)
        {
            Boolean returncheck = false;
            HtmlNodeCollection node_1 = Document.DocumentNode.SelectNodes("//ul[@class='list1']/li/b");
            if (node_1!= null)
            foreach (HtmlNode nodes in node_1)
            {
                if (node.InnerText.Equals(nodes.InnerText)) { returncheck = true; break; }
            }
            return returncheck;
        }
        private bool checkNodeList2(HtmlDocument Document, HtmlNode node)
        {
            Boolean returncheck = false;
            HtmlNodeCollection node_2 = Document.DocumentNode.SelectNodes("//ul[@class='list2']");
            if(node_2!=null)
            foreach (HtmlNode nodes in node_2)
            {
              
                if (node.InnerText.Equals(nodes.InnerText)) {
                   
                 
                    returncheck = true; break; }
            }
            return returncheck;
        }
        private void BtnVoice_Click(object sender, RoutedEventArgs e)
        {
            if (newsearch == true)
            {
                try
                {
                    if (link.ToString() != "")
                    {
                        media.Source = link;
                        //BackgroundAudioPlayer.Instance.PlayStateChanged += new EventHandler(Instance_PlayStateChanged);
                        media.Play();
                    }
                }
                catch (NullReferenceException nullex)
                {
                    return;
                }
               
            }
        }
        public class MyListViewModel
        {
            public string Field1 { get; set; }

            public string Field2 { get; set; }
            public string Field3 { get; set; }

        }
        public class Data
        {
            public string LinkVoice { get; set; }
            public string Pronunce { get; set; }
            public string Keyword { get; set; }
            public string Linkrequest { get; set; }

        }

        private void saveWord_Click(object sender, EventArgs e)
        {
            if (this.Textbox_Search.Text.Length > 0 && (havenetwork==true)&& saved==false)
            {
                saved = true;
                if (myVM != null)
                {
                    if (myVM.Count >= 3)
                    {

                        ToDoItem newToDoItem = new ToDoItem
                        {
                            ItemName = Textbox_Search.Text,
                            Category = (ToDoCategory)TypeLanguage(ListLanguage.SelectedIndex),
                            Pronounce = Textbox_Pronounciation.Text,
                            MeanWord1 = myVM[0].Field1,
                            MeanWord2 = "\t" + myVM[0].Field2,
                            MeanWord3 = "\t\t" + myVM[0].Field3,
                            MeanWord4 = myVM[1].Field1,
                            MeanWord5 = "\t" + myVM[1].Field2,
                            MeanWord6 = "\t\t" + myVM[1].Field3,
                            MeanWord7 = myVM[2].Field1,
                            MeanWord8 = "\t" + myVM[2].Field2,
                            MeanWord9 = "\t\t" + myVM[2].Field3

                        };
                        App.ViewModel.AddToDoItem(newToDoItem);
                    }
                    if (myVM.Count == 2)
                    {

                        ToDoItem newToDoItem = new ToDoItem
                        {
                            ItemName = Textbox_Search.Text,
                            Category = (ToDoCategory)TypeLanguage(ListLanguage.SelectedIndex),
                            Pronounce = Textbox_Pronounciation.Text,
                            MeanWord1 = myVM[0].Field1,
                            MeanWord2 = "\t" + myVM[0].Field2 ,
                            MeanWord3 = "\t\t" + myVM[0].Field3,
                            MeanWord4 = myVM[1].Field1,
                            MeanWord5 = "\t" + myVM[1].Field2,
                            MeanWord6 = "\t\t" + myVM[1].Field3

                        };
                        App.ViewModel.AddToDoItem(newToDoItem);
                    }
                    if (myVM.Count == 1)
                    {

                        ToDoItem newToDoItem = new ToDoItem
                        {
                            ItemName = Textbox_Search.Text,
                            Category = (ToDoCategory)TypeLanguage(ListLanguage.SelectedIndex),
                            Pronounce = Textbox_Pronounciation.Text,
                            MeanWord1 = myVM[0].Field1,
                            MeanWord2 = "\t" + myVM[0].Field2,
                            MeanWord3 = "\t\t" + myVM[0].Field3
                        };
                        App.ViewModel.AddToDoItem(newToDoItem);
                    }
                }
                //// Return to the main page.
                //if (NavigationService.CanGoBack)
                //{
                //    NavigationService.GoBack();
                //}
            }
        }

        private string ConvertData(ObservableCollection<MyListViewModel> myVM)
        {
            string data = "";
            foreach (MyListViewModel items in myVM)
            {
              
                    data +="\n"+ items.Field1 ;
                if(data.Length+items.Field2.Length<3000)
                   data+= "\n\t" + items.Field2;
                if (data.Length + items.Field3.Length < 3000)
                    data += "\n\t\t" + items.Field3;
               
            }
            return data;
        }

        private ToDoCategory TypeLanguage(int p)
        {
            ToDoViewModel newobject = App.ViewModel;
           // newobject.LoadCollectionsFromDatabase();
            List<ToDoCategory> list = newobject.CategoriesList;
            ToDoCategory newToDoCategory;

            switch (p)
            {
                case 0: newToDoCategory= list[0]; break;
                case 1: newToDoCategory = list[1]; break;
                case 2: newToDoCategory = list[2]; break;
                case 3: newToDoCategory = list[1]; break;
                case 4: newToDoCategory = list[0]; break;
                default: newToDoCategory = list[0]; break; 
            };
            return newToDoCategory;
        }

        private void goWord_Click(object sender, EventArgs e)
        {
            String PageMyword = "/Myword.xaml";
            if (!String.IsNullOrWhiteSpace(PageMyword))
            {
                this.NavigationService.Navigate(new Uri(PageMyword, UriKind.Relative));
            }
        }

        private void Textbox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (!key.Equals(this.Textbox_Search.Text)) textchange = true;
            else textchange = false;
        }



       
    }
}