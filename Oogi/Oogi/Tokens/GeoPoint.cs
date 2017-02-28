namespace Oogi.Tokens
{
    public class GeoPoint
    {
        public string Point => "Point";
        public double[] Coordinates { get; set; }
        
        public GeoPoint()
        {            
        }

        public GeoPoint(double @long, double lat)
        {
            Coordinates = new[] { @long, lat };            
        }
    }
}
