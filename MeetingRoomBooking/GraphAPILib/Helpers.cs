using MeetingRoomBooking.TokenStorage;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MeetingRoomBooking.GraphAPILib
{
    public class Helpers
    {
        public static async Task<string> GetAccessToken(HttpContextBase HttpContext)
        {
            string accessToken = null;

            // Load the app config from web.config
            string appId = ConfigurationManager.AppSettings["ida:AppId"];
            string appPassword = ConfigurationManager.AppSettings["ida:AppPassword"];
            string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
            string[] scopes = ConfigurationManager.AppSettings["ida:AppScopes"]
                .Replace(' ', ',').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Get the current user's ID
            string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!string.IsNullOrEmpty(userId))
            {
                // Get the user's token cache
                SessionTokenCache tokenCache = new SessionTokenCache(userId, HttpContext);

                ConfidentialClientApplication cca = new ConfidentialClientApplication(
                    appId, redirectUri, new ClientCredential(appPassword), tokenCache.GetMsalCacheInstance(), null);

                // Call AcquireTokenSilentAsync, which will return the cached
                // access token if it has not expired. If it has expired, it will
                // handle using the refresh token to get a new one.
                AuthenticationResult result = await cca.AcquireTokenSilentAsync(scopes, cca.Users.First());

                accessToken = result.AccessToken;
            }

            return accessToken;
        }

        public static async Task<string> GetUserEmail(HttpContextBase HttpContext)
        {
            GraphServiceClient client = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        string accessToken = await GraphAPILib.Helpers.GetAccessToken(HttpContext);
                        requestMessage.Headers.Authorization =
                            new AuthenticationHeaderValue("Bearer", accessToken);
                    }));

            // Get the user's email address
            try
            {
                Microsoft.Graph.User user = await client.Me.Request().GetAsync();
                return user.Mail;
            }
            catch (ServiceException ex)
            {
                return string.Format("#ERROR#: Could not get user's email address. {0}", ex.Message);
            }
        }

        public const string GrahQueryUrl = "https://graph.microsoft.com/v1.0/me/calendars/AAMkADA0NDU2M2Q3LWNmYmYtNDQxOS1hMGNiLTQ5MzZmNDE4N2E4YQBGAAAAAADH6Gkr6C_wQZBa2ATY99PWBwDwBEJQvRi-TIFWAWJVJieuAAAAAAEGAADwBEJQvRi-TIFWAWJVJieuAAAFo3SGAAA=/events";
        public static async Task<bool> AddEvent(HttpContextBase httpContext, Models.BaseEventRequestBody eventRequestBody)
        {
            string token = await GetAccessToken(httpContext);

            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, GrahQueryUrl);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string json = JsonConvert.SerializeObject(eventRequestBody);

            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(requestMessage);

            var statusCode = response.IsSuccessStatusCode;

            string respJson = await response.Content.ReadAsStringAsync();
            dynamic test = JsonConvert.DeserializeObject(respJson);

            return statusCode;
        }

        public static async Task CreateEvent(HttpContextBase httpContext, Models.BaseEventRequestBody eventRequestBody)
        {
            await AddEvent(httpContext, eventRequestBody);
        }
    }
}