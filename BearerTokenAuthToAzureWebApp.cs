using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace CreateBearerTokenAAD
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();

            var token = ServicePrincipal.GetS2SAccessTokenForProdMSAAsync().Result;

            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.AccessToken);

            // make the call to the resource requiring auth!
            var resp = client.GetAsync("<<URL of your web app>>").Result;

            // do something with the response
            string rene = resp.StatusCode.ToString();
        }
    }

    public static class ServicePrincipal
    {
        /// <summary>
        /// The variables below are standard Azure AD terms from our various samples
        /// We set these in the Azure Portal for this app for security and to make it easy to change (you can reuse this code in other apps this way)
        /// You can name each of these what you want as long as you keep all of this straight
        /// </summary>
        static string authority = "https://login.microsoftonline.com/<<your tenant name>>.onmicrosoft.com";
        static string clientId = "yourSPNclientID";
        static string clientSecret = "yourSPNclientSecret=";
        static string resource = "<<client ID of target app"; // Client Id van geregistreerde Azure AD app gelinkt aan https://test-lincon-app5.azurewebsites.net/score

        /// <summary>
        /// wrapper that passes the above variables
        /// </summary>
        /// <returns></returns>
        static public async Task<AuthenticationResult> GetS2SAccessTokenForProdMSAAsync()
        {
            return await GetS2SAccessToken(authority, resource, clientId, clientSecret);
        }

        static async Task<AuthenticationResult> GetS2SAccessToken(string authority, string resource, string clientId, string clientSecret)
        {
            var clientCredential = new ClientCredential(clientId, clientSecret);
            AuthenticationContext context = new AuthenticationContext(authority, false);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(
                resource,  // the resource (app) we are going to access with the token
                clientCredential);  // the client credentials

            return authenticationResult;
        }
    }
}
