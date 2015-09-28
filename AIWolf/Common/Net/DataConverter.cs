using AIWolf.Common.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// AIWolfのObjectを送信形式にEncode,Decodeする．
    /// <para>
    /// Singleton
    /// </para>
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class DataConverter
    {
        private static DataConverter converter;

        /// <summary>
        /// 唯一のConverterを取得
        /// </summary>
        /// <returns></returns>
        public static DataConverter GetInstance()
        {
            if (converter == null)
            {
                converter = new DataConverter();
            }
            return converter;
        }

        //private JavaScriptSerializer serializer = new JavaScriptSerializer();

        private DataConverter()
        {
        }

        public string Convert(object obj)
        {
            //return serializer.Serialize(obj);
            return JsonConvert.SerializeObject(obj);
        }

        public Packet ToPacket(string line)
        {
            Dictionary<string, object> map = JsonConvert.DeserializeObject<Dictionary<string, object>>(line);
            //Dictionary<string, object> map = serializer.Deserialize<Dictionary<string, object>>(line);

            Request request = (Request)Enum.Parse(typeof(Request), (string)map["request"]);
            //GameInfoToSend gameInfoToSend = serializer.Deserialize<GameInfoToSend>(serializer.Serialize(map["gameInfo"]));
            GameInfoToSend gameInfoToSend = JsonConvert.DeserializeObject<GameInfoToSend>(JsonConvert.SerializeObject(map["gameInfo"]));
            if (map["gameSetting"] != null)
            {
                //GameSetting gameSetting = serializer.Deserialize<GameSetting>(serializer.Serialize(map["gameSetting"]));
                GameSetting gameSetting = JsonConvert.DeserializeObject<GameSetting>(JsonConvert.SerializeObject(map["gameSetting"]));
                return new Packet(request, gameInfoToSend, gameSetting);
            }
            else
            {
                return new Packet(request, gameInfoToSend);
            }
        }

        public Agent ToAgent(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is string)
            {
                Match m = new Regex("\\{\"agentIdx\":(\\d+)\\}").Match((string)obj);
                if (m.Success)
                {
                    return Agent.GetAgent(int.Parse(m.Value));
                }
            }
            if (obj is Agent)
            {
                return (Agent)obj;
            }
            else if (obj is Dictionary<string, string>)
            {
                return Agent.GetAgent(int.Parse(((Dictionary<string, string>)obj)["agentIdx"])); //TODO あやしい
            }
            else
            {
                throw new AIWolfRuntimeException("Can not convert to agent " + obj.GetType() + "\t" + obj);
                //return serializer.Deserialize<Agent>(serializer.Serialize(obj));
            }
        }
    }
}
