﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// Enumeration of the supported HTTP verbs supported by the <see cref="Twitterizer.Core.CommandPerformer{T}"/>
    /// </summary>
    public enum HTTPVerb
    {
        /// <summary>
        /// The HTTP GET method is used to retrieve data.
        /// </summary>
        GET,

        /// <summary>
        /// The HTTP POST method is used to transmit data.
        /// </summary>
        POST,

        /// <summary>
        /// The HTTP DELETE method is used to indicate that a resource should be deleted.
        /// </summary>
        DELETE
    }

    /// <include file='OAuthUtility.xml' path='OAuthUtility/OAuthUtility/*'/>
    public static class OAuthUtility
    {
        /// <summary>
        /// The name of the signature type twiter uses.
        /// </summary>
        private const string SignatureType = "HMAC-SHA1";

        #region Public Methods
        /// <summary>
        /// Gets a new OAuth request token from the twitter api.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <returns>
        /// A new <see cref="Twitterizer.OAuthTokenResponse"/> instance.
        /// </returns>
        public static OAuthTokenResponse GetRequestToken(string consumerKey, string consumerSecret)
        {
            return GetRequestToken(consumerKey, consumerSecret, string.Empty);
        }

        public static OAuthTokenResponse GetRequestToken(string consumerKey, string consumerSecret, string callbackAddress)
        {
            return GetRequestToken(consumerKey, consumerSecret, callbackAddress, null);
        }

        /// <summary>
        /// Gets a new OAuth request token from the twitter api.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="callbackAddress">Address of the callback.</param>
        /// <returns>
        /// A new <see cref="Twitterizer.OAuthTokenResponse"/> instance.
        /// </returns>
        public static OAuthTokenResponse GetRequestToken(string consumerKey, string consumerSecret, string callbackAddress, WebProxy proxy)
        {
            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException("consumerKey");
            }

            if (string.IsNullOrEmpty(consumerSecret))
            {
                throw new ArgumentNullException("consumerSecret");
            }

            OAuthTokenResponse response = new OAuthTokenResponse();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(callbackAddress))
            {
                parameters.Add("oauth_callback", callbackAddress);
            }

            try
            {
                HttpWebResponse webResponse = ExecuteRequest(
                    "https://api.twitter.com/oauth/request_token",
                    parameters,
                    HTTPVerb.POST,
                    consumerKey,
                    consumerSecret,
                    null,
                    null,
                    proxy);

                string responseBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

                Match matchedValues = Regex.Match(responseBody, @"oauth_token=(?<token>[^&]+)|oauth_token_secret=(?<secret>[^&]+)|oauth_verifier=(?<verifier>[^&]+)");

                response.Token = matchedValues.Groups["token"].Value;
                response.TokenSecret = matchedValues.Groups["secret"].Value;
                response.VerificationString = matchedValues.Groups["verifier"].Value;
            }
            catch (WebException wex)
            {
                throw new Exception(wex.Message, wex);
            }

            return response;
        }

        /// <summary>
        /// Gets the access token from pin.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="requestToken">The request token.</param>
        /// <param name="verifier">The pin number or verifier string.</param>
        /// <returns>
        /// An <see cref="OAuthTokenResponse"/> class containing access token information.
        /// </returns>
        public static OAuthTokenResponse GetAccessToken(string consumerKey, string consumerSecret, string requestToken, string verifier)
        {
            return GetAccessToken(consumerKey, consumerSecret, requestToken, verifier, null);
        }

        /// <summary>
        /// Gets the access token from pin.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="requestToken">The request token.</param>
        /// <param name="verifier">The pin number or verifier string.</param>
        /// <param name="proxy">The proxy.</param>
        /// <returns>
        /// An <see cref="OAuthTokenResponse"/> class containing access token information.
        /// </returns>
        public static OAuthTokenResponse GetAccessToken(string consumerKey, string consumerSecret, string requestToken, string verifier, WebProxy proxy)
        {
            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException("consumerKey");
            }

            if (string.IsNullOrEmpty(consumerSecret))
            {
                throw new ArgumentNullException("consumerSecret");
            }

            if (string.IsNullOrEmpty(requestToken))
            {
                throw new ArgumentNullException("requestToken");
            }

            OAuthTokenResponse response = new OAuthTokenResponse();

            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(verifier))
                {
                    parameters.Add("oauth_verifier", verifier);
                }

                HttpWebResponse webResponse = ExecuteRequest(
                    "https://api.twitter.com/oauth/access_token",
                    parameters,
                    HTTPVerb.POST,
                    consumerKey,
                    consumerSecret,
                    requestToken,
                    string.Empty,
                    proxy);

                string responseBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

                response.Token = Regex.Match(responseBody, @"oauth_token=([^&]+)").Groups[1].Value;
                response.TokenSecret = Regex.Match(responseBody, @"oauth_token_secret=([^&]+)").Groups[1].Value;
                response.UserId = long.Parse(Regex.Match(responseBody, @"user_id=([^&]+)").Groups[1].Value, CultureInfo.CurrentCulture);
                response.ScreenName = Regex.Match(responseBody, @"screen_name=([^&]+)").Groups[1].Value;
            }
            catch (WebException wex)
            {
                throw new Exception(wex.Message, wex);
            }

            return response;
        }

        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Return type is not a URL.")]
        public static string EncodeForUrl(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            value = HttpUtility.UrlEncode(value).Replace("+", "%20");

            // UrlEncode escapes with lowercase characters (e.g. %2f) but oAuth needs %2F
            value = Regex.Replace(value, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());

            // these characters are not escaped by UrlEncode() but needed to be escaped
            value = value
                .Replace("(", "%28")
                .Replace(")", "%29")
                .Replace("$", "%24")
                .Replace("!", "%21")
                .Replace("*", "%2A")
                .Replace("'", "%27");

            // these characters are escaped by UrlEncode() but will fail if unescaped!
            value = value.Replace("%7E", "~");

            return value;
        }
        #endregion

        /// <summary>
        /// Builds the authorization URI.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        /// <returns>A new <see cref="Uri"/> instance.</returns>
        public static Uri BuildAuthorizationUri(string requestToken)
        {
            return BuildAuthorizationUri(requestToken, false);
        }

        /// <summary>
        /// Builds the authorization URI.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        /// <param name="authenticate">if set to <c>true</c>, the authenticate url will be used. (See: "Sign in with Twitter")</param>
        /// <returns>A new <see cref="Uri"/> instance.</returns>
        public static Uri BuildAuthorizationUri(string requestToken, bool authenticate)
        {
            StringBuilder parameters = new StringBuilder("https://twitter.com/oauth/");

            if (authenticate)
            {
                parameters.Append("authenticate");
            }
            else
            {
                parameters.Append("authorize");
            }

            parameters.AppendFormat("?oauth_token={0}", requestToken);

            return new Uri(parameters.ToString());
        }

        /// <summary>
        /// Gets the access token during callback.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <returns>
        /// Access tokens returned by the Twitter API
        /// </returns>
        public static OAuthTokenResponse GetAccessTokenDuringCallback(string consumerKey, string consumerSecret)
        {
            HttpContext context = HttpContext.Current;
            if (context == null || context.Request == null)
            {
                throw new ApplicationException("Could not located the HTTP context. GetAccessTokenDuringCallback can only be used in ASP.NET applications.");
            }

            string requestToken = context.Request.QueryString["oauth_token"];
            string verifier = context.Request.QueryString["oauth_verifier"];

            if (string.IsNullOrEmpty(requestToken))
            {
                throw new ApplicationException("Could not locate the request token.");
            }

            if (string.IsNullOrEmpty(verifier))
            {
                throw new ApplicationException("Could not locate the verifier value.");
            }

            return GetAccessToken(consumerKey, consumerSecret, requestToken, verifier);
        }

        internal static HttpWebResponse ExecuteRequest(
            string baseUrl,
            string fileParameterName,
            string filename,
            byte[] fileData,
            string mimeType,
            Dictionary<string, string> parameters,
            string consumerKey,
            string consumerSecret,
            string token,
            string tokenSecret,
            WebProxy proxy)
        {
            Dictionary<string, string> combinedParameters = PrepareOAuthParameters(
                baseUrl,
                parameters,
                HTTPVerb.POST,
                consumerKey,
                consumerSecret,
                token,
                tokenSecret);

            baseUrl = AppendParametersForPOST(baseUrl, combinedParameters);

            // Build the POST body.
            string boundaryTicks = DateTime.Now.Ticks.ToString("x");
            string contentBoundary = string.Format("--{0}\r\n", boundaryTicks);
            string endBoundary = string.Format("--{0}--\r\n", boundaryTicks);

            string contentDisposition = string.Format(
                "Content-Disposition: form-data;name=\"image\";filename=\"{0}\"\r\nContent-Type: {1}\r\n\r\n",
                filename,
                mimeType);

            byte[] formData = Encoding.UTF8.GetBytes(
                string.Format(
                    "{0}{1}{2}\r\n{3}",
                    contentBoundary,
                    contentDisposition,
                    Encoding.Default.GetString(fileData),
                    endBoundary)
                );

            // Create and setup the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
            request.Method = "POST";
            request.UserAgent = string.Format(CultureInfo.InvariantCulture, "Twitterizer");
            request.Headers.Add("Authorization", GenerateAuthorizationHeader(combinedParameters));
            request.ContentType = string.Concat("multipart/form-data;boundary=", boundaryTicks);
            request.AllowWriteStreamBuffering = true;
            request.ContentLength = formData.Length;

            if (proxy != null)
                request.Proxy = proxy;

            // Stream the form data to the request
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(formData, 0, formData.Length);
                reqStream.Flush();
            }

