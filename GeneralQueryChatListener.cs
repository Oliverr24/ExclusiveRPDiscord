using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExclusiveRPBot.Methods {
    public class GeneralQueryChatListener {

        public async Task GeneralQueryListenToChat(DiscordClient client, DiscordGuild guild, DiscordMessage message) {

            if (client == null || guild == null || message == null) {
                return;
            }

            if (guild.Id != 810465971201376256 && guild.Id != 827523101981540362) {
                return;
            }

            var messageMember = (DiscordMember) message.Author;

            var respondChannel = message.Channel;
            var ticketChannel = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("open-a-ticket")).Value;

            var replyEmbed = new DiscordEmbedBuilder() {
                Color = DiscordColor.Cyan,
                Timestamp = DateTime.UtcNow,
            };

            string[] sentences = message.Content.Split('.', '!', '?');

            foreach(var sen in sentences) {

                //Ban query
                if (sen.ToLower().Contains("i am banned") || sen.ToLower().Contains("help i'm banned") || sen.ToLower().Contains("help i got banned") || sen.ToLower().Contains("help im banned") || sen.ToLower().Contains("why am i banned") || sen.ToLower().Contains("can i be unbanned") || ((sen.ToLower().Contains("explosion") || sen.ToLower().Contains("explosion")) && (sen.ToLower().Contains("got banned") || sen.ToLower().Contains("banned")))) {

                    replyEmbed.Title = "Help I Am Banned";
                    replyEmbed.Description = $"{messageMember.Mention} please {ticketChannel.Mention}. This will be looked at as soon as possble. Please do not ping any staff members regarding this.";

                    await respondChannel.SendMessageAsync(embed: replyEmbed);

                    return;

                }

                //Map or Texture query
                if (sen.ToLower().Contains("map not loading") || sen.ToLower().Contains("texture bug") || sen.ToLower().Contains("invisible roads") || sen.ToLower().Contains("bugged textures")) {
                    
                    replyEmbed.Title = "Texture Issues In Game";
                    replyEmbed.Description = $"{messageMember.Mention} please make sure your settings are as follows or lower: Graphics **all** set to 'Normal' or lower. Also check 'Population Density', 'Population Variety', and 'Distance Scaling'. If they're maxed out, lower them down slightly. Further issues, please {ticketChannel.Mention}";

                    await respondChannel.SendMessageAsync(embed: replyEmbed);

                    return;

                }

                //Error queries
                if (sen.ToLower().Contains("curl error") || sen.ToLower().Contains("curl error 18") || sen.ToLower().Contains("curl error 28") || sen.ToLower().Contains("curl 18 error") || sen.ToLower().Contains("curl 28 error")) {

                    replyEmbed.Title = "Curl Error";
                    replyEmbed.Description = $"{messageMember.Mention} please do the following and try again.. Close down FiveM and Steam. Re-open Steam, and then open FiveM and try connect again. \nIf that fails, try connecting through **'F8'** and typing `connect cfxe.re/join/kpxpqa`.";

                    await respondChannel.SendMessageAsync(embed: replyEmbed);

                    return;

                }

            }


        }

    }
}
