using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obao.common
{
    public class Constant
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public const string NameSpace = "http://www.niqb.com.cn/obao";
        /// <summary>
        /// 返回码：接口执行成功
        /// </summary>
        public const int RETURN_CODE_SUCCESSFUL = 0;
        /// <summary>
        /// 返回码：未知错误
        /// </summary>
        public const int RETURN_CODE_UNKNOWN = 9999;

        public const string API_URL = "http://10.0.32.107:9900/api/";

        public const string API_URL_TOKEN_GET = API_URL + "token/get";

        public const string API_URL_ITEM_SEARCH = API_URL + "item/search";
        public const string API_URL_ITEM_GET = API_URL + "item/get";
        public const string API_URL_ITEM_CATEGORY_SEARCH = API_URL + "item/category/search";
        public const string API_URL_ITEM_TAG_SEARCH = API_URL + "item/tag/search";

        public const string API_URL_PROMOTION_PLAN_SEARCH = API_URL + "promotion/plan/search";
        public const string API_URL_PROMOTION_PLAN_GET = API_URL + "promotion/plan/get";

        public const string API_URL_COMMON_ASSESS_ADD = API_URL + "common/assess/add";

        public const string API_URL_VIP_REGIEST = API_URL + "vip/regist";
    }
}
