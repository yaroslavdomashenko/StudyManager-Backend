using Microsoft.AspNetCore.SignalR;
using StudyManager.Data.Models.Chat;
using StudyManager.Models;
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
            ClientsHandler.ConnectedIds.Add(Context.ConnectionId, groupName);
            var count = ClientsHandler.GroupCount(groupName);
            await Clients.Group(groupName).SendAsync("GroupInfo", count);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ClientsHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToGroup(MessageModel message)
        {
            var msg = await _chatService.AddMessage(message);
            await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", msg);
        }
    }
}
