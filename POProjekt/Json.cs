using Newtonsoft.Json;

namespace POProjekt
{
    /// <summary> Używana do zapisu/odczytu z dysku. </summary>
    public class Json
    {
        public static JsonSerializerSettings JsonSerializerSettings = new()
        {
            Formatting = Formatting.None,
        };

        public readonly int Hash;

        public Json(int hash)
        {
            Hash = hash;
        }

        public Json(object obj)
        {
            Hash = obj.GetHashCode();
        }
    }
}
