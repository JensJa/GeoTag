using System.ComponentModel;

namespace GeoTag
{
    public enum MapProvider
    {
        GoogleMaps,
        OpenStreetMap
    }

    public partial class mapCtrl : UserControl
    {
        public mapCtrl()
        {
            InitializeComponent();

            initBrowser();

        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            
        }





        #region Properties

        [Browsable(true), DefaultValue("https://www.openstreetmap.org"), Description("URL displayed now"), Category("GPS"), DisplayName("act URL")]
        public String acturl { get; private set; }

        [Browsable(true), DefaultValue(true), Description("Show the tag symbol"), Category("GPS"), DisplayName("show symbol")]
        public bool ShowSymbol { get; set; } = true;

        [Browsable(true), DefaultValue(true), Description("Show the tag symbol"), Category("GPS"), DisplayName("show symbol")]
        public MapProvider activeMapProvider { get; set; } = MapProvider.OpenStreetMap;

        [Browsable(true), DefaultValue(0), Description("get Latitude"), Category("GPS"), DisplayName("get the longitude")]
        public double latitude { get; set; } = 0;
        [Browsable(true), DefaultValue(0), Description("get Longitude"), Category("GPS"), DisplayName("get the latitude")]
        public double longitude { get; set; } = 0;

        #endregion

        private async Task initialized()
        {
            await webView21.EnsureCoreWebView2Async(null);
        }
        //52.074865, 12.951682

        public async void initBrowser()

        {
            await initialized();
            navigateToUrl();
        }

        public void navigateToUrl()
        {
            acturl = getUrl(activeMapProvider, latitude.ToString().Replace(',', '.'), longitude.ToString().Replace(',', '.'), 19, true);

            Task.Delay(500);
            
            webView21.CoreWebView2.Navigate(acturl);
        }

        //public event EventHandler<DataEventArgs>? DataProcessed; 
        //protected virtual void OnDataProcessed(string msg) 
        //{ DataProcessed?.Invoke(this, new DataEventArgs(msg)); }
        
        ////// The event. Note that by using the generic EventHandler<T> event type
        ////// we do not need to declare a separate delegate type.
        public event EventHandler<GeoTagEventArgument>? CoordinateChange;

        public void OnCoordinateChange(GeoTagEventArgument e)
        {
            // e = new GeoTagEventArgument(latitude,longitude, acturl);

            // Call the base class event invocation method.
            // base.OnShapeChanged(e);
        }

        private void webView21_SourceChanged(object sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs e)
        {
            string[] parameters;
            acturl = webView21.Source.ToString();

            if (acturl.ToLower().StartsWith("https://www.google.com/maps"))

            {

                string[] urls = acturl.Split('/');
                if (urls[urls.Length - 1].Contains("data"))
                {
                    parameters = urls[urls.Length - 2].Split(",");
                }

                else
                {
                    parameters = urls[urls.Length - 1].Split(",");
                }
                latitude = Double.Parse(parameters[0]);
                longitude = Double.Parse(parameters[1]);
            }

            else if (acturl.ToLower().StartsWith("https://www.openstreetmap.org"))

            {
                string[] urls = acturl.ToLower().Split('=');
                parameters = urls[urls.Length - 1].Split("/");
                latitude = Double.Parse(parameters[1].Replace('.', ','));
                longitude = Double.Parse(parameters[2].Replace('.', ','));
            }

            else

            {
                latitude = 0;
                longitude = 0;
            }

          //  CoordinateChange(this, new GeoTagEventArgument(latitude, longitude, acturl));
        }


        public string getUrl(MapProvider provider, string latitude, string longitude, int zoom = 19, bool baseurl = false)
        {

            switch (provider)
            {
                case MapProvider.GoogleMaps:
                    return getGoogleMapsUrl(latitude, longitude, zoom,baseurl);
                case MapProvider.OpenStreetMap:
                    return getOsmUrl(latitude, longitude, zoom,baseurl);
                default:
                    return "";
            }


        }

        private static string getGoogleMapsUrl(string latitude, string longitude, int zoom = 21, bool baseUrl = false)
        //https://www.google.com/maps/@52.1229021,12.9080458,24212m/data=!3m1!1e3?authuser=0&entry=ttu&g_ep=EgoyMDI2MDIxOC4wIKXMDSoASAFQAw%3D%3D
        {// Zoom begrenzen (Google Maps max. 21)
            if (baseUrl) { return "https://www.google.com/maps"; }
            if (zoom > 21) zoom = 21; if (zoom < 0) zoom = 0;

            string url = $"https://www.google.com/maps/@{latitude},{longitude},{zoom}z";
            return url;
        }

        private static string getOsmUrl(string latitude, string longitude, int zoom = 21, bool baseUrl = false)
        //https://www.openstreetmap.org/#map=19/56.003528/8.129462
        {
            if (baseUrl)
            { return "https://www.openstreetmap.org"; }
            // Zoom begrenzen (Gsm max. 19)
            if (zoom > 19) zoom = 19; if (zoom < 0) zoom = 0;

            string url = $"https://www.openstreetmap.org/#map={zoom}/{latitude}/{longitude}";
            return url;
        }
    
 
    }
}
