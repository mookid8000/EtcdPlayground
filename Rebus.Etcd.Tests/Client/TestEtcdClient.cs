using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Rebus.Etcd.Client;

namespace Rebus.Etcd.Tests.Client
{
    [TestFixture]
    public class TestEtcdClient : FixtureBase
    {
        EtcdClient _client;

        protected override void SetUp()
        {
            _client = new EtcdClient("http://localhost:2379");
        }

        [Test]
        public async Task CanDoIt()
        {
            var version = await _client.GetVersion();

            Console.WriteLine($"Version is {version.Etcdcluster} (cluster) / {version.Etcdserver} (server)");
        }

        [Test]
        public async Task CanSaveAndLoadValue()
        {
            var text = DateTime.Now.ToString("O");

            await _client.Save("/test", text);

            var loadedValue = await _client.Load("/test");

            Console.WriteLine(loadedValue);

            Assert.That(loadedValue, Is.EqualTo(text));
        }

        [Test]
        public async Task CanListKeys()
        {
            var keys = await _client.LoadKeys();

            Console.WriteLine($@"KEYS:

{string.Join(Environment.NewLine, keys)}");
        }
    }
}
