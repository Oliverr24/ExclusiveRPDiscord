using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using ExclusiveRPBot.Commands;
using ExclusiveRPBot.Methods;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveRPBot {
    public class Bot {

        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }

        public async Task RunAsync() {

            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                Intents = DiscordIntents.All
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;
            Client.MessageCreated += MessageListenerForQuery;

            Client.UseInteractivity(new InteractivityConfiguration {
                Timeout = TimeSpan.FromMinutes(10)
            });

            var commandsConfig = new CommandsNextConfiguration {

                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = false,

            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<Report>();

            await Client.ConnectAsync();

            await Task.Delay(-1);

        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e) {
            return Task.CompletedTask;
        }

        private async Task MessageListenerForQuery(DiscordClient sender, MessageCreateEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var generalQueryChatInstance = new GeneralQueryChatListener();

            _ = Task.Run(() => generalQueryChatInstance.GeneralQueryListenToChat(sender, e.Guild, e.Message));


            await Task.CompletedTask;
        }

    }
}
