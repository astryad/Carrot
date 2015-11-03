namespace Carrot.Model
{
    public class Exchange
    {
        public Exchange(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public string Type { get; private set; }
        public string Name { get; private set; }
    }
}