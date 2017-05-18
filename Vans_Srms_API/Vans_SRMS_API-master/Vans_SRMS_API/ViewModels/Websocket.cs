using Newtonsoft.Json;

namespace Vans_SRMS_API.ViewModels
{
    public enum MessageType
    {
        Text,
        ClientMethodInvocation,
        ConnectionEvent
    }

    public class WebSocketMessage
    {
        public string Route { get; set; }
        public string DeviceKey { get; set; }
        public object Data { get; set; }

        public WebSocketMessage(object d, string r = "", string k = "")
        {
            Route = r;
            DeviceKey = k;
            Data = d;
        }
    }

    public class InvocationDescriptor
    {
        [JsonProperty("methodName")]
        public string MethodName { get; set; }

        [JsonPropertyAttribute("arguments")]
        public object[] Arguments { get; set; }
    }

}
