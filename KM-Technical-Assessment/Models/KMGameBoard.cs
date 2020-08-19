namespace KM_Technical_Assessment.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var that = other as KMGameBoard;
            if (that == null)
            {
                return false;
            }

            return this.nodes.SequenceEqual(that.nodes);
        }

        public override int GetHashCode()
        {
            var hash = 1;
            foreach(var node in this.nodes)
            {
                hash = HashCode.Combine(hash, node.GetHashCode());
            }

            return hash;
        }
    }
}
