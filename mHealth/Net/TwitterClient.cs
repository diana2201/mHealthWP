using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestTwitter.Models;
using TestTwitter.Util;

namespace TestTwitter.Net
{
    public class TwitterClient
    {

        //CallBack
        public interface TwitterClientI {
            void userTweets(List<Tweet> tweets);
        }
        
        //OAuth
        private const string twitterRequestTokenUrl = "http://api.twitter.com/oauth/request_token";
        private const string twitterAccessTokenUrl = "http://api.twitter.com/oauth/access_token";
        private const string twitterAuthorizeUrl = "http://api.twitter.com/oauth/authorize";
        private const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";

        //API REST
        private String user_timeline = "https://api.twitter.com/1.1/statuses/user_timeline.json";
        private String user_timeline_params = "screen_name={0}&count={1}";

        private String update_state = "https://api.twitter.com/1.1/statuses/update.json";
        private String update_state_params = "status={0}";

        //Atributos
        public String ConsumerKey {get; set;} 
        public String ConsumerSecret {get; set;}
        public String AccessToken {get; set;}
        public String AccessTokenSecret {get; set;}
        public String OAuthVerifier{ get; set;}

        private String requestToken = null;
        private String requestTokenSecret = null;
        private String requestAuthorizeUrl = null;

        HttpClient client;
        OAuthUtil oAuthUtil;

        private TwitterClientI twitterClientI;

        public TwitterClient()
        {
            
            oAuthUtil = new OAuthUtil();
        }

        #region Token Methods
        
        private async void getRequestToken() {
            string nonce = oAuthUtil.GetNonce();
            string timeStamp = oAuthUtil.GetTimeStamp();
            string sigBaseStringParams = "oauth_consumer_key=" + ConsumerKey;
            sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
            sigBaseStringParams += "&" + "oauth_signature_method=" + "HMAC-SHA1";
            sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;
            sigBaseStringParams += "&" + "oauth_version=1.0";

            string sigBaseString = "POST&";
            sigBaseString += Uri.EscapeDataString(twitterRequestTokenUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);

            string signature = oAuthUtil.GetSignature(sigBaseString, ConsumerSecret);

            var responseText = await oAuthUtil.PostData(twitterRequestTokenUrl, sigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(signature));

            if (!string.IsNullOrEmpty(responseText))
            {
                string[] keyValPairs = responseText.Split('&');

                for (int i = 0; i < keyValPairs.Length; i++)
                {
                    String[] splits = keyValPairs[i].Split('=');
                    switch (splits[0])
                    {
                        case "oauth_token":
                            requestToken = splits[1];
                            break;
                        case "oauth_token_secret":
                            requestTokenSecret = splits[1];
                            break;
                        case "xoauth_request_auth_url":
                            requestAuthorizeUrl = splits[1];
                            break;
                    }
                }
            }
        }


        private async void getAccessToken()
        {
            string nonce = oAuthUtil.GetNonce();
            string timeStamp = oAuthUtil.GetTimeStamp();

            string sigBaseStringParams = "oauth_consumer_key=" + ConsumerKey;
            sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
            sigBaseStringParams += "&" + "oauth_signature_method=" + "HMAC-SHA1";
            sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;
            sigBaseStringParams += "&" + "oauth_token=" + requestToken;
            sigBaseStringParams += "&" + "oauth_verifier=" + OAuthVerifier;
            sigBaseStringParams += "&" + "oauth_version=1.0";
            string sigBaseString = "POST&";
            sigBaseString += Uri.EscapeDataString(twitterAccessTokenUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);

            string signature = oAuthUtil.GetSignature(sigBaseString, ConsumerSecret, requestTokenSecret);

            var responseText = await oAuthUtil.PostData(twitterAccessTokenUrl, sigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(signature));

            if (!string.IsNullOrEmpty(responseText))
            {
                string oauth_token = null;
                string oauth_token_secret = null;
                string[] keyValPairs = responseText.Split('&');

                for (int i = 0; i < keyValPairs.Length; i++)
                {
                    String[] splits = keyValPairs[i].Split('=');
                    switch (splits[0])
                    {
                        case "oauth_token":
                            oauth_token = splits[1];
                            break;
                        case "oauth_token_secret":
                            oauth_token_secret = splits[1];
                            break;
                    }
                }

                AccessToken = oauth_token;
                AccessToken = oauth_token_secret;
            }
        }

        private string getParamsBaseString(string queryParamsString, string nonce, string timeStamp)
        {
            // these parameters are required in every request api
            var baseStringParams = new Dictionary<string, string>{
                {"oauth_consumer_key", ConsumerKey},
                {"oauth_nonce", nonce},
                {"oauth_signature_method", "HMAC-SHA1"},
                {"oauth_timestamp", timeStamp},
                {"oauth_token", AccessToken},
                {"oauth_verifier", OAuthVerifier},
                {"oauth_version", "1.0"},                
            };

            // put each parameter into dictionary
            var queryParams = queryParamsString
                                .Split('&')
                                .ToDictionary(p => p.Substring(0, p.IndexOf('=')), p => p.Substring(p.IndexOf('=') + 1));

            foreach (var kv in queryParams)
            {
                baseStringParams.Add(kv.Key, kv.Value);
            }

            // The OAuth spec says to sort lexigraphically, which is the default alphabetical sort for many libraries.
            var ret = baseStringParams
                .OrderBy(kv => kv.Key)
                .Select(kv => kv.Key + "=" + kv.Value)
                .Aggregate((i, j) => i + "&" + j);

            return ret;
        }

