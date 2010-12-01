using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace SocialMediaLibrary
{
    public class BitlyInfo
    {

        public string LoginName
        {
            get
            {
                string key = ConfigurationManager.AppSettings["BitlyLogin"];
                if (key == null) { throw new Exception("No 'BitlyLogin' found in the AppSettings of the web.config."); }
                return key;
            }
        }

        public string ApiKey
        {
            get
            {
                string key = ConfigurationManager.AppSettings["BitlyApiKey"];
                if (key == null) { throw new Exception("No 'BitlyApiKey' found in the AppSettings of the web.config.  After logging in to your account, you can get the key at http://bit.ly/a/your_api_key."); }
                return key;
            }
        }

        public string ShortenUrl(string LongUrl)
        {
            string result = "";

            string RequestUrl = "http://api.bit.ly/v3/shorten?login=" + LoginName + "&apiKey=" + ApiKey + "&longUrl=" + HttpUtility.UrlEncode(LongUrl) + "&format=txt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(RequestUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader ResponseStream = new StreamReader(response.GetResponseStream());
            result = ResponseStream.ReadToEnd().Replace("\n", "");
            return result;
        }

        public List<String> GetUrls(string Text)
        {
            List<String> Urls = new List<String>();
            //from regexlib.com
            Regex UrlMatcher = new Regex(@"([\d\w-.]+?\.(a[cdefgilmnoqrstuwz]|b[abdefghijmnorstvwyz]|c[acdfghiklmnoruvxyz]|d[ejkmnoz]|e[ceghrst]|f[ijkmnor]|g[abdefghilmnpqrstuwy]|h[kmnrtu]|i[delmnoqrst]|j[emop]|k[eghimnprwyz]|l[abcikrstuvy]|m[acdghklmnopqrstuvwxyz]|n[acefgilopruz]|om|p[aefghklmnrstwy]|qa|r[eouw]|s[abcdeghijklmnortuvyz]|t[cdfghjkmnoprtvwz]|u[augkmsyz]|v[aceginu]|w[fs]|y[etu]|z[amw]|aero|arpa|biz|com|coop|edu|info|int|gov|mil|museum|name|net|org|pro)(\b|\W(?<!&|=)(?!\.\s|\.{3}).*?))(\s|$)");
            MatchCollection Matches = UrlMatcher.Matches(Text);
            foreach (Match m in Matches)
            {
                string url = m.Value;
                Urls.Add(url);
            }

            return Urls;
        }

        /// <summary>
        /// Search through the string and find all urls in the string, send a request to shorten the urls, and then return the result with shortened urls
        /// </summary>
        public string ReturnTextWithShortenedUrls(string text)
        {
            foreach (string url in GetUrls(text))
            {
                string trimmedurl = url.Trim();

                string shortenurl = trimmedurl;
                if (!trimmedurl.Contains("http://") || !trimmedurl.Contains("https://"))
                {
                    shortenurl = "http://" + trimmedurl;
                }


                text = text.Replace(trimmedurl, ShortenUrl(shortenurl));
                text = text.Replace("http://http://", "http://");
            }

            return text;
        }




    }
}
