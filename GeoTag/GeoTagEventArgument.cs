namespace GeoTag
{
    public class GeoTagEventArgument
    {


        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string Url { get; private set; }


        public GeoTagEventArgument(double latitude, double longitude, string url)
        {
            Latitude = latitude;
            Longitude = longitude;
            Url = url;
        }
    }
}
