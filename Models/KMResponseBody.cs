namespace KM_Technical_Assessment.Models
{
    /// <summary>
    /// Response wrapper subclass.
    /// Used by <see cref="KMResponse"/> response wrapper
    /// to wrap actual messages.
    /// </summary>
    public class KMResponseBody
    {
        public object newLine { get; set; }

        public string heading { get; set; }

        public string message { get; set; }
    }
}