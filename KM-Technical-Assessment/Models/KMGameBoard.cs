namespace KM_Technical_Assessment.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Game board. Keeps track of nodes already picked in the game.
    /// Nodes are in the order they were added to the path,
    /// meaning the first node is the beginning of the path and
    /// the last node is the end.
    /// </summary>
    public class KMGameBoard
    {
        public List<KMPoint> nodes { get; set; }

        public KMGameBoard()
        {
            this.nodes = new List<KMPoint>();
        }
    }
}
