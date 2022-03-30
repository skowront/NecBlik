using Newtonsoft.Json;

namespace ZigBee.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ApplicationSettings
    {
        public string Language { get; set; } = "en";
    }
}