#if DEBUG
            Console.WriteLine("----- Headers -----");
            foreach (string key in request.Headers.AllKeys)
            {
                Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} = {1}\n", key, request.Headers[key]));
            }

            Console.WriteLine("----- End Of Headers -----");
#endif
            return (HttpWebResponse)request.GetResponse();
        }

        /// <summary>
        /// Creates and executes an OAuth signed HTTP request.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="verb">The HTTP verb to perform.</param>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="token">The access or request token.</param>
        /// <param name="tokenSecret">The token secret.</param>
        /// <returns>
        /// A new instance of the <see cref="System.Net.HttpWebRequest"/> class.
        /// </returns>
        internal static HttpWebResponse ExecuteRequest(
            string baseUrl,
            Dictionary<string, string> parameters,
            HTTPVerb verb,
            string consumerKey,
            string consumerSecret,
            string token,
            string tokenSecret,
            WebProxy proxy)
        {
            Dictionary<string, string> combinedParameters = PrepareOAuthParameters(
                baseUrl,
                parameters,
                verb,
                consumerKey,
                consumerSecret,
                token,
                tokenSecret);

            HttpWebResponse response;

            if (verb == HTTPVerb.POST)
            {
                baseUrl = AppendParametersForPOST(baseUrl, combinedParameters);
            }
            else
            {
                string querystring = GenerateGetQueryString(combinedParameters);
                if (!string.IsNullOrEmpty(querystring))
                {
                    baseUrl = string.Concat(baseUrl, "?", querystring);
                }
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
            request.Method = verb.ToString();
            request.UserAgent = string.Format(CultureInfo.InvariantCulture, "Twitterizer");
            request.Headers.Add("Authorization", GenerateAuthorizationHeader(combinedParameters));

            if (proxy != null)
                request.Proxy = proxy;

            if (verb == HTTPVerb.POST)
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

#if DEBUG
            Console.WriteLine("----- Headers -----");
            foreach (string key in request.Headers.AllKeys)
            {
                Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} = {1}\n", key, request.Headers[key]));
            }

            Console.WriteLine("----- End Of Headers -----");
#endif
            response = (HttpWebResponse)request.GetResponse();

            return response;
        }

        private static string AppendParametersForPOST(string baseUrl, Dictionary<string, string> combinedParameters)
        {
            StringBuilder requestParametersBuilder = new StringBuilder();

            foreach (KeyValuePair<string, string> item in combinedParameters.Where(p => !p.Key.Contains("oauth_") || p.Key == "oauth_verifier"))
            {
                if (requestParametersBuilder.Length > 0)
                {
                    requestParametersBuilder.Append("&");
                }

                requestParametersBuilder.AppendFormat(
                    "{0}={1}",
                    item.Key,
                    EncodeForUrl(item.Value));
            }

            if (requestParametersBuilder.Length > 0)
                baseUrl = string.Concat(baseUrl, "?", requestParametersBuilder.ToString());

            return baseUrl;
        }

        /// <summary>
        /// Prepares the OAuth parameters.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="token">The token.</param>
        /// <param name="tokenSecret">The token secret.</param>
        /// <returns>
        /// 
        /// </returns>
        private static Dictionary<string, string> PrepareOAuthParameters(string baseUrl, Dictionary<string, string> parameters, HTTPVerb verb, string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            Dictionary<string, string> combinedParameters = new Dictionary<string, string>();

            if (parameters != null)
            {
                // Copy the given parameters into a new collection, as to not modify the source collection
                foreach (KeyValuePair<string, string> item in parameters)
                {
                    combinedParameters.Add(item.Key, item.Value);
                }
            }

            // Add the OAuth parameters
            combinedParameters.Add("oauth_version", "1.0");
            combinedParameters.Add("oauth_nonce", GenerateNonce());
            combinedParameters.Add("oauth_timestamp", GenerateTimeStamp());
            combinedParameters.Add("oauth_signature_method", "HMAC-SHA1");
            combinedParameters.Add("oauth_consumer_key", consumerKey);
            combinedParameters.Add("oauth_consumer_secret", consumerSecret);

            if (!string.IsNullOrEmpty(token))
            {
                combinedParameters.Add("oauth_token", token);
            }

            if (!string.IsNullOrEmpty(tokenSecret))
            {
                combinedParameters.Add("oauth_token_secret", tokenSecret);
            }

            AddSignatureToParameters(
                new Uri(baseUrl),
                combinedParameters,
                verb,
                consumerSecret,
                tokenSecret);

            return combinedParameters;
        }

        /// <summary>
        /// Generates the query string for HTTP GET requests.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// A string of all parameters prepared for use in a querystring.
        /// </returns>
        private static string GenerateGetQueryString(Dictionary<string, string> parameters)
        {
            StringBuilder queryStringBuilder = new StringBuilder();
            foreach (var item in from p in parameters
                                 where !p.Key.Contains("oauth_")
                                 orderby p.Key, p.Value
                                 select p)
            {
                if (queryStringBuilder.Length > 0)
                {
                    queryStringBuilder.Append("&");
                }

                queryStringBuilder.AppendFormat(
                    "{0}={1}",
                    item.Key,
                    EncodeForUrl(item.Value));
            }

            return queryStringBuilder.ToString();
        }

        /// <summary>
        /// Generates the authorization header.
        /// </summary>
        /// <param name="newParameters">The new parameters.</param>
        /// <returns>A string value of all OAuth parameters formatted for use in the Authorization HTTP header.</returns>
        private static string GenerateAuthorizationHeader(Dictionary<string, string> newParameters)
        {
            StringBuilder authHeaderBuilder = new StringBuilder("OAuth realm=\"Twitter API\"");

            foreach (var item in newParameters
                .Where(p => p.Key.Contains("oauth_") &&
                    !p.Key.EndsWith("_secret", StringComparison.OrdinalIgnoreCase) &&
                    p.Key != "oauth_signature" &&
                    p.Key != "oauth_verifier" &&
                    !string.IsNullOrEmpty(p.Value))
                .OrderBy(p => p.Key)
                .ThenBy(p => EncodeForUrl(p.Value)))
            {
                authHeaderBuilder.AppendFormat(
                    ",{0}=\"{1}\"",
                    EncodeForUrl(item.Key),
                    EncodeForUrl(item.Value));
            }

            authHeaderBuilder.AppendFormat(",oauth_signature=\"{0}\"", EncodeForUrl(newParameters["oauth_signature"]));

#if DEBUG
            System.Diagnostics.Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "OAUTH HEADER: {0}", authHeaderBuilder.ToString()));
