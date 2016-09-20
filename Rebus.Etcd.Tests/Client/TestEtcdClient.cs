using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Rebus.Etcd.Client;
using Rebus.Etcd.Tests.Extensions;

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
        public async Task CanGetVersion()
        {
            var version = await _client.GetVersion();

            version.Dump();

            Assert.That(version.Etcdserver, Is.EqualTo("3.1.0-alpha.0"));
            Assert.That(version.Etcdcluster, Is.EqualTo("3.1.0"));
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

            keys.Dump();
        }

        [Test]
        public async Task CanListChildKeys()
        {
            await _client.Save("/test3/key1", "hey");
            await _client.Save("/test3/key2", "hey");
            await _client.Save("/test3/key3", "hey");

            var keys = await _client.LoadKeys("/test3");

            keys.Dump();
        }
    }
}
