using System.Collections.Generic;
using System.Linq;

namespace Rebus.Etcd.Responses
{
    // {"action":"get","node":{"dir":true,"nodes":[{"key":"/test","value":"","modifiedIndex":9,"createdIndex":9}]}}
    public class ListKeysResponse
    {
        public ListKeysResponse(ListKeysResponseNode node)
        {
            Node = node;
        }

        public ListKeysResponseNode Node { get;  }
    }

    public class ListKeysResponseNode
    {
        public ListKeysResponseNode(IEnumerable<ListKeysResult> nodes)
        {
            Nodes = nodes.ToArray();
        }

        public ListKeysResult[] Nodes { get; }
    }

    public class ListKeysResult
    {
        public ListKeysResult(string key, string value, int modifiedIndex, int createdIndex)
        {
            Key = key;
            Value = value;
            ModifiedIndex = modifiedIndex;
            CreatedIndex = createdIndex;
        }

        public string Key { get;  }
        public string Value { get; }
        public int ModifiedIndex { get; }
        public int CreatedIndex { get; }
    }
}