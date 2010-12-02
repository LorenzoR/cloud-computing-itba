using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TweetSharp.Model;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;


namespace SocialMediaLibrary
{
    public class TwitterInfo
    {

        public TwitterInfo()
        {
            AuthorizeAndLoadInfo();
        }

        public TwitterInfo(string CallbackUrl)
        {
            _CallbackUrl = CallbackUrl;
            AuthorizeAndLoadInfo();
        }

        #region "Properties"

        public enum TwitterInfoStatus
        {
            ExceededMaxLength,
            Success,
            Other
        }

        /// <summary>
        /// A starting point for twitter updates or requests.  Be sure to add the following using statements to your page:
        /// 
        /// using TweetSharp.Twitter;
        /// using TweetSharp.Twitter.Model;
        /// using TweetSharp.Twitter.Extensions;
        /// using TweetSharp.Twitter.Fluent;
        /// </summary>
        public IFluentTwitter Twitter
        {
            get
            {
                return FluentTwitter.CreateRequest()
                           .Configuration.UseGzipCompression()
                           .AuthenticateWith(ConsumerKey, ConsumerSecret, AccessToken.Token, AccessToken.TokenSecret);
            }
        }


        public string CookieName
        {
            get
            {
                string key = ConfigurationManager.AppSettings["TwitterCookieName"];
                if (key != null) { return key; }
                return "TwitterAuth";
            }
        }

        public string UserId
        {
            get
            {
                string key = ConfigurationManager.AppSettings["TwitterUserId"];
                if (key == null) { throw new Exception("No 'TwitterUserId' found in the AppSettings of the web.config.  You can get this by hovering over the RSS feed on your twitter home page and looking at the number in the url."); }
                return key;
            }

        }


        //The consumerkey and consumersecret need to be set after creating your twitter app
        public string ConsumerKey
        {
            get
            {
                string key = ConfigurationManager.AppSettings["TwitterConsumerKey"];
                if (key == null) { throw new Exception("No 'TwitterConsumerKey' found in the AppSettings of the web.config.  You must create a twitter application (http://dev.twitter.com/apps) and get the key from there."); }
                return key;
            }
        }

        public string ConsumerSecret
        {
            get
            {
                string key = ConfigurationManager.AppSettings["TwitterConsumerSecretKey"];
                if (key == null) { throw new Exception("No 'TwitterConsumerSecretKey' found in the AppSettings of the web.config.  You must create a twitter application (http://dev.twitter.com/apps) and get the key from there."); }
                return key;
            }
        }


        private string _CallbackUrl = "";
        public string CallbackUrl
        {
            get
            {
                string url = ConfigurationManager.AppSettings["TwitterCallbackUrl"];

                if (url == null)
                {
                    //If not callback url is entered, redirect back to the current page
                    //_CallbackUrl = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + HttpContext.Current.Request.Url.PathAndQuery;
                    _CallbackUrl = "http://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.Url.PathAndQuery;

                    //_CallbackUrl = "http://" + HttpContext.Current.Request.Url.Host + ":7777" + HttpContext.Current.Request.Url.PathAndQuery;

                }


                return _CallbackUrl;
            }
        }



        private OAuthToken _RequestToken = new OAuthToken();
        public OAuthToken RequestToken
        {
            get { return _RequestToken; }
            set
            {
                _RequestToken = value;
            }
        }

        private OAuthToken _AccessToken = new OAuthToken();
        public OAuthToken AccessToken
        {
            get { return _AccessToken; }
            set
            {
                _AccessToken = value;
            }
        }

        #endregion

        #region "LoadAuthInfo"

