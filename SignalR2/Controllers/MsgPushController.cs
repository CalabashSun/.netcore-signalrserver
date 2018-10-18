using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR2.Hubs;
using SignalR2.Models;

namespace SignalR2.Controllers
{
    [Route("api/MsgPush")]
    public class MsgPushController : Controller
    {
        private readonly IHubContext<MemberHubs> _hubContext;

        public MsgPushController(IHubContext<MemberHubs> hubContext)
        {
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost,Route("MsgPushSingle")]
        public IActionResult MsgPushSingle([FromBody]SignalrGroups group)
        {
            if (group != null && !string.IsNullOrEmpty(group.ShopId))
            {
                var list = SignalrGroups.UserGroups.FirstOrDefault(p => p.ShopId==group.ShopId);
                _hubContext.Clients.Client(list.ConnectionId).SendAsync("MsgPushSingle",
                    $"{list.ConnectionId}: {list.Content}");
            }
            return Ok();
        }

        [HttpGet,Route("MsgPushAll")]
        public IActionResult MsgPushAll(string message)
        {
            _hubContext.Clients.All.SendAsync("MsgPushAll",
                message);
            return Ok();
        }
        [HttpPost]
        public IActionResult MsgPushGroup([FromBody] SignalrGroups group)
        {
            if (group != null)
            {
                _hubContext.Clients.Group(group.GroupName).SendAsync("MsgPushGroup", $"{group.Content}");
            }

            return Ok();
        }
    }
}