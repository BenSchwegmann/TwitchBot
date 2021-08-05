using ApuDoingStuff.Jsons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApuDoingStuff.Commands
{
    public static class CommandHelper
    {
        public static Command GetCommand(CommandType type)
        {
            return JsonController.CommandList.Commands.Where(cmd => cmd.CommandName == type.ToString().ToLower()).FirstOrDefault();
        }

        public static MessageCommandsCooldown GetMessageCommand (MessageType type)
        {
            return JsonController.CommandList.MessageCommandsCooldowns.Where(cmd => cmd.CommandName == type.ToString().ToLower()).FirstOrDefault();
        }
    }

    
}
