//using Alipay.AopSdk.Core;
//using Alipay.AopSdk.Core.Domain;
//using Alipay.AopSdk.Core.Request;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Blog.Core.Common
//{
//    public class AlipayHelper
//    {
//        private LogHelper _log = new LogHelper(typeof(AlipayHelper));
//        private ConfigHelper _config = new ConfigHelper("Alipay.xml");
//        private const string ProductCOde = "FAST_INSTANT_TRADE_PAY";// 销售产品码，与支付宝签约的产品码名称

//        /// 发起支付请求
//        /// </summary>
//        /// <param name="tradeno">外部订单号，商户网站订单系统中唯一的订单号</param>
//        /// <param name="subject">订单名称</param>
//        /// <param name="totalAmout">付款金额</param>
//        /// <param name="itemBody">商品描述</param>
//        /// <returns></returns>
//        public string GetAlipayUrl(string tradeno, string subject, string totalAmout, string itemBody)
//        {
//            try
//            {
//                string result = string.Empty;
//                DefaultAopClient client = new DefaultAopClient(_config.ALIPAY_URL, _config.ALIPAY_APPID, _config.ALIPAY_APP_PRIVATE_KEY, "json", "2.0", _config.ALIPAY_SIGN_TYPE, _config.ALIPAY_APP_PUBLIC_KEY, _config.ALIPAY_CHARSET, false);
//                // 组装业务参数model
//                AlipayTradePagePayModel model = new AlipayTradePagePayModel();
//                model.Body = itemBody;
//                model.Subject = subject;
//                model.TotalAmount = totalAmout;
//                model.OutTradeNo = tradeno;
//                model.ProductCode = ProductCOde;

//                AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
//                // 设置同步回调地址
//                //request.SetReturnUrl("http://localhost:5000/Pay/Callback");
//                // 设置异步通知接收地址
//                request.SetNotifyUrl("");
//                // 将业务model载入到request
//                request.SetBizModel(model);

//                var response = client.SdkExecute(request);
//                Console.WriteLine($"订单支付发起成功，订单号：{tradeno}");
//                return result;
//            }
//            catch (Exception ex)
//            {
//                _log.Error(ex);
//                throw ex;
//            }
//        }
//    }
//    #region 请求参数
//    /// <summary>
//    /// 公共参数
//    /// </summary>
//    public class PublicParams
//    {
//        /// <summary>
//        /// 支付宝分配给开发者的应用ID
//        /// </summary>
//        public string app_id { get; set; }

//        /// <summary>
//        /// 接口名称
//        /// </summary>
//        public string method { get; set; }

//        /// <summary>
//        /// 仅支持JSON - 否
//        /// </summary>
//        public string format { get; set; } = string.Empty;

//        /// <summary>
//        /// HTTP/HTTPS开头字符串 - 否
//        /// https://m.alipay.com/Gk8NF23
//        /// </summary>
//        public string return_url { get; set; } = string.Empty;

//        /// <summary>
//        /// 请求使用的编码格式，如utf-8,gbk,gb2312等
//        /// </summary>
//        public string charset { get; set; }

//        /// <summary>
//        /// 商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
//        /// </summary>
//        public string sign_type { get; set; }

//        /// <summary>
//        /// 商户请求参数的签名串
//        /// </summary>
//        public string sign { get; set; }

//        /// <summary>
//        /// 发送请求的时间，格式"yyyy-MM-dd HH:mm:ss"
//        /// </summary>
//        public string timestamp { get; set; }

//        /// <summary>
//        /// 调用的接口版本，固定为：1.0
//        /// </summary>
//        public string version { get; set; }

//        /// <summary>
//        /// 支付宝服务器主动通知商户服务器里指定的页面http/https路径 - 否
//        /// </summary>
//        public string notify_url { get; set; } = string.Empty;

//        /// <summary>
//        /// 第三方授权token - 否
//        /// </summary>
//        public string app_auth_token { get; set; } = string.Empty;

//        /// <summary>
//        /// 请求参数的集合
//        /// </summary>
//        public RequestParams biz_content { get; set; }
//    }

//    /// <summary>
//    /// 请求参数
//    /// </summary>
//    public class RequestParams
//    {
//        /// <summary>
//        /// 商户订单号,64个字符以内、可包含字母、数字、下划线；需保证在商户端不重复
//        /// </summary>
//        public string out_trade_no { get; set; }

//        /// <summary>
//        /// 销售产品码，与支付宝签约的产品码名称。 
//        /// 注：目前仅支持FAST_INSTANT_TRADE_PAY
//        /// </summary>
//        public string product_code { get; set; }

//        /// <summary>
//        /// 订单总金额，单位为元，精确到小数点后两位
//        /// </summary>
//        public decimal total_amount { get; set; }

//        /// <summary>
//        /// 订单标题
//        /// </summary>
//        public string subject { get; set; }

//        /// <summary>
//        /// 订单描述 - 否
//        /// </summary>
//        public string body { get; set; }

//        /// <summary>
//        /// 绝对超时时间，格式为yyyy-MM-dd HH:mm:ss - 否
//        /// </summary>
//        public string time_expire { get; set; } = string.Empty;

