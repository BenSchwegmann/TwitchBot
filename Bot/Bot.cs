using System;
using System.Runtime.CompilerServices;
using TwitchLib.Client.Models;
using TwitchLib.Client;
using TwitchLib.Api.Core.HttpCallHandlers;
using TwitchLib.Client.Events;
using TwitchLib.Communication.Events;

namespace Bot
{
    internal class Bot
    {
        ConnectionCredentials creds = new ConnectionCredentials(TwitchInfo.ChannelName, TwitchInfo.Token);

        TwitchClient client;
        

        

        private void Client_OnConnect(object sender, OnConnectedArgs e)
        {
            Console.WriteLine("[Bot]: Connected");
        }

        private void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            switch (e.Command.CommandText.ToLower())
            {
                case "ping":
                    client.SendMessage(TwitchInfo.ChannelName, $"ApuSpin PONG!");
                    break;
            }
            switch (e.Command.CommandText.ToLower())
            {
                case "color":
                    client.SendMessage(TwitchInfo.ChannelName, $"{e.Command.ChatMessage.DisplayName}, your color is {e.Command.ChatMessage.ColorHex} Apu");
                    
                    break;
            }

        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Console.WriteLine($"[{e.ChatMessage.DisplayName}]: {e.ChatMessage.Message}");

            if(e.ChatMessage.DisplayName == "Lauriin")
            {
                client.SendMessage(TwitchInfo.ChannelName, "Laurin hat nen kleinen Schniedel AlienPls");
                
            }
        }

        private void Client_OnError(object sender, OnErrorEventArgs e)
        {
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine(e.Data);
        }

        internal void Disconnect()
        {
            client.Disconnect();
        }
    }
}
