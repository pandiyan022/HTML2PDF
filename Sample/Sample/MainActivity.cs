using Android.App;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using Android.Print;
using android.print;

namespace Sample
{
    [Activity(Label = "Sample", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        private WebView wv;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
           // Button button = FindViewById/<Button>(Resource.Id.myButton);
            wv = (WebView)FindViewById<WebView>(Resource.Id.web_view);
            wv.SetWebViewClient(new HelloWebViewClient());
            //wv.LoadUrl("file:///android_asset/report.html");

            wv.LoadDataWithBaseURL("file:///android_asset/report.html", "", "text/html", "UTF-8", null);
        }


        public class HelloWebViewClient : WebViewClient
        {

            public override void OnPageFinished(WebView view, string url)
            {

                base.OnPageFinished(view, url);

                string jobName =  "Document";
                PrintAttributes attributes = new PrintAttributes.Builder()
                        .SetMediaSize(PrintAttributes.MediaSize.IsoA4)
                        .SetResolution(new PrintAttributes.Resolution("pdf", "pdf", 600, 600))
                        .SetMinMargins(PrintAttributes.Margins.NoMargins).Build();
                PdfPrint pdfPrint = new PdfPrint(attributes);
                pdfPrint.Print(view.CreatePrintDocumentAdapter(jobName));

            }
        }

    }
}

