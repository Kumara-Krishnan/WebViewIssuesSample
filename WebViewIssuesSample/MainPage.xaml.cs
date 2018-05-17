using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WebViewIssuesSample.WebView;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebViewIssuesSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            App.ProtocolActivated += OnProtocolActivated;
        }

        private async void OnProtocolActivated(ProtocolActivatedEventArgs args)
        {
            string content = string.Empty;
            try
            {
                var uri = args.Uri.ToString();
                content = $"{uri} was clicked";
            }
            catch (UriFormatException)
            {
                content = "Acccessing the Uri in ProtocolActivatedEventArgs threw an UriFormatException";
            }
            finally
            {
                await new MessageDialog(content).ShowAsync();
            }
        }

        private void OnListViewItemClicked(object sender, ItemClickEventArgs e)
        {
            var listViewItem = e.ClickedItem as TextBlock;
            var tag = listViewItem.Tag.ToString();
            Type pageType = null;
            switch (tag)
            {
                case "LoadHTML":
                    pageType = typeof(WebViewPage);
                    break;
                case "LoadMailTo":
                    pageType = typeof(MailTo.MailTo);
                    break;
            }
            AppFrame.Navigate(pageType);
        }
    }
}
