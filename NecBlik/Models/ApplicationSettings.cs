using Newtonsoft.Json;

namespace NecBlik.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ApplicationSettings
    {
        public string Language { get; set; } = "en";
    }
}
