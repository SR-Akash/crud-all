using CRUD_API.IRepository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_API.Repository
{
    public class APICallfromOtherApplication : IAPICallfromOtherApplication
    {
        public async Task<string> Create()
        {

            // Replace these values with your actual BioTime API endpoint, client ID, and client secret
            string apiEndpoint = "http://127.0.0.1:8090";
            string clientId = "IMRAN-5740";
            string clientSecret = "Imran1234@";

            // Step 1: Get Access Token
            string accessToken = await GetAccessToken(apiEndpoint, clientId, clientSecret);

            Console.WriteLine(accessToken);

            if (!string.IsNullOrEmpty(accessToken))
            {
                // Step 2: Use Access Token to make a request to another API endpoint
                string otherEndpoint = "/iclock/api/transactions/";
                await MakeApiRequest(apiEndpoint + otherEndpoint, accessToken);
            }

            return "OK";
        }

        private static async Task<string> GetAccessToken(string apiEndpoint, string clientId, string clientSecret)
        {
            using (HttpClient client = new HttpClient())
            {
                string tokenEndpoint = "/jwt-api-token-auth/";
                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

                // Construct the request body as a JSON string
                string requestBody = "{\"grant_type\":\"password\", \"username\":\"IMRAN-5740\", \"password\":\"Imran1234@\"}";

                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, apiEndpoint + tokenEndpoint);
                tokenRequest.Headers.Add("Authorization", "Basic " + credentials);
                tokenRequest.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                HttpResponseMessage tokenResponse = await client.SendAsync(tokenRequest);

                if (tokenResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        // Parse the token response to get the access token

                        string responseContent = await tokenResponse.Content.ReadAsStringAsync();
                        return responseContent;
                        //var tokenData = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(responseContent);
                        //return tokenData?.AccessToken;
                    }
                    catch (Exception ex)
                    {
                        // Print the response content in case of an error
                        string errorContent = await tokenResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error obtaining access token: {tokenResponse.StatusCode}, {errorContent}");
                        return null;
                    }
                }
                else
                {
                    // Print the response content in case of an error
                    string errorContent = await tokenResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error obtaining access token: {tokenResponse.StatusCode}, {errorContent}");
                    return null;
                }
            }
        }

        private static async Task MakeApiRequest(string apiEndpoint, string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                // Make a request to the specified API endpoint using the access token
                string jsonString = accessToken;

                // Parse the JSON string
                JObject jsonObject = JObject.Parse(jsonString);

                // Extract the value associated with the key "token"
                string tokenValue = jsonObject["token"].ToString();

                client.DefaultRequestHeaders.Add("Authorization", "JWT " + tokenValue);

                HttpResponseMessage response = await client.GetAsync(apiEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    // Parse and handle the response data
                    string responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseData);
                }
                else
                {
                    Console.WriteLine($"Error making API request: {response.StatusCode}");
                }
            }
        }

        // Define a class to represent the token response structure
        private class TokenResponse
        {
            //[JsonProperty("token")]
            public string AccessToken { get; set; }
        }
    }
}