using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rebus.Etcd.Client;
using Rebus.Etcd.Router;
using Rebus.Routing;
using Rebus.Subscriptions;
using Rebus.Transport;

namespace Rebus.Config
{
    public static class EtcdRoutingConfigurationExtensions
    {
        public static EtcdConfigurationBuilder UseEtcd(this StandardConfigurer<IRouter> configurer, string url)
        {
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));
            if (url == null) throw new ArgumentNullException(nameof(url));

            var typeHelper = new TypeHelper();
            var configurationBuilder = new EtcdConfigurationBuilder(typeHelper);

            configurer.OtherService<EtcdClient>().Register(c =>
            {
                var etcdClient = new EtcdClient(url);
                var transport = c.Get<ITransport>();

                configurationBuilder.Initialize(etcdClient, transport);

                return etcdClient;
            });

            configurer.OtherService<EtcdRouter>().Register(c =>
            {
                var etcdClient = c.Get<EtcdClient>();

                return new EtcdRouter(etcdClient, typeHelper);
            });

            configurer.Register(c => c.Get<EtcdRouter>());

            configurer.OtherService<ISubscriptionStorage>().Register(c => c.Get<EtcdRouter>());

            return configurationBuilder;
        }
    }

    public class EtcdConfigurationBuilder
    {
        readonly HashSet<Type> _ownedCommandTypes = new HashSet<Type>();
        readonly TypeHelper _typeHelper;

        internal EtcdConfigurationBuilder(TypeHelper typeHelper)
        {
            if (typeHelper == null) throw new ArgumentNullException(nameof(typeHelper));
            _typeHelper = typeHelper;
        }

        internal void Initialize(EtcdClient client, ITransport transport)
        {
            var address = transport.Address;
            var done = new ManualResetEvent(false);

            ThreadPool.QueueUserWorkItem(async _ =>
            {
                await Task.WhenAll(_ownedCommandTypes.Select(async type =>
                {
                    var typeName = _typeHelper.GetName(type);
                    await client.Save(typeName, address);
                }));

                done.Set();
            });

            done.WaitOne();
        }

        public EtcdConfigurationBuilder RegisterAsOwnerOf<TMessage>()
        {
            _ownedCommandTypes.Add(typeof(TMessage));
            return this;
        }
    }
}