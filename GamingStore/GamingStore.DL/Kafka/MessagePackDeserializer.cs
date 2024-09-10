using Confluent.Kafka;
using MessagePack;

namespace GamingStore.GamingStore.DL.Kafka
{
    public class MessagePackDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(
            ReadOnlySpan<byte> data,
            bool isNull,
            SerializationContext context )
        {
            return
                MessagePackSerializer.Deserialize<T>(data.ToArray());
        }
    }
}
