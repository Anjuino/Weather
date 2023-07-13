using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Xml;

namespace GetHumTemp
{
   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public static string z;
        private string Hum(string z)
        {
            WebRequest TwrGETURL = WebRequest.Create("https://api.thingspeak.com/channels/2204972/fields/2/last.xml?timezone=Asia/Krasnoyarsk");
            WebRequest HwrGETURL = WebRequest.Create("https://api.thingspeak.com/channels/2204972/fields/1/last.xml?timezone=Asia/Krasnoyarsk");
            WebRequest PwrGETURL = WebRequest.Create("https://api.thingspeak.com/channels/2204972/fields/3/last.xml?timezone=Asia/Krasnoyarsk");
            Stream TobjStream = TwrGETURL.GetResponse().GetResponseStream();
            Stream HobjStream = HwrGETURL.GetResponse().GetResponseStream();
            Stream PobjStream = PwrGETURL.GetResponse().GetResponseStream();
            StreamReader TobjReader = new StreamReader(TobjStream);
            StreamReader HobjReader = new StreamReader(HobjStream);
            StreamReader PobjReader = new StreamReader(PobjStream);
            string xml1 = TobjReader.ReadToEnd();
            string xml2 = HobjReader.ReadToEnd();
            string xml3 = PobjReader.ReadToEnd();
            XmlDocument xmlDocT = new XmlDocument();
            XmlDocument xmlDocH = new XmlDocument();
            XmlDocument xmlDocP = new XmlDocument();
            xmlDocT.LoadXml(xml1);
            xmlDocH.LoadXml(xml2);
            xmlDocP.LoadXml(xml3);
            XmlNodeList elemlist1 = xmlDocT.GetElementsByTagName("field2");
            XmlNodeList elemlist2 = xmlDocT.GetElementsByTagName("created-at");
            XmlNodeList elemlist3 = xmlDocH.GetElementsByTagName("field1");
            XmlNodeList elemlist4 = xmlDocP.GetElementsByTagName("field3");
            string resT = elemlist1[0].InnerXml;
            string ti = elemlist2[0].InnerXml;
            string resH = elemlist3[0].InnerXml;
            string resP = elemlist4[0].InnerXml;
            string result ="Время записи:"+ " " + ti + "\n" + "Температура:" + " " + resT.Trim()+ "С" + "\n"  + "Влажность:" + " " + resH.Trim()+"%" + "\n" + "Давление:" + " " + resP.Trim() + "мм рт. ст.";
            return result;
        }
        private string res;

        private string Status(string res)
        {
            WebRequest TwrGETURL = WebRequest.Create("https://api.thingspeak.com/channels/2204972/fields/2/last.xml?timezone=Asia/Krasnoyarsk");
            Stream TobjStream = TwrGETURL.GetResponse().GetResponseStream();
            StreamReader TobjReader = new StreamReader(TobjStream);
            string xml1 = TobjReader.ReadToEnd();
            XmlDocument xmlDocT = new XmlDocument();
            xmlDocT.LoadXml(xml1);
            XmlNodeList elemlist2 = xmlDocT.GetElementsByTagName("created-at");
            string ti = elemlist2[0].InnerXml;
            
            return ti;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("Отсутствует или ограниченно физическое подключение к сети\nПроверьте настройки вашего сетевого подключения");
                return;
            }
            else {
                MessageBox.Show(Hum(z));
            }
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            DateTime thisDay = DateTime.Now;

            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("Отсутствует или ограниченно физическое подключение к сети\nПроверьте настройки вашего сетевого подключения");
            }
            else if (Status(res).Substring(0, 13).Equals(thisDay.ToString("yyyy'-'MM'-'dd'T'HH")))
            {
                MessageBox.Show("Датчик включен =)");
            }
            else
            {
                MessageBox.Show("Датчик выключен =(\nВремя последней записи:" + " " + Status(res));
            }
        }
    }
}
