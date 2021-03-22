using System;
using System.Collections.Generic;
using System.Text;
using Blog.Core.Common;

namespace Blog.Core.Biz.Dream
{
    /// <summary>
    /// 梦想编信息辑模型
    /// </summary>
    public class DreamInfoEditModel
    {
        /// <summary>
        /// 彩票Id
        /// </summary>
        public string DreamInfoId { get; set; }

        /// <summary>
        /// 彩票号码
        /// </summary>
        public string DreamCode { get; set; }

        /// <summary>
        /// 起始期数
        /// </summary>
        public int StartStage { get; set; }

        /// <summary>
        /// 截止期数
        /// </summary>
        public int EndStage { get; set; }

        /// <summary>
        /// 彩票类型
        /// </summary>
        public int Type { get; set; }
    }

    //如果好用，请收藏地址，帮忙分享。
    public class PrizeLevelListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int awardType { get; set; }
        /// <summary>
        /// 一等奖
        /// </summary>
        public string prizeLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string stakeAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string stakeCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalPrizeamount { get; set; }
    }

    public class ListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string drawFlowFund { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string drawFlowFundRj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string estimateDrawTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isGetKjpdf { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isGetXlpdf { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotteryDrawNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotteryDrawResult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lotteryDrawStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotteryDrawTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lotteryEquipmentCount { get; set; }
        /// <summary>
        /// 超级大乐透
        /// </summary>
        public string lotteryGameName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotteryGameNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lotteryGamePronum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lotteryPromotionFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotterySaleBeginTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotterySaleEndtime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lotterySuspendedFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotteryUnsortDrawresult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> matchList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pdfType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string poolBalanceAfterdraw { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string poolBalanceAfterdrawRj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PrizeLevelListItem> prizeLevelList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> prizeLevelListRj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ruleType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> termList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalSaleAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalSaleAmountRj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int verify { get; set; }
    }

    public class Value
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ListItem> list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pages { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string dataFrom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string emptyFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// 处理成功
        /// </summary>
        public string errorMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Value value { get; set; }
    }
}