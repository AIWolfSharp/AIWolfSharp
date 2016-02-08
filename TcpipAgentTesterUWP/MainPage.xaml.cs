using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace TcpipAgentTesterUWP
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int port = int.Parse(portTextBox.Text);
            string host = hostTextBox.Text;
            IPlayer player = null;
            try
            {
                player = (IPlayer)Activator.CreateInstance(typeof(SimplePlayer));
            }
            catch (Exception)
            {
                throw;
            }
            new TcpipClient(host, port, null).Connect(player);
            connectButton.Content = "Connected";
            connectButton.IsEnabled = false;
        }

    }
}
