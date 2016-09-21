using System;
using System.Threading.Tasks;
using Rebus.Etcd.Client;
using Rebus.Exceptions;
using Rebus.Messages;
using Rebus.Routing;
using Rebus.Subscriptions;

namespace Rebus.Etcd.Router
{
    class EtcdRouter : IRouter, ISubscriptionStorage
    {
        readonly EtcdClient _client;
        readonly TypeHelper _typeHelper;

        public EtcdRouter(EtcdClient client, TypeHelper typeHelper)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (typeHelper == null) throw new ArgumentNullException(nameof(typeHelper));
            _client = client;
            _typeHelper = typeHelper;
        }

        public async Task<string> GetDestinationAddress(Message message)
        {
            var body = message.Body;
            var typeName = _typeHelper.GetName(body.GetType());

            var address = await _client.Load(typeName);

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new RebusApplicationException($"Could not find destination address for command/request message of type {body.GetType()}");
            }

            return address;
        }

        public Task<string> GetOwnerAddress(string topic)
        {
            throw new System.NotImplementedException();
        }

        public Task<string[]> GetSubscriberAddresses(string topic)
        {
            throw new System.NotImplementedException();
        }

        public Task RegisterSubscriber(string topic, string subscriberAddress)
        {
            throw new System.NotImplementedException();
        }

        public Task UnregisterSubscriber(string topic, string subscriberAddress)
        {
            throw new System.NotImplementedException();
        }

        public bool IsCentralized => true;
    }
}