        public void AuthorizeAndLoadInfo()
        {
            //If the info is already saved to a cookie, load it up
            if (HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                HttpCookie AuthCookie = HttpContext.Current.Request.Cookies[CookieName];
                RequestToken.Token = AuthCookie.Values["oauth_token"];
                RequestToken.TokenSecret = AuthCookie.Values["oauth_verifier"];
                AccessToken.Token = AuthCookie.Values["access_token"];
                AccessToken.TokenSecret = AuthCookie.Values["access_token_secret"];
            }
            else
            {
                //Otherwise, authorize the user and save the info to a cookie

                string AuthUrl = GetOAuthUrl();
                string OauthToken = HttpContext.Current.Request.QueryString["oauth_token"];
                string OauthVerifier = HttpContext.Current.Request.QueryString["oauth_verifier"];
                if (OauthToken == null)
                {
                    HttpContext.Current.Response.Redirect(AuthUrl);
                }
                else
                {
                    HttpCookie AuthCookie = new HttpCookie(CookieName);
                    AuthCookie.Expires = DateTime.Now.AddYears(100);

                    AccessToken = GetAccessToken(OauthToken, OauthVerifier);
                    AuthCookie["access_token"] = AccessToken.Token;
                    AuthCookie["access_token_secret"] = AccessToken.TokenSecret;
                    AuthCookie["oauth_token"] = OauthToken;
                    AuthCookie["oauth_verifier"] = OauthVerifier;

                    RequestToken.Token = OauthToken;
                    RequestToken.TokenSecret = OauthVerifier;

                    HttpContext.Current.Response.Cookies.Add(AuthCookie);
                }

            }
        }

        private string GetOAuthUrl()
        {

            IFluentTwitter twitter;

            //Override the callback url if one was entered
            if (CallbackUrl != null && CallbackUrl.Trim().Length > 0)
            {
                twitter = FluentTwitter.CreateRequest().Configuration.UseHttps().Authentication.GetRequestToken(ConsumerKey, ConsumerSecret, CallbackUrl);
            }
            else
            {
                twitter = FluentTwitter.CreateRequest().Configuration.UseHttps().Authentication.GetRequestToken(ConsumerKey, ConsumerSecret);
            }

            OAuthToken UnauthorizedToken;
            string AuthorizationUrl;

            var response = twitter.Request();
            UnauthorizedToken = response.AsToken();


            AuthorizationUrl = FluentTwitter.CreateRequest()
                .Authentication.GetAuthorizationUrl(UnauthorizedToken.Token);

            return AuthorizationUrl;
        }

        private OAuthToken GetAccessToken(string OauthToken, string OauthVerifier)
        {
            return FluentTwitter.CreateRequest()
                .Configuration.UseHttps().Authentication.GetAccessToken(ConsumerKey, ConsumerSecret, OauthToken, OauthVerifier)
                .Request().AsToken();

        }

        #endregion

        public TwitterInfoStatus PostTweet(string tweet)
        {
            return PostTweet(tweet, false);
        }

        public TwitterInfoStatus PostTweet(string tweet, bool shortenUrls)
        {
            BitlyInfo bi = new BitlyInfo();
            if (shortenUrls) { tweet = bi.ReturnTextWithShortenedUrls(tweet); }

            if (tweet.Length > 140) { return TwitterInfoStatus.ExceededMaxLength; }

            FluentTwitter.CreateRequest().AuthenticateWith(ConsumerKey,
                                  ConsumerSecret, AccessToken.Token, AccessToken.TokenSecret)
                                  .Statuses().Update(tweet).Request();
            return TwitterInfoStatus.Success;
        }

        public IEnumerable<TwitterUser> GetUsers()
        {
            return FluentTwitter.CreateRequest()
                           .Configuration.UseGzipCompression()
                           .AuthenticateWith(ConsumerKey, ConsumerSecret, AccessToken.Token, AccessToken.TokenSecret)
                           .Users()
                           .GetFriends()
                           .AsJson().Request().AsUsers();
        }

        public IEnumerable<TwitterStatus> GetTweets()
        {
            return FluentTwitter.CreateRequest()
                           .Configuration.UseGzipCompression()
                           .AuthenticateWith(ConsumerKey, ConsumerSecret, AccessToken.Token, AccessToken.TokenSecret)
                           .Statuses().OnUserTimeline().For(UserId).AsJson().Request().AsStatuses();
        }




        public TwitterStatus GetLastTweet()
        {
            return FluentTwitter.CreateRequest()
                           .Configuration.UseGzipCompression()
                           .AuthenticateWith(ConsumerKey, ConsumerSecret, AccessToken.Token, AccessToken.TokenSecret)
                           .Statuses().OnUserTimeline().For(UserId).Take(1).AsJson().Request().AsStatuses().FirstOrDefault();

        }


    }

}

