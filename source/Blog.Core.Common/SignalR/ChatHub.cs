using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Core.Common.SignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            if (!Context.User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("Unauthenticated");
            string connectId = Context.ConnectionId;
            var claims = Context.User.Claims.ToArray();
            string cacheKey = Constants.Redis_Chat_Prefix + claims.Where<Claim>(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value.ToUpper();
            if (CacheHelper.Exists(cacheKey))
            {
                CacheHelper.Remove(cacheKey);
            }
            CacheHelper.Insert(cacheKey, connectId);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (!Context.User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("Unauthenticated");
            var claims = Context.User.Claims.ToArray();
            string cacheKey = Constants.Redis_Chat_Prefix + claims.Where<Claim>(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value.ToUpper();
            CacheHelper.Remove(cacheKey);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="useraccount"></param>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string useraccount, string username, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", useraccount, username, message);
        }

        /// <summary>
        /// 发送消息给特定用户
        /// </summary>
        /// <param name="sendaccount"></param>
        /// <param name="sendname"></param>
        /// <param name="receiveid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToUser(string sendaccount, string sendname, string receiveid, string message)
        {
            string cacheKey = Constants.Redis_Chat_Prefix + receiveid.ToUpper();
            string userid = Cast.ConToString(CacheHelper.Get(cacheKey));
            if (string.IsNullOrEmpty(userid))
            {
                throw new Exception("接收用户不在线，请稍后！");
            }
            await Clients.User(userid).SendAsync("UserReceiveMessage", sendaccount, sendname, message);
        }

        /// <summary>
        /// 发送消息给特定用户组
        /// </summary>
        /// <param name="sendaccount"></param>
        /// <param name="sendname"></param>
        /// <param name="sendid"></param>
        /// <param name="receivegroupname"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToGroup(string sendaccount, string sendname, string sendid, string receivegroupname, string message)
        {
            if (string.IsNullOrEmpty(receivegroupname))
            {
                throw new Exception("接收组不存在，请稍后！");
            }
            await Clients.Group(receivegroupname).SendAsync("GroupReceiveMessage", sendaccount, sendname, message);
        }
    }
}
