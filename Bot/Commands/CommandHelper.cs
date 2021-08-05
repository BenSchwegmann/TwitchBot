using ApuDoingStuff.Jsons;
using System.Linq;

namespace ApuDoingStuff.Commands
{
    public static class CommandHelper
    {
        public static Command GetCommand(CommandType type)
        {
            return JsonController.CommandList.Commands.Where(cmd => cmd.CommandName == type.ToString().ToLower()).FirstOrDefault();
        }

        public static MessageCommandsCooldown GetMessageCommand(MessageType type)
        {
            return JsonController.CommandList.MessageCommands.Where(cmd => cmd.MessageName == type.ToString().ToLower()).FirstOrDefault();
        }
    }


}
