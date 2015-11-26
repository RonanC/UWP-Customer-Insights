using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace customer_insights
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
       // private object searchQuery;

        public MainPage()
        {
            this.InitializeComponent();
            spJsonRes.Visibility = Visibility.Collapsed;
        }

        private void tbkSubHeading_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Boolean success = false;
            int searchQuery;
            try
            {
                searchQuery = Convert.ToInt32(tbxCustId.Text);
                success = true;

            }
            catch (Exception)
            {
                tbxCustId.Text = "Must be an Integer";
                searchQuery = 0;
            }

            if (success)
            {
                getUser(searchQuery);
            }
        }

        // code from
        //https://msdn.microsoft.com/en-us/library/windows/apps/mt187345.aspx
        async void getUser(int searchQuery)
        {
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;

            //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            //especially if the header value is coming from user input.
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            Uri requestUri = new Uri("http://178.62.9.141:5000/datathon/customer/" + searchQuery);

            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }

            // custom
            tbkJsonRes.Text = httpResponseBody;
        }

        private void cbShowJsonRes_Checked(object sender, RoutedEventArgs e)
        {
            spJsonRes.Visibility = Visibility.Visible;
        }

        private void cbShowJsonRes_Unchecked(object sender, RoutedEventArgs e)
        {
            spJsonRes.Visibility = Visibility.Collapsed;
        }
    }
}
