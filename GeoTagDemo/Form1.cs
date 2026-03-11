
using ExifEdit;
using GeoTag;

//using MetadataExtractor;
//using Directory = MetadataExtractor.Directory;

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
            if (rb_google.Checked)
            {
                mapCtrl1.MapProvider = MapProvider.GoogleMaps;
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

        private void button3_Click(object sender, EventArgs e)
        {
            //IEnumerable<Directory> directories = ImageMetadataReader.ReadMetadata("D:\\tmp\\T2\\IR5_2025-05-01-5691A.jpg");
            //foreach (var  directory in directories)
            //    foreach (var tag in directory.Tags)
            //        lb_tags.Items.Add($"{directory.Name} - {tag.Name} = {tag.Description}");


            ReadExif exifReader = new ReadExif();
            exifReader.parsefile(@"D:\TestBilder\Watvögel\IR5_2022-10-28-3046A.jpg");
            foreach (var item in exifReader.ExifEntryList)
            {

                lb_tags.Items.Add($"{item.Key} - {item.Value.tagtyp.ToString()} = {item.Value.value}");

            }
            foreach (var item in exifReader.iptcList)
            {
                foreach (var field in item.Value)
                {
                    lb_tags.Items.Add($"{item.Key} - {field.Tag} = {field.DataString}");
                }
            }
        }
    }
}
