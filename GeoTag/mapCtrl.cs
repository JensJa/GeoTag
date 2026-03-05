using System.ComponentModel;
using System.Diagnostics;

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
        public MapProvider MapProvider { get; set; } = MapProvider.OpenStreetMap;

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
            acturl = getUrl(MapProvider, latitude.ToString().Replace(',', '.'), longitude.ToString().Replace(',', '.'), 19, latitude == 0 && longitude == 0);
                                  
            webView21.CoreWebView2.Navigate(acturl);
            Thread.Sleep(1000);
            showIcon();
        }

        #region events
        //public event EventHandler<DataEventArgs>? DataProcessed; 
        //protected virtual void OnDataProcessed(string msg) 
        //{ DataProcessed?.Invoke(this, new DataEventArgs(msg)); }

        ////// The event. Note that by using the generic EventHandler<T> event type
        ////// we do not need to declare a separate delegate type.
        public event EventHandler<GeoTagEventArgument>? CoordinateChange;

       

        private void webView21_SourceChanged(object sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs e)
        {
            Debug.WriteLine("Source changed: " + webView21.Source.ToString());
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
                latitude = Double.Parse(parameters[0].Replace("@","").Replace('.', ','));
                longitude = Double.Parse(parameters[1].Replace('.', ','));
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

            CoordinateChange(this, new GeoTagEventArgument(latitude, longitude, acturl));
        }

        #endregion region


        #region viewmap 
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

        #endregion

        #region Icon Draw


        public void  showIcon()
        { 
         switch (MapProvider)
            {
                case MapProvider.GoogleMaps:
                    drawIconGoogleMaps();
                    break;
                case MapProvider.OpenStreetMap:
                    drawIconOSM();
                    break;
                default:
                    break;
            }

        }

        public async Task drawIconGoogleMaps()
        {
            using (var ms = new MemoryStream())
            {
                Resource1.location.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                string base64Image = Convert.ToBase64String(ms.ToArray());

                string script = $@"
(function() {{

    // Overlay erzeugen
    var overlay = document.getElementById('myOverlay');
    if (!overlay) {{
        overlay = document.createElement('div');
        overlay.id = 'myOverlay';
        overlay.style.position = 'fixed';
        overlay.style.top = '0';
        overlay.style.left = '0';
        overlay.style.width = '100vw';
        overlay.style.height = '100vh';
        overlay.style.pointerEvents = 'none'; // wichtig!
        overlay.style.zIndex = '999999999';   // höher als alles von Google
        document.body.appendChild(overlay);
    }}

    // Icon erzeugen
    var old = document.getElementById('geoTagIcon');
    if (old) old.remove();

    var img = document.createElement('img');
    img.src = 'data:image/png;base64,{base64Image}';
    img.id = 'geoTagIcon';

    img.style.position = 'absolute';
    img.style.top = '50%';
    img.style.left = '50%';
    img.style.width = '40px';
    img.style.height = '58px';
    img.style.transform = 'translate(-50%, -100%)';
    img.style.zIndex = '1000000000';

    overlay.appendChild(img);



}})();

";

                await webView21.CoreWebView2.ExecuteScriptAsync(script);
            }
        }
        /// <summary>
        /// /////////////////////////////////////////
        /// </summary>
        /// <returns></returns>

        public async Task drawIconOSM()
        {
            using (var ms = new MemoryStream())
            {
                Resource1.location.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                string base64Image = Convert.ToBase64String(ms.ToArray());
                string top = "" + webView21.Height / 2;
                string left = "" + webView21.Width / 2;
                string script = $@"
(function() {{
    var mapDiv = document.getElementById('map');
    var img = document.createElement('img');
    img.src = 'data:image/png;base64,{base64Image}';
    img.id = 'geoTagIcon';

    img.style.position = 'absolute';
    img.style.width = '40px';
    img.style.height = '58px';
    img.style.zIndex = '9999';

    // Mittelpunkt des map-Divs
    img.style.top = 'calc(50% - 29px)';   // 58px Höhe / 2
    img.style.left = 'calc(50% - 20px)';  // 40px Breite / 2
   img.style.transform = 'translate(-50%, -100%)';
    mapDiv.appendChild(img);
}})();
";
                await webView21.CoreWebView2.ExecuteScriptAsync(script);
            }
        }

        #endregion

    }

}
