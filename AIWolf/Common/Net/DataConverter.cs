using AIWolf.Common.Data;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

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

        private DataConverter()
        {
        }

        public string Convert(object obj)
        {
            JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            return ser.Serialize(obj);
        }

        public Packet ToPacket(string line)
        {
            JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> map = ser.Deserialize<Dictionary<string, object>>(line);

            Request request = (Request)Enum.Parse(typeof(Request), (string)map["request"]);
            GameInfoToSend gameInfoToSend = ser.Deserialize<GameInfoToSend>((string)map["gameInfo"]);
            if (map["gameSetting"] != null)
            {
                GameSetting gameSetting = ser.Deserialize<GameSetting>((string)map["gameSetting"]);
                return new Packet(request, gameInfoToSend, gameSetting);
            }
            else
            {
                return new Packet(request, gameInfoToSend);
            }
        }

        public Agent ToAgent(object obj)
        {
            if(obj == null)
            {
                return null;
            }
            if(obj.GetType() == typeof(string))
            {
                Match m = new Regex("\\{\"agentIdx\":(\\d+)\\}").Match((string)obj);
                if (m.Success)
                {
                    return Agent.GetAgent(int.Parse(m.Value));
                }
            }
            if(obj.GetType() == typeof(Agent))
            {
                return (Agent)obj;
            }
            //TODO 実装し残しあり
            return null;
        }
    }
}
