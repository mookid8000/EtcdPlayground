using System;
using Newtonsoft.Json;

namespace Rebus.Etcd.Tests.Extensions
{
    public static class ObjectExtensions
    {
        public static void Dump(this object obj)
        {
            Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}