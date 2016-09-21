namespace Rebus.Etcd.Responses
{
    class LoadKeyResponse
    {
        public GetKeyResult Node { get; }

        public LoadKeyResponse(GetKeyResult node)
        {
            Node = node;
        }
    }

    class GetKeyResult
    {
        public GetKeyResult(string key, string value, int modifiedIndex, int createdIndex)
        {
            Key = key;
            Value = value;
            ModifiedIndex = modifiedIndex;
            CreatedIndex = createdIndex;
        }

        public string Key { get; }
        public string Value { get; }
        public int ModifiedIndex { get; }
        public int CreatedIndex { get; }
    }
}