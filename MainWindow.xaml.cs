using System;
using System.Collections.Generic;
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
using System.Net;
using System.Net.Sockets;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int port=54000;
            if (int.TryParse(port_num.Text, out port))
            {

            }
            else
            {
                var result = MessageBox.Show("是否用預設port 54000?", "格式錯誤", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            UdpClient server = new UdpClient(port); // 使用指定的端口號
            IPEndPoint remoteEP;
            if (ip_cb.IsChecked == true)
            {
                IPAddress iP;
                if (IPAddress.TryParse(port_num.Text, out iP))
                {
                    remoteEP = new IPEndPoint(iP, 0);
                }
                else
                {
                    MessageBox.Show("請輸入正確的IP位置!!!", "格式錯誤");
                    return;
                }
                
            }
            else
            {
                remoteEP = new IPEndPoint(IPAddress.Any, 0);
            }
            
            try
            {
                chat.Text+="等待客戶端連接...\n";
                while (true)
                {
                    byte[] receiveBytes = server.Receive(ref remoteEP);
                    string receivedData = Encoding.UTF8.GetString(receiveBytes);
                    chat.Text += $"收到來自 {remoteEP.Address.ToString()}:{remoteEP.Port} 的消息：{receivedData}";
                    string responseData = "收到消息！";
                    byte[] sendBytes = Encoding.UTF8.GetBytes(responseData);
                    server.Send(sendBytes, sendBytes.Length, remoteEP);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                server.Close();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ip_cb.IsChecked==true)
            {
                ip_textbox.IsEnabled = true;
            }
            else
            {
                ip_textbox.IsEnabled = false;
            }
        }
    }
}
