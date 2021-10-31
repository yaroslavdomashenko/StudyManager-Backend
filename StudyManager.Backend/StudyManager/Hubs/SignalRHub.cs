using Microsoft.AspNetCore.SignalR;
using StudyManager.Data.Models.Chat;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManager.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly IChatService _chatService;
        public SignalRHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task EnterToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveTheGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToGroup(MessageModel message)
        {
            var msg = await _chatService.AddMessage(message);
            await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", msg);
        }
    }
}