//        /// <summary>
//        /// 订单包含的商品列表信息，json格式
//        /// </summary>
//        public GoodsDetail[] goods_detail { get; set; }

//        /// <summary>
//        /// 公用回传参数 - 否
//        /// 如果请求时传递了该参数，则返回给商户时会回传该参数。
//        /// 支付宝只会在同步返回（包括跳转回商户网站）和异步通知时将该参数原样返回。
//        /// 本参数必须进行UrlEncode之后才可以发送给支付宝。
//        /// </summary>
//        public string passback_params { get; set; } = string.Empty;

//        /// <summary>
//        /// 业务扩展参数-否
//        /// </summary>
//        public ExtendParams extend_params { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        public string goods_type { get; set; }

//        /// <summary>
//        /// 商品主类型 - 否
//        /// 0-虚拟类商品,1-实物类商品
//        /// 注：虚拟类商品不支持使用花呗渠道
//        /// </summary>
//        public string sss { get; set; } = "0";

//        /// <summary>
//        /// 该笔订单允许的最晚付款时间，逾期将关闭交易 - 否
//        /// 取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）
//        /// </summary>
//        public string timeout_express { get; set; } = "90m";

//        /// <summary>
//        /// 优惠参数  - 否
//        /// </summary>
//        public string promo_params { get; set; }
//    }

//    /// <summary>
//    /// 商品信息
//    /// </summary>
//    public class GoodsDetail
//    {
//        /// <summary>
//        /// 商品的编号
//        /// </summary>
//        public string goods_id { get; set; }

//        /// <summary>
//        /// 支付宝定义的统一商品编号 - 否
//        /// </summary>
//        public string alipay_goods_id { get; set; }

//        /// <summary>
//        /// 商品名称
//        /// </summary>
//        public string goods_name { get; set; }

//        /// <summary>
//        /// 商品数量
//        /// </summary>
//        public int quantity { get; set; }

//        /// <summary>
//        /// 商品单价，单位为元
//        /// </summary>
//        public decimal price { get; set; }

//        /// <summary>
//        /// 商品类目 - 否
//        /// </summary>
//        public string goods_category { get; set; }

//        /// <summary>
//        /// 商品类目树 - 否
//        /// 124868003|126232002|126252004
//        /// </summary>
//        public string categories_tree { get; set; }

//        /// <summary>
//        /// 商品描述信息 - 否
//        /// </summary>
//        public string body { get; set; }

//        /// <summary>
//        /// 商品的展示地址
//        /// </summary>
//        public string show_url { get; set; }
//    }

//    /// <summary>
//    /// 业务扩展参数
//    /// </summary>
//    public class ExtendParams
//    {
//        /// <summary>
//        /// 系统商编号 - 否
//        /// 该参数作为系统商返佣数据提取的依据，
//        /// 请填写系统商签约协议的PID
//        /// </summary>
//        public string sys_service_provider_id { get; set; } = string.Empty;

//        /// <summary>
//        /// 使用花呗分期要进行的分期数 - 否
//        /// </summary>
//        public string hb_fq_num { get; set; } = string.Empty;

//        /// <summary>
//        /// 使用花呗分期需要卖家承担的手续费比例的百分值 - 否
//        /// 传入100代表100%
//        /// </summary>
//        public string hb_fq_seller_percent { get; set; } = string.Empty;

//        /// <summary>
//        /// 行业数据回流信息 - 否
//        /// </summary>
//        public string industry_reflux_info { get; set; } = string.Empty;

//        /// <summary>
//        /// 卡类型 - 否
//        /// </summary>
//        public string card_type { get; set; } = string.Empty;
//    }
//    #endregion

//    #region 响应参数
//    /// <summary>
//    /// 公共响应参数
//    /// </summary>
//    public class PublicResponseParams
//    {
//        /// <summary>
//        /// 网关返回码
//        /// </summary>
//        public string code { get; set; }

//        /// <summary>
//        /// 网关返回码描述
//        /// </summary>
//        public string msg { get; set; }

//        /// <summary>
//        /// 业务返回码
//        /// </summary>
//        public string sub_code { get; set; }

//        /// <summary>
//        /// 业务返回码描述
//        /// </summary>
//        public string sub_msg { get; set; }

//        /// <summary>
//        /// 签名
//        /// </summary>
//        public string sign { get; set; }
//    }

//    /// <summary>
//    /// 公共响应参数
//    /// </summary>
//    public class ResponseParams
//    {
//        /// <summary>
//        /// 支付宝交易号
//        /// </summary>
//        public string trade_no { get; set; }

//        /// <summary>
//        /// 商户订单号
//        /// </summary>
//        public string out_trade_no { get; set; }

//        /// <summary>
//        /// 收款支付宝账号对应的支付宝唯一用户号
//        /// </summary>
//        public string seller_id { get; set; }

//        /// <summary>
//        /// 交易金额
//        /// </summary>
//        public decimal total_amount { get; set; }
//    }
//    #endregion
//}
