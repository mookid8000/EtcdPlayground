using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rebus.Etcd.Responses;

namespace Rebus.Etcd.Client
{
    class EtcdClient : IDisposable
    {
        readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        readonly HttpClient _client = new HttpClient();
        readonly string _url;

        public EtcdClient(string url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            _url = url;
        }

        public async Task<VersionResponse> GetVersion()
        {
            return await Get<VersionResponse>("/version");
        }

        public async Task Save(string key, string value)
        {
            await Put<SaveKeyResponse>($"/v2/keys/{key}", value);
        }

        public async Task<string> Load(string key)
        {
            var response = await Get<LoadKeyResponse>($"/v2/keys/{key}");

            return response?.Node?.Value;
        }

        public async Task<Key[]> LoadKeys(string parentKey = null)
        {
            var jsonText = await Get<ListKeysResponse>(parentKey == null ? "/v2/keys" : $"/v2/keys/{parentKey}");

            return jsonText?.Node
                       .Nodes.Select(n => new Key(n.Key, n.Dir))
                       .ToArray()
                   ?? new Key[0];
        }

        string GetUrl(string key)
        {
            return $"{_url}/v2/keys/{key}";
        }

        async Task<TResponse> Put<TResponse>(string relativeUrl, string value)
        {
            var url = $"{_url}/{relativeUrl}?value={HttpUtility.UrlEncode(value)}";
            var response = await _client.PutAsync(url, null);
            var responseText = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<TResponse>(responseText, _serializerSettings);
            }
            catch (Exception exception)
            {
                throw new FormatException($"Could not parse JSON response as {typeof(TResponse)}: '{responseText}'", exception);
            }
        }

        async Task<TResponse> Get<TResponse>(string relativeUrl)
        {
            var url = $"{_url}/{relativeUrl}";
            var responseText = await _client.GetStringAsync(url);
            Console.WriteLine(responseText);
            try
            {
                return JsonConvert.DeserializeObject<TResponse>(responseText, _serializerSettings);
            }
            catch (Exception exception)
            {
                throw new FormatException($"Could not parse JSON response from {url} as {typeof(TResponse)}: '{responseText}'", exception);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
