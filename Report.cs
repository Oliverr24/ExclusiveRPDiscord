using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExclusiveRPBot.Commands {
    public class Report : BaseCommandModule {

        [Command("report")]
        [Hidden]
        public async Task ReportCommand(CommandContext ctx) {

            if (ctx.Guild == null) {
                return;
            }

            if (ctx.Guild.Id != 827523101981540362) {
                return;
            }

            await ctx.Message.DeleteAsync();

            var reportChannel = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.Contains("reports")).Value;

            var interactivity = ctx.Client.GetInteractivity();

            //
            //Patient
            //Incident
            //Injury
            //Solution
            //

            List<DiscordMessage> delMsg = new List<DiscordMessage>();

            //Patient Question 
            var patientMessage = await ctx.Channel.SendMessageAsync("Please type out the **Patients** name.");

            delMsg.Add(patientMessage);

            var patientResponse = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.Member.Id && x.Channel == ctx.Channel);

            delMsg.Add(patientResponse.Result);

            //Incident Question
            var incidentMessage = await ctx.Channel.SendMessageAsync("Please type out the **Incident** that occured.");

            delMsg.Add(incidentMessage);

            var incidentResponse = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.Member.Id && x.Channel == ctx.Channel);

            delMsg.Add(incidentResponse.Result);

            //Injury Question
            var injuryMessage = await ctx.Channel.SendMessageAsync("Please type out the **Injury** the patient sustained.");

            delMsg.Add(injuryMessage);

            var injuryResponse = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.Member.Id && x.Channel == ctx.Channel);

            delMsg.Add(injuryResponse.Result);

            //Solution Question
            var solutionMessage = await ctx.Channel.SendMessageAsync("Please type out the **Soltuion** for the injury at hand.");

            delMsg.Add(solutionMessage);

            var solutionResponse = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.Member.Id && x.Channel == ctx.Channel);

            delMsg.Add(solutionResponse.Result);

            var reportEmbed = new DiscordEmbedBuilder() {
                Title = "EMS Report",
                Color = DiscordColor.Cyan,
                Timestamp = DateTime.UtcNow,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    Text = $"Submitted by: {ctx.Member.DisplayName}",
                    IconUrl = $"{ctx.Member.AvatarUrl}"
                }
            };

            reportEmbed.AddField("Patient:", $"{patientResponse.Result.Content}");
            reportEmbed.AddField("Incident:", $"{incidentResponse.Result.Content}");
            reportEmbed.AddField("Injury:", $"{injuryResponse.Result.Content}");
            reportEmbed.AddField("Solution:", $"{solutionResponse.Result.Content}");

            await reportChannel.SendMessageAsync(embed: reportEmbed);

            await ctx.Channel.DeleteMessagesAsync(delMsg);

        }

    }
}
