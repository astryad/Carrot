using System.Text;

namespace Carrot
{
    public class Utf8MessageSerializer : IMessageSerializer
    {
        public byte[] Serialize(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }
    }
}