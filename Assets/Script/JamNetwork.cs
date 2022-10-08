using JamMeistro.Jams;
using Newtonsoft.Json;

namespace Script
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JamNetwork
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("jam")]
        public Jam Jam { get; set; }
        
        public JamNetwork(string name, Jam jam)
        {
            Name = name;
            Jam = jam;
        }
        
        public override string ToString()
        {
            return Name;
        }
        
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}