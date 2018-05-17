using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WebViewIssuesSample.WebView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebViewPage : Page
    {
        public WebViewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void OnWebViewPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if (sender is UIElement webContent)
            {
                e.GetIntermediatePoints(webContent);
                if (!e.GetCurrentPoint(this).Properties.IsHorizontalMouseWheel)
                {
                    var verticalOffset = RootScrollViewer.VerticalOffset - e.GetCurrentPoint(webContent).Properties.MouseWheelDelta;
                    RootScrollViewer.ChangeView(RootScrollViewer.HorizontalOffset, verticalOffset, RootScrollViewer.ZoomFactor);
                }
            }
        }

        private void OnWebContentScriptNotify(object sender, NotifyEventArgs e)
        {
            try
            {
                var args = e.Value;
                var jObj = JObject.Parse(args);
                var action = (string)jObj["Action"];
                var param = (string)jObj["Param"];
                switch (action)
                {
                    case "SetSize":
                        ResizeContent(param);
                        break;
                }
            }
            catch { }
        }

        private void ResizeContent(string param)
        {
            var size = JObject.Parse(param);
            var width = (double)size["Wt"];
            var height = (double)size["Ht"];
            ContentWebView.Height = (height < 1400) ? 1400 : height;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ErrorBlock.Visibility = Visibility.Collapsed;
            var contentUri = ContentWebView.BuildLocalStreamUri("WebContent", "PrimaryPage.html");
            var contentResolver = new WebContentResolver();
            ContentWebView.NavigateToLocalStreamUri(contentUri, contentResolver);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var contentUri = ContentWebView.BuildLocalStreamUri("WebContent", "SecondaryPage.html");
            var contentResolver = new WebContentResolver();
            ContentWebView.NavigateToLocalStreamUri(contentUri, contentResolver);
        }

        private void ContentWebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            ErrorBlock.Text = $"Failed to load page {e.Uri} from another project. {e.WebErrorStatus} ";
            ErrorBlock.Visibility = Visibility.Visible;
        }
    }
}
