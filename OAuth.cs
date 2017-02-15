using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace obao.common
{
    public class OAuth
    {
        public const string ConsumerKeyKey = "oauth_appkey";
        public const string SignatureKey = "oauth_signature";
        public const string TimestampKey = "oauth_timestamp";
        public const string TokenKey = "oauth_token";

        private Random random = new Random();
        public static string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        public string GetOauthUrl(string url, string appKey, string appSecrect, 
                   string accessToken, List<Parameter> parameters, out string queryString)
        {
            string parameterString = this.normalizeRequestParameters(parameters);

            string urlWithParameter = url;
            if (!string.IsNullOrEmpty(parameterString))
            {
                urlWithParameter += "?" + parameterString;
            }

            Uri uri = new Uri(urlWithParameter);
            //string timeStamp = OAuth.GenTimeStamp();
            //parameters.Add(new Parameter(TimestampKey, timeStamp));
            //parameters.Add(new Parameter(ConsumerKeyKey, appKey));
            //if (!string.IsNullOrEmpty(accessToken))
            //{
            //    parameters.Add(new Parameter(TokenKey, accessToken));
            //}

            string normalizedUrl = null;
            string signature = this.genSignature(uri, appSecrect, parameters, out normalizedUrl, out queryString);
            queryString += "&oauth_signature=" + OAuth.UrlEncode(signature);

            return normalizedUrl;
        }

        private string genSignature(Uri url, string appSecret, List<Parameter> parameters, out string normalizedUrl, out string normalizedRequestParameters)
        {
            string signatureBase = this.genSignatureBase(url, appSecret, parameters, out normalizedUrl, out normalizedRequestParameters);

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5_in = UTF8Encoding.Default.GetBytes(signatureBase);
            byte[] md5_out = md5.ComputeHash(md5_in);
            string ret = BitConverter.ToString(md5_out).Replace("-", "").ToLower();
            return ret;
        }

        private string genSignatureBase(Uri url, string appSecret, IList<Parameter> parameters, out string normalizedUrl, out string normalizedRequestParameters)
        {
            normalizedUrl = null;
            normalizedRequestParameters = null;

            ((List<Parameter>)parameters).Sort(new ParameterComparer());

            normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
            {
                normalizedUrl += ":" + url.Port;
            }
            normalizedUrl += url.AbsolutePath;

            normalizedRequestParameters = this.formEncodeParameters(parameters);

            string normalizedRequestParameters2 = OAuth.UrlEncode(normalizedRequestParameters);
            StringBuilder signatureBase = new StringBuilder();
            signatureBase.AppendFormat("{1}&{0}&{1}", normalizedRequestParameters2, appSecret);

            return signatureBase.ToString();
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="parameters">参数</param>
        /// <param name="appSecret">应用程序密钥</param>
        /// <returns></returns>
        public string GenSingature(IList<Parameter> parameters, string appSecret)
        {
            ((List<Parameter>)parameters).Sort(new ParameterComparer());
            string s1 = this.formEncodeParameters(parameters);
            string s2 = OAuth.UrlEncode(s1);
            StringBuilder signatureBase = new StringBuilder();
            signatureBase.AppendFormat("{1}&{0}&{1}", s2, appSecret);
            string s3 = signatureBase.ToString();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5_in = UTF8Encoding.Default.GetBytes(s3);
            byte[] md5_out = md5.ComputeHash(md5_in);
            string ret = BitConverter.ToString(md5_out).Replace("-", "").ToLower();
            return ret;
        }

        private string genEncodeParameters(List<Parameter> parameters)
        {
            List<Parameter> encodeParams = new List<Parameter>();
            foreach (Parameter a in parameters)
            {
                encodeParams.Add(new Parameter(a.Name, OAuth.UrlEncode(a.Value)));
            }

            return this.normalizeRequestParameters(encodeParams);
        }

        private string generateSignatureUsingHash(string data, HashAlgorithm hash)
        {
            return this.computeHash(hash, data);
        }

        /// <summary>
        /// 生成HASH值
        /// </summary>
        /// <param name="hashAlgorithm">hash算法</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        private string computeHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException("hashAlgorithm");
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }

            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

        private string formEncodeParameters(IList<Parameter> parameters)
        {
            IList<Parameter> encodeParams = new List<Parameter>();
            foreach (Parameter a in parameters)
            {
                encodeParams.Add(new Parameter(a.Name, OAuth.UrlEncode(a.Value)));
            }

            return this.normalizeRequestParameters(encodeParams);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string UrlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            StringBuilder ret = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(value);

            foreach (byte symbol in byStr)
            {
                if (UnreservedChars.IndexOf((char)symbol) != -1)
                {
                    ret.Append((char)symbol);
                }
                else
                {
                    ret.Append('%' + Convert.ToString((char)symbol, 16).ToUpper());
                }
            }

            return ret.ToString();
        }

        /// <summary>
        /// 标准化参数
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        private string normalizeRequestParameters(IList<Parameter> parameters)
        {
            StringBuilder sb = new StringBuilder();
            Parameter p = null;
            for (int i = 0; i < parameters.Count; i++)
            {
                p = parameters[i];
                sb.AppendFormat("{0}={1}", p.Name, p.Value);

                if (i < parameters.Count - 1)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成时间戳
        /// </summary>
        /// <returns></returns>
        public static string GenTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
    }
}
