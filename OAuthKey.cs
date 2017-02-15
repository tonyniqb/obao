using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obao.common
{
    public class OAuthKey
    {
        /// <summary>
        /// 访问
        /// </summary>
        public const string UrlGetAccessToken = "http://10.0.32.107:9900/api/token/get";

        /// <summary>
        /// 应用程序KEY
        /// </summary>
        public string AppKey
        { get; set; }
        /// <summary>
        /// 应用程序密钥
        /// </summary>
        public string AppSecret
        { get; set; }
        /// <summary>
        /// 应用程序访问令牌
        /// </summary>
        public string AccessToken
        { get; set; }
        /// <summary>
        /// 访问用户ID
        /// </summary>
        public string UserID
        { get; set; }
        /// <summary>
        /// 访问用户姓名
        /// </summary>
        public string UserName
        { get; set; }
        /// <summary>
        /// 字符集
        /// </summary>
        public Encoding Charset 
        { get; set; }

        public OAuthKey()
        {
            this.AppKey = null;
            this.AppSecret = null;
            this.AccessToken = null;
            this.Charset = Encoding.UTF8;
        }

        public OAuthKey(string appKey, string appSecret)
            : this()
        {
            this.AppKey = appKey;
            this.AppSecret = appSecret;
        }
    }
}
