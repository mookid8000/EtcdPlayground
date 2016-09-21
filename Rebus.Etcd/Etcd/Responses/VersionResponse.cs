namespace Rebus.Etcd.Responses
{
    //  {"etcdserver":"3.1.0-alpha.0","etcdcluster":"3.1.0"}
    class VersionResponse
    {
        public VersionResponse(string etcdserver, string etcdcluster)
        {
            Etcdserver = etcdserver;
            Etcdcluster = etcdcluster;
        }

        public string Etcdserver { get; }
        public string Etcdcluster { get; }
    }
}