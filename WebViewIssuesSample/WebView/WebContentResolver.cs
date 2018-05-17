using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web;

namespace WebViewIssuesSample.WebView
{
    public sealed class WebContentResolver : IUriToStreamResolver
    {
        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentException("Uri cannot be null");
            }

            string path = uri.AbsolutePath;
            return GetContent(path).AsAsyncOperation();
        }

        private async Task<IInputStream> GetContent(string uri)
        {
            try
            {
                Uri localUri = new Uri(@"ms-appx:///Web/" + uri);
                if (uri == "/PrimaryPage.html")
                {
                    localUri = new Uri(@"ms-appx:///WebView/" + uri);
                }
                else if (uri == "/SecondaryPage.html")
                {
                    localUri = new Uri(@"ms-appx:///Web/" + uri);
                }
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(localUri);

                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    return stream.GetInputStreamAt(0);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading Uri - {uri} in content Webview:");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

    }
}
