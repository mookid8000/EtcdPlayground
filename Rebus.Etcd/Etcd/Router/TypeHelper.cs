using System;

namespace Rebus.Etcd.Router
{
    class TypeHelper
    {
        public string GetName(Type type)
        {
            return type.Name;
        }
    }
}