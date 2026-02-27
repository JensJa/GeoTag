
namespace GeoTagDemo
{
    public partial class Form3 : Form
    {


        string apiKey = "";

        String htmcode = @"
<!DOCTYPE html>
<html>
<head>
  <title>Google Maps WebView2 Beispiel</title>
  <script src=""https://maps.googleapis.com/maps/api/js?key=DEIN_API_KEY""></script>
</head>
<body>
  <div id=""map"" style=""width:100%; height:100%;""></div>
  <script>
    let map;

    function initMap() {
      map = new google.maps.Map(document.getElementById(""map""), {
        center: { lat: 52.1, lng: 13.2 }, // Startposition
        zoom: 8
      });

      // Klick-Event Listener
      map.addListener(""click"", function(event) {
        const lat = event.latLng.lat();
        const lng = event.latLng.lng();

        // Nachricht an WebView2 senden
        window.chrome.webview.postMessage({ lat: lat, lng: lng });
      });
    }

    window.onload = initMap;
  </script>
</body>
</html>


";



        public static string ErzeugeGoogleMapsUrl(string latitude, string longitude, int zoom = 21)
        //https://www.google.com/maps/@52.1229021,12.9080458,24212m/data=!3m1!1e3?authuser=0&entry=ttu&g_ep=EgoyMDI2MDIxOC4wIKXMDSoASAFQAw%3D%3D
        {// Zoom begrenzen (Google Maps max. 21)
            if (zoom > 21) zoom = 21; if (zoom < 0) zoom = 0;

            string url = $"https://www.google.com/maps/@{latitude},{longitude},{zoom}z"; return url;
        }



        public Form3()
        {
            InitializeComponent();
            initBrowser();
        }


        private async Task initialized()
        {
            await webView.EnsureCoreWebView2Async(null);
            await webViewGoogle.EnsureCoreWebView2Async(null);
        }
        //52.074865, 12.951682


        public async void initBrowser()

        {
            await initialized();
            webViewGoogle.CoreWebView2.Navigate(@"https://www.google.com/maps/@52.1229021,12.9080458,20298m");
            webView.CoreWebView2.Navigate(@"https://www.openstreetmap.org");

        }

        /*
            img.style.position = 'fixed';
            img.style.top = '{top}px';
            img.style.left = '{left}px';
        */

        public async Task zeichneIcon2()
        {
            using (var ms = new MemoryStream())
            {
                Properties.Resources.location.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                string base64Image = Convert.ToBase64String(ms.ToArray());
                string top = "" + webView.Height / 2;
                string left = "" + webView.Width / 2;
                string script = $@"
        (function() {{
            var img = document.createElement('img');
            img.src = 'data:image/png;base64,{base64Image}';
            img.style.position = 'absolute';
            img.style.top = '50%';
            img.style.left = '50%';
            img.style.width = '40px';
            img.style.height = '58px';
             img.style.transform = 'translate(-36%, -87%)';
                        
            img.style.zIndex = '9999';
            img.id = 'geoTagIcon';
            document.body.appendChild(img);

        }})();
 ";
                await webView.CoreWebView2.ExecuteScriptAsync(script);
            }
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        public async Task ZeichneGoogleMapsIcon()
        {
            using (var ms = new MemoryStream())
            {
                Properties.Resources.location.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
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

                await webViewGoogle.CoreWebView2.ExecuteScriptAsync(script);
            }
        }
        /// <summary>
        /// /////////////////////////////////////////
        /// </summary>
        /// <returns></returns>

        public async Task zeichneIcon()
        {
            using (var ms = new MemoryStream())
            {
                Properties.Resources.location.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                string base64Image = Convert.ToBase64String(ms.ToArray());
                string top = "" + webView.Height / 2;
                string left = "" + webView.Width / 2;
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
                await webView.CoreWebView2.ExecuteScriptAsync(script);
            }
        }



        //  img.style.transform = 'translate(-36%, -87%)';

        double latitude = 0;
        double longitude = 0;

        public void getGPSKoordinates()
        {
            string acturl = webView.Source.ToString();
            string[] parameters;
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


            lb_latitude.Text = "" + latitude;
            lb_longitude.Text = "" + longitude;
            tb_url.Text = acturl;

        }




        private void button1_Click(object sender, EventArgs e)
        {

            zeichneIcon();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // https://www.openstreetmap.org/#map=19/52.233908/13.130380
            string url = $"https://www.openstreetmap.org/#map=19/{tb_latitude.Text.Replace(',', '.')}/{tb_Longitude.Text.Replace(',', '.')}";
            tb_url.Text = url;
            webView.CoreWebView2.Navigate(tb_url.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webView.CoreWebView2.Navigate(tb_url.Text);
        }

        private void webView_SourceChanged(object sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs e)
        {
            if (webView != null)
            {
                getGPSKoordinates();
            }
        }

        private void tb_latitude_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void tb_url_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            tb_url_google.Text = ErzeugeGoogleMapsUrl(lb_latitude.Text.Replace(',', '.'), lb_longitude.Text.Replace(',', '.'), 19);
            webViewGoogle.CoreWebView2.Navigate(tb_url_google.Text);

            Thread.Sleep(1000);
            ZeichneGoogleMapsIcon();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ZeichneGoogleMapsIcon();
        }


        //



    }
}