        #endregion

        #region Api Methods
        public async Task<String> requestTwitterApiGet(string url, string qParams)
        {
            string nonce = oAuthUtil.GetNonce();
            string timeStamp = oAuthUtil.GetTimeStamp();

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.MaxResponseContentBufferSize = int.MaxValue;
                httpClient.DefaultRequestHeaders.ExpectContinue = false;
                HttpRequestMessage requestMsg = new HttpRequestMessage();
                requestMsg.Method = new HttpMethod("GET");

                var urlWithParams = url + "?" + qParams;
                requestMsg.RequestUri = new Uri(urlWithParams);

                string paramsBaseString = getParamsBaseString(qParams, nonce, timeStamp);

                string sigBaseString = "GET&";
                // signature base string uses base url
                sigBaseString += Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(paramsBaseString);

                string signature = oAuthUtil.GetSignature(sigBaseString, ConsumerSecret, AccessTokenSecret);
                string data = "oauth_consumer_key=\"" + ConsumerKey
                              +
                              "\", oauth_nonce=\"" + nonce +
                              "\", oauth_signature=\"" + Uri.EscapeDataString(signature) +
                              "\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"" + timeStamp +
                              "\", oauth_token=\"" + AccessToken +
                              "\", oauth_verifier=\"" + OAuthVerifier +
                              "\", oauth_version=\"1.0\"";
                requestMsg.Headers.Authorization = new AuthenticationHeaderValue("OAuth", data);
                var response = await httpClient.SendAsync(requestMsg);
                var text = await response.Content.ReadAsStringAsync();

                return text;
                
            }
            catch (Exception Err)
            {
                return null;
            }
        }

        public async Task<String> requestTwitterApiPost(string url, string qParams)
        {
            string nonce = oAuthUtil.GetNonce();
            string timeStamp = oAuthUtil.GetTimeStamp();

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.MaxResponseContentBufferSize = int.MaxValue;
                httpClient.DefaultRequestHeaders.ExpectContinue = false;
                HttpRequestMessage requestMsg = new HttpRequestMessage();
                requestMsg.Method = new HttpMethod("POST");

                var urlWithParams = url + "?" + qParams;
                requestMsg.RequestUri = new Uri(urlWithParams);

                string paramsBaseString = getParamsBaseString(qParams, nonce, timeStamp);

                string sigBaseString = "POST&";
                // signature base string uses base url
                sigBaseString += Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(paramsBaseString);

                string signature = oAuthUtil.GetSignature(sigBaseString, ConsumerSecret, AccessTokenSecret);
                string data = "oauth_consumer_key=\"" + ConsumerKey
                              +
                              "\", oauth_nonce=\"" + nonce +
                              "\", oauth_signature=\"" + Uri.EscapeDataString(signature) +
                              "\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"" + timeStamp +
                              "\", oauth_token=\"" + AccessToken +
                              "\", oauth_verifier=\"" + OAuthVerifier +
                              "\", oauth_version=\"1.0\"";
                requestMsg.Headers.Authorization = new AuthenticationHeaderValue("OAuth", data);
                var response = await httpClient.SendAsync(requestMsg);
                var text = await response.Content.ReadAsStringAsync();
                return text;
                
                
            }
            catch (Exception Err)
            {
                return null;
            }
        }

        public async void postTweet(String msg) {

            msg = msg.Replace(" ","%20");
            String rta = await requestTwitterApiPost(update_state, String.Format(update_state_params, new String[] { msg }));



        }

        public async void getTweets(String screenName, String count, TwitterClientI twitterClientI) {
            
            this.twitterClientI = twitterClientI;

            String rta = await requestTwitterApiGet(user_timeline, String.Format(user_timeline_params, new String[]{screenName, count}));
            JArray array = JArray.Parse(rta);

            List<Tweet> data = new List<Tweet>();

            foreach (JObject o in array.Children<JObject>()) {
                Tweet t = new Tweet();

                t.CreateAt = DateTime.ParseExact((String)o["created_at"], format, CultureInfo.InvariantCulture);
                t.Id = (String)o["id_str"];
                t.Text = (String)o["text"];
                t.UserScreenName = (String)o["user"]["screen_name"];
                t.UserName = (String)o["user"]["name"];
                t.UserBackgroundImageUrl = (String)o["user"]["profile_background_image_url"];
                t.UserProfileImageUrl = (String)o["user"]["profile_image_url"];

                List<String> hashtags = new List<string>();
                List<String> urls = new List<string>();
                //List<String> userMetions = new List<string>();

                foreach (JObject h in ((JArray)o["entities"]["hashtags"]).Children<JObject>())
                {
                    hashtags.Add((String)h["text"]);
                }

                foreach (JObject u in ((JArray)o["entities"]["urls"]).Children<JObject>())
                {
                    urls.Add((String)u["url"]);
                }

                t.Hashtags = hashtags;
                t.Urls = urls;

                data.Add(t);
            }

            this.twitterClientI.userTweets(data);

        }

        #endregion




    }
}
