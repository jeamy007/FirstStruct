
namespace AppleHelp.Json
{
    public class JsonConvert
    {
        public static T ToObject<T>(string jsonString)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
        }
        public static string ToString(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}