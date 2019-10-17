using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.Common.DAO.Sms
{
    using System.Net;

    using Properties;

    using Util.Config;

    public partial class SwisscomSmsClient
    {
        private readonly string username;
        private readonly string password;
        private readonly string proxyUrl;

        public SwisscomSmsClient(string baseUrl, string username, string password, string proxyUrl)
            : this(baseUrl)
        {
            this.username = username;
            this.password = password;
            this.proxyUrl = proxyUrl;
        }

        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        partial void PrepareClientHandler(System.Net.Http.HttpClientHandler handler)
        {
            if (!string.IsNullOrEmpty(proxyUrl))
            {
                handler.Proxy = new WebProxy(new Uri(proxyUrl, UriKind.Absolute));
                handler.UseProxy = true;
            }
        }
    }
}
