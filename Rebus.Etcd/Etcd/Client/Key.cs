namespace Rebus.Etcd.Client
{
    class Key
    {
        public Key(string name, bool isContainer)
        {
            Name = name;
            IsContainer = isContainer;
        }

        public string Name { get; }
        public bool IsContainer { get; }
    }
}