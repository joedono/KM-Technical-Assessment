namespace KM_Technical_Assessment.Models
{
    using System;

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

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var that = other as KMPoint;
            if (that == null)
            {
                return false;
            }

            return this.x == that.x && this.y == that.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }
}
