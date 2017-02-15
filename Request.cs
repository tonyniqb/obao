using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obao.common
{
    /// <summary>
    /// 请求
    /// </summary>
    public class Request
    {
        /// <summary>
        /// 同步http请求
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="httpMethod">GET/POST</param>
        /// <param name="key">访问KEY</param>
        /// <param name="listParam">参数列表</param>
        /// <param name="listFile">文件文件</param>
        /// <returns></returns>
        public string SyncRequest(string url, string httpMethod, OAuthKey key, 
            List<Parameter> listParam, List<Parameter> listFile)
        {
            OAuth oauth = new OAuth();

            string queryString = null;
            string oauthUrl = oauth.GetOauthUrl(url, key.AppKey, key.AppSecret,
                key.AccessToken, listParam, out queryString);

            SyncHttp http = new SyncHttp();
            if (httpMethod == "GET")
            {
                return http.HttpGet(oauthUrl, queryString);
            }
            else if ((listFile == null) || (listFile.Count == 0))
            {
                return http.HttpPost(oauthUrl, queryString);
            }
            else
            {
                return http.HttpPostWithFile(oauthUrl, queryString, listFile);
            }
        }
    }
}
