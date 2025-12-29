//using Microsoft.AspNetCore.SignalR;
//using System;
//using System.Threading.Tasks;

//namespace Web_Project.Hubs
//{
//    public class OrderHub : Hub
//    {
//        private const string AdminsGroup = "admins";

//        public override async Task OnConnectedAsync()
//        {
//            if (Context.User?.HasClaim(c => c.Type == "Role" && c.Value == "Admin") == true)
//            {
//                await Groups.AddToGroupAsync(Context.ConnectionId, AdminsGroup);
//            }

//            await base.OnConnectedAsync();
//        }

//        public override async Task OnDisconnectedAsync(Exception? exception)
//        {
//            if (Context.User?.HasClaim(c => c.Type == "Role" && c.Value == "Admin") == true)
//            {
//                await Groups.RemoveFromGroupAsync(Context.ConnectionId, AdminsGroup);
//            }

//            await base.OnDisconnectedAsync(exception);
//        }
//    }
//}


using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web_Project.Hubs
{
    public class OrderHub : Hub
    {
        private const string AdminsGroup = "admins";

        public override async Task OnConnectedAsync()
        {
            // Use IsInRole (works if you enabled roles) and fall back to common role claim checks
            var user = Context.User;
            if (user != null &&
                (user.IsInRole("Admin")
                 || user.HasClaim(c => (c.Type == "Role" && c.Value == "Admin")
                                       || (c.Type == ClaimTypes.Role && c.Value == "Admin")
                                       || (c.Type == "role" && c.Value == "Admin"))))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, AdminsGroup);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = Context.User;
            if (user != null &&
                (user.IsInRole("Admin")
                 || user.HasClaim(c => (c.Type == "Role" && c.Value == "Admin")
                                       || (c.Type == ClaimTypes.Role && c.Value == "Admin")
                                       || (c.Type == "role" && c.Value == "Admin"))))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, AdminsGroup);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}