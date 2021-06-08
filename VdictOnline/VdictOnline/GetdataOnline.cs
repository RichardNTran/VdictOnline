using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VdictOnline
{
    class GetDataOnline
    {
        HtmlDocument Document;
        public string Keyword { get; set; }
        public string Linkrequest { get; set; }
        public string Linkvoice { get; set; }
        public string Pronunce { get; set; }

        public void Loading_Dictionary()
        {

            System.Uri targetUri = MakeUrirequest_Dictionary(Linkrequest, Keyword);
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(targetUri);

        }

        private Uri MakeUrirequest_Dictionary(string linkrequest, string keyword)
        {

            System.Uri targetUri = new System.Uri(string.Format(linkrequest, keyword));
            return targetUri;
        }

        private  void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Document = new HtmlDocument();
            try
            {
                 Document.LoadHtml(e.Result);
                HtmlNode divNode_Pronunce = Document.DocumentNode.SelectSingleNode("//div[@class='pronounce']");
                Pronunce = divNode_Pronunce.InnerText;
                HtmlNodeCollection scriptsnode = Document.DocumentNode.SelectNodes("//script[@type='text/javascript'] ");
                List<HtmlNode> ListNode = new List<HtmlNode>();
                foreach (HtmlNode node in scriptsnode)
                {
                    ListNode.Add(node);

                }
                HtmlNode Node_LinkVoice = ListNode.ElementAt(7);

                string pattern = " |,|\" ";
                Regex myRegex = new Regex(pattern);
                string[] sKetQua = myRegex.Split(Node_LinkVoice.InnerText);

                foreach (string subString in sKetQua)
                {
                    if (Regex.IsMatch(subString, @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?(.mp3)"))
                    {

                        Linkvoice = subString.Substring(1, subString.Length - 2) + "\n";
                    }
                }
            }
            catch (Exception ex)
            {
                Linkvoice = ex.Message;
            }
        }
    }
}
