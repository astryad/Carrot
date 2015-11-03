namespace Carrot
{
    public interface IMessageSerializer
    {
        byte[] Serialize(string message);
    }
}