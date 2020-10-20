using Newtonsoft.Json;

namespace MBCT.Csv.Models
{
    /// <summary>
    /// SoapNote class. Represents a SOAP note.
    /// </summary>
    public class SoapNote
    {
        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        [JsonProperty(PropertyName = "SerializedContentId")]
        public string SerializedContentId { get; set; }

        [JsonProperty(PropertyName = "Subjective")]
        public string Subjective { get; set; }

        [JsonProperty(PropertyName = "Objective")]
        public string Objective { get; set; }

        [JsonProperty(PropertyName = "Assessment")]
        public string Assessment { get; set; }

        [JsonProperty(PropertyName = "Plan")]
        public string Plan { get; set; }

        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
