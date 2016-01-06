using AIWolf.Common.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// AIWolfのObjectを送信形式にEncode,Decodeする．
    /// <para>
    /// Singleton.
    /// </para>
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class DataConverter
    {
        static DataConverter converter;

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

        JsonSerializerSettings serializerSetting;

        DataConverter()
        {
            serializerSetting = new JsonSerializerSettings();
            // オブジェクトを名前順にして変換
            serializerSetting.ContractResolver = new OrderedContractResolver();
            // enumを文字列のまま変換
            serializerSetting.Converters.Add(new StringEnumConverter());
        }

        public string Convert(object obj)
        {
            return JsonConvert.SerializeObject(obj, serializerSetting);
        }

        public Packet ToPacket(string line)
        {
            Dictionary<string, object> map = JsonConvert.DeserializeObject<Dictionary<string, object>>(line, serializerSetting);

            Request request = map["request"] != null ? (Request)Enum.Parse(typeof(Request), (string)map["request"]) : Request.DUMMY;
            GameInfoToSend gameInfoToSend = null;
            if (map["gameInfo"] != null)
            {
                gameInfoToSend = JsonConvert.DeserializeObject<GameInfoToSend>(JsonConvert.SerializeObject(map["gameInfo"], serializerSetting), serializerSetting);
                if (map["gameSetting"] != null)
                {
                    GameSetting gameSetting = JsonConvert.DeserializeObject<GameSetting>(JsonConvert.SerializeObject(map["gameSetting"], serializerSetting), serializerSetting);
                    return new Packet(request, gameInfoToSend, gameSetting);
                }
                else
                {
                    return new Packet(request, gameInfoToSend);
                }
            }
            else if (map["talkHistory"] != null)
            {
                List<TalkToSend> talkHistoryList = ToTalkList(JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(JsonConvert.SerializeObject(map["talkHistory"], serializerSetting), serializerSetting));
                List<TalkToSend> whisperHistoryList = ToTalkList(JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(JsonConvert.SerializeObject(map["whisperHistory"], serializerSetting), serializerSetting));
                return new Packet(request, talkHistoryList, whisperHistoryList);
            }
            else
            {
                return new Packet(request);
            }
        }

        private List<TalkToSend> ToTalkList(List<Dictionary<string, string>> mapList)
        {
            List<TalkToSend> list = new List<TalkToSend>();
            foreach (var value in mapList)
            {
                TalkToSend talk = JsonConvert.DeserializeObject<TalkToSend>(JsonConvert.SerializeObject(value, serializerSetting), serializerSetting);
                list.Add(talk);
            }
            return list;
        }

        public Agent ToAgent(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is string)
            {
                Match m = new Regex(@"{""agentIdx"":(\d+)}").Match((string)obj);
                if (m.Success)
                {
                    return Agent.GetAgent(int.Parse(m.Groups[1].Value));
                }
            }
            if (obj is Agent)
            {
                return (Agent)obj;
            }
            else if (obj is Dictionary<string, object>)
            {
                //TODO 互換性? Java版では Agent.getAgent(((BigDecimal)((Map)obj).get("agentIdx")).intValue());
                return Agent.GetAgent((int)((Dictionary<string, object>)obj)["agentIdx"]);
            }
            else
            {
                throw new AIWolfRuntimeException("Can not convert to agent " + obj.GetType() + "\t" + obj);
            }
        }
    }

    /// <summary>
    /// オブジェクトを並べ替える
    /// </summary>
    class OrderedCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
        }
    }

    class OrderedContractResolver : DefaultContractResolver
    {
        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
        }
    }

}
