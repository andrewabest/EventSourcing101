using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Domain
{
    public static class AggregateRootExtensions
    {
        public static T BinaryClone<T>(this T entity) where T : IAggregateRoot
        {
            var serializer = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, entity);
                ms.Position = 0;
                var clone = (T)serializer.Deserialize(ms);
                return clone;
            }
        }
    }
}