#endif
            string authorizationHeader = authHeaderBuilder.ToString();
            return authorizationHeader;
        }

        /// <summary>
        /// Generate the timestamp for the signature        
        /// </summary>
        /// <returns>A timestamp value in a string.</returns>
        private static string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns>A random number between 123400 and 9999999 in a string.</returns>
        private static string GenerateNonce()
        {
            // Just a simple implementation of a random number between 123400 and 9999999
            return new Random()
                .Next(123400, int.MaxValue)
                .ToString("X", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Generates and adds a signature to parameters.
        /// </summary>
        /// <param name="url">The base URL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="verb">The HTTP verb to perform.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="tokenSecret">The token secret.</param>
        private static void AddSignatureToParameters(
            Uri url,
            Dictionary<string, string> parameters,
            HTTPVerb verb,
            string consumerSecret,
            string tokenSecret)
        {
            string normalizedUrl = NormalizeUrl(url);

            // Get the oauth parameters from the parameters
            Dictionary<string, string> baseStringParameters =
                (from p in parameters
                 where !(p.Key.EndsWith("_secret", StringComparison.OrdinalIgnoreCase) &&
                    p.Key.StartsWith("oauth_", StringComparison.OrdinalIgnoreCase) &&
                    !p.Key.EndsWith("_verifier", StringComparison.OrdinalIgnoreCase))
                 select p).ToDictionary(p => p.Key, p => p.Value);

            string signatureBase = string.Format(
                CultureInfo.InvariantCulture,
                "{0}&{1}&{2}",
                verb.ToString().ToUpper(CultureInfo.InvariantCulture),
                EncodeForUrl(normalizedUrl),
                UrlEncode(baseStringParameters));

            HMACSHA1 hmacsha1 = new HMACSHA1();

            string key = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}&{1}",
                    EncodeForUrl(consumerSecret),
                    EncodeForUrl(tokenSecret));

            hmacsha1.Key = Encoding.ASCII.GetBytes(key);

            string result = Convert.ToBase64String(
                hmacsha1.ComputeHash(
                    Encoding.ASCII.GetBytes(signatureBase)));

            // Add the signature to the oauth parameters
            parameters.Add("oauth_signature", result);

#if DEBUG
            System.Diagnostics.Debug.WriteLine("----------- OAUTH SIGNATURE GENERATION -----------");
            System.Diagnostics.Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "url.PathAndQuery = \"{0}\"", url.PathAndQuery));
            System.Diagnostics.Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "httpMethod = \"{0}\"", verb.ToString()));
            System.Diagnostics.Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "consumerSecret = \"{0}\"", consumerSecret));
            System.Diagnostics.Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "tokenSecret = \"{0}\"", tokenSecret));
            System.Diagnostics.Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "normalizedUrl = \"{0}\"", normalizedUrl));
            System.Diagnostics.Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "signatureBase = \"{0}\"", signatureBase));
            System.Diagnostics.Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "key = \"{0}\"", key));
            System.Diagnostics.Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "signature = \"{0}\"", result));
            System.Diagnostics.Debug.WriteLine("--------- END OAUTH SIGNATURE GENERATION ----------");
#endif
        }

        /// <summary>
        /// Normalizes the URL.
        /// </summary>
        /// <param name="url">The URL to normalize.</param>
        /// <returns>The normalized url string.</returns>
        private static string NormalizeUrl(Uri url)
        {
            string normalizedUrl = string.Format(CultureInfo.InvariantCulture, "{0}://{1}", url.Scheme, url.Host);
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
            {
                normalizedUrl += ":" + url.Port;
            }

            normalizedUrl += url.AbsolutePath;
            return normalizedUrl;
        }

        /// <summary>
        /// URLs the encode.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A string of all the <paramref name="parameters"/> keys and value pairs with the values encoded.</returns>
        private static string UrlEncode(Dictionary<string, string> parameters)
        {
            StringBuilder parameterString = new StringBuilder();

            var paramsSorted = from p in parameters
                               orderby p.Key, p.Value
                               select p;

            foreach (var item in paramsSorted)
            {
                if (parameterString.Length > 0)
                {
                    parameterString.Append("&");
                }

                parameterString.Append(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}={1}",
                        EncodeForUrl(item.Key),
                        EncodeForUrl(item.Value)));
            }

            return EncodeForUrl(parameterString.ToString());
        }
    }
}