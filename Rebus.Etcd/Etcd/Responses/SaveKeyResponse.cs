namespace Rebus.Etcd.Responses
{
    // {"action":"set","node":{"key":"/test","value":"","modifiedIndex":10,"createdIndex":10},"prevNode":{"key":"/test","value":"","modifiedIndex":9,"createdIndex":9}}
    class SaveKeyResponse
    {
        public SaveKeyResponse(string action, SaveKeyResponseNode node)
        {
            Action = action;
            Node = node;
        }

        public string Action { get; }

        public SaveKeyResponseNode Node { get; }
    }

    class SaveKeyResponseNode
    {
        public SaveKeyResponseNode(int createdIndex, int modifiedIndex, string value, string key)
        {
            CreatedIndex = createdIndex;
            ModifiedIndex = modifiedIndex;
            Value = value;
            Key = key;
        }

        public string Key { get; }
        public string Value { get; }
        public int ModifiedIndex { get; }
        public int CreatedIndex { get; }
    }

    class SaveKeyResponseNodeWithPrevious : SaveKeyResponseNode
    {
        public SaveKeyResponseNodeWithPrevious(int createdIndex, int modifiedIndex, string value, string key, SaveKeyResponseNode prevNode)
            : base(createdIndex, modifiedIndex, value, key)
        {
            PrevNode = prevNode;
        }

        public SaveKeyResponseNode PrevNode { get; }
    }
}