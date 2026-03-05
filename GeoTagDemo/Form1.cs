using GeoTag;

namespace GeoTagDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mapCtrl1.navigateToUrl();
        }

        private void rb_google_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_google.Checked) { mapCtrl1.MapProvider = MapProvider.GoogleMaps;
                mapCtrl1.navigateToUrl();
            }
        }

        private void rb_osm_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_osm.Checked)
            {
                mapCtrl1.MapProvider = MapProvider.OpenStreetMap;
                mapCtrl1.navigateToUrl();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            mapCtrl1.showIcon();
        }

        private void mapCtrl1_CoordinateChange(object sender, GeoTagEventArgument e)
        {
            tb_latitude.Text = e.Latitude.ToString();   
            tb_longitude.Text = e.Longitude.ToString();
            tb_url.Text = e.Url.ToString();
        }
    }
}
