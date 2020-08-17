namespace KM_Technical_Assessment.Models
{
    using System.Collections.Generic;

    public class KMGameBoard
    {
        public List<KMPoint> nodes { get; set; }

        public KMGameBoard()
        {
            this.nodes = new List<KMPoint>();
        }
    }
}
