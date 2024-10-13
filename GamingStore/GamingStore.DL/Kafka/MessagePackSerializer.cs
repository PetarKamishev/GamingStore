using Confluent.Kafka;
using GamingStore.GamingStore.Models.Models;
using MessagePack;

namespace GamingStore.GamingStore.DL.Kafka
{
    public class MessagePackSerializer<T> : ISerializer<T>
    {       
        public byte[] Serialize(T data , SerializationContext context)
        {
            return MessagePackSerializer.Serialize(data);
        }
    }
}
