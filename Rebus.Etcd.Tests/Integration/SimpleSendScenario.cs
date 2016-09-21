using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Transport.InMem;

namespace Rebus.Etcd.Tests.Integration
{
    [TestFixture]
    public class SimpleSendScenario : FixtureBase
    {
        BuiltinHandlerActivator _activator1;
        BuiltinHandlerActivator _activator2;
        InMemNetwork _network;

        protected override void SetUp()
        {
                    _network = new InMemNetwork();

            _activator1 = StartBus("queue-1");
            _activator2 = StartBus("queue-2", b => b.RegisterAsOwnerOf<RelevantMessage>());
        }

        [Test]
        public async Task ItWorks()
        {
            await _activator1.Bus.Send(new RelevantMessage("this is good!"));

            await Task.Delay(5000);
        }

        BuiltinHandlerActivator StartBus(string inputQueueName, Action<EtcdConfigurationBuilder> builderAction = null)
        {
            var handlerActivator = new BuiltinHandlerActivator();

            Using(handlerActivator);

            Configure.With(handlerActivator)
                .Transport(t => t.UseInMemoryTransport(_network, inputQueueName))
                .Routing(r =>
                {
                    var builder = r.UseEtcd("http://localhost:2379");

                    builderAction?.Invoke(builder);
                })
                .Start();

            return handlerActivator;
        }
    }

    public class RelevantMessage
    {
        public string Text { get; }

        public RelevantMessage(string text)
        {
            Text = text;
        }
    }
}