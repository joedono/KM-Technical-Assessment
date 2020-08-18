namespace KM_Technical_Assessment.Models
{
    /// <summary>
    /// Response wrapper class.
    /// All responses to the client should be wrapped in this.
    /// </summary>
    public class KMResponse
    {
        public string msg { get; set; }

        public KMResponseBody body { get; set; }
    }
}
