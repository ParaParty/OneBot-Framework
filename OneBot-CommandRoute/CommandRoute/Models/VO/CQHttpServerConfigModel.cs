using Newtonsoft.Json;
using Sora.Server;

namespace OneBot.CommandRoute.Models.VO
{
    public class CQHttpServerConfigModel
    {
        /// <summary>反向服务器监听地址</summary>
        [JsonProperty("Location")]
        public string Location { get; set; }

        /// <summary>反向服务器端口</summary>
        [JsonProperty("Port")]
        public uint Port { get; set; }

        /// <summary>鉴权Token</summary>
        [JsonProperty("AccessToken")]
        public string AccessToken { get; set; }

        /// <summary>API请求路径</summary>
        [JsonProperty("ApiPath")]
        public string ApiPath { get; set; }

        /// <summary>Event请求路径</summary>
        [JsonProperty("EventPath")]
        public string EventPath { get; set; }

        /// <summary>Universal请求路径</summary>
        [JsonProperty("UniversalPath")]
        public string UniversalPath { get; set; }

        /// <summary>
        /// <para>心跳包超时设置(秒)</para>
        /// <para>此值请不要小于或等于客户端心跳包的发送间隔</para>
        /// </summary>
        [JsonProperty("HeartBeatTimeOut")]
        public uint HeartBeatTimeOut { get; set; }

        /// <summary>
        /// <para>客户端API调用超时设置(毫秒)</para>
        /// <para>默认为1000无需修改</para>
        /// </summary>
        [JsonProperty("ApiTimeOut")]
        public uint ApiTimeOut { get; set; }

        public ServerConfig ToServerConfig()
        {
            return new ServerConfig
            {
                Location = Location,
                Port = Port,
                AccessToken = AccessToken,
                ApiPath = ApiPath,
                EventPath = EventPath,
                UniversalPath = UniversalPath,
                HeartBeatTimeOut = HeartBeatTimeOut,
                ApiTimeOut = ApiTimeOut,
                EnableSoraCommandManager = false
            };
        }
    }
}