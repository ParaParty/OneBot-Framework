using System;
using Newtonsoft.Json;
using Sora.Interfaces;
using Sora.OnebotModel;

namespace OneBot.CommandRoute.Models.VO
{
    public class CQHttpServerConfigModel
    {
        /// <summary>模式 ws / reverse_ws</summary>
        [JsonProperty("Mode")]
        public string Mode { get; set; }

        /// <summary>反向服务器监听地址</summary>
        [JsonProperty("Location")]
        public string Location { get; set; } = null;

        /// <summary>反向服务器监听地址</summary>
        [JsonProperty("Host")]
        public string Host { get; set; } = null;

        /// <summary>反向服务器端口</summary>
        [JsonProperty("Port")]
        public uint Port { get; set; } = 8080;

        /// <summary>鉴权Token</summary>
        [JsonProperty("AccessToken")]
        public string AccessToken { get; set; } = "";

        /// <summary>Universal请求路径</summary>
        [JsonProperty("UniversalPath")]
        public string UniversalPath { get; set; } = "";

        /// <summary>
        /// <para>心跳包超时设置(秒)</para>
        /// <para>此值请不要小于或等于客户端心跳包的发送间隔</para>
        /// </summary>
        [JsonProperty("HeartBeatTimeOut")]
        public uint HeartBeatTimeOut { get; set; } = 10;

        /// <summary>
        /// <para>客户端API调用超时设置(毫秒)</para>
        /// <para>默认为1000无需修改</para>
        /// </summary>
        [JsonProperty("ApiTimeOut")]
        public uint ApiTimeOut { get; set; } = 1000;

        public ISoraConfig ToServiceConfig()
        {
            var mode = Mode?.ToLower();
            if (mode == null && !string.IsNullOrEmpty(Location))
            {
                mode = "reverse_ws";
            }
            if (mode == null && !string.IsNullOrEmpty(Host))
            {
                mode = "ws";
            }

            if (mode == "reverse_ws" && !string.IsNullOrEmpty(Location))
            {
                return new ServerConfig
                {
                    Location = Location,
                    Port = Port,
                    AccessToken = AccessToken,
                    UniversalPath = UniversalPath,
                    HeartBeatTimeOut = HeartBeatTimeOut,
                    ApiTimeOut = ApiTimeOut,
                    EnableSoraCommandManager = false
                };
            }
            
            if (mode == "ws" && !string.IsNullOrEmpty(Host))
            {
                return new ClientConfig
                {
                    Host = Host,
                    Port = Port,
                    AccessToken = AccessToken,
                    UniversalPath = UniversalPath,
                    HeartBeatTimeOut = HeartBeatTimeOut,
                    ApiTimeOut = ApiTimeOut,
                    EnableSoraCommandManager = false
                };
            }

            throw new ArgumentException(@"There is something wrong in OneBot settings.");
        }
    }
}