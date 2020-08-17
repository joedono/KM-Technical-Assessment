namespace KM_Technical_Assessment.Models
{
    public class KMPoint
    {
        public int x { get; set; }

        public int y { get; set; }

        public KMPoint() { }

        public KMPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
