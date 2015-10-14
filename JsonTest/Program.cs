using AIWolf.Common.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace JsonTest
{
    //[JsonConverter(typeof(StringEnumConverter))]
    //enum Role2
    //{
    //    BODYGUARD,
    //    FREEMASON, // ver0.1.xでは使用されません
    //    MEDIUM,
    //    POSSESSED,
    //    SEER,
    //    VILLAGER,
    //    WEREWOLF
    //}

    class Program
    {
        static void Main(string[] args)
        {
            JsonSerializerSettings serializerSetting = new JsonSerializerSettings();
            DataConverter dc = DataConverter.GetInstance();
            //var o = new OrderedCamelCasePropertyNamesContractResolver();
            //serializerSetting.ContractResolver = o;
            serializerSetting.Converters.Add(new StringEnumConverter());

            GameSetting gs = GameSetting.GetDefaultGame(5);
            Console.WriteLine(dc.Convert(gs));
        }
    }

    class OrderedCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
        }
    }

}
