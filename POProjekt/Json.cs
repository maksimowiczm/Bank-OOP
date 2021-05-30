using Newtonsoft.Json;

namespace POProjekt
{
    public class Json
    {
        public static JsonSerializerSettings JsonSerializerSettings = new()
        {
            Formatting = Formatting.Indented,
        };

        public readonly int Hash;

        public Json(object obj)
        {
            Hash = obj.GetHashCode();
        }
    }
}
