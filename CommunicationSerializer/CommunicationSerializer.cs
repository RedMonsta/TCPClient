using Newtonsoft.Json;
using CommandsLib;

namespace CommunicationSerializer
{
    public class CommunicationSerializer
    {
        public Response Deserialize(string str)
        {
            return JsonConvert.DeserializeObject<Response>(str, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public string Serialize(object command)
        {
            return JsonConvert.SerializeObject(command, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            });
        }
    }
}
