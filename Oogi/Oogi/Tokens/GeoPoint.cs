namespace Oogi.Tokens
{
    public class GeoPoint
    {
        public string Point => "Point";
        public double[] Coordinates { get; set; }
        
        public GeoPoint()
        {            
        }

        public GeoPoint(long @long, long lat)
        {
            Coordinates = new double[] { @long, lat };            
        }
    }
}
