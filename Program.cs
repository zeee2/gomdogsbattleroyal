using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Discord.Commands;
using CSharpOsu;
using CSharpOsu.Module;

namespace GBR_Bot
{
    class Program
    {
        DiscordSocketClient _client;
        CommandHandler _handler;
        public static OsuClient _osuclient = new OsuClient("c7e4c336af96d85c90a76705a077bd5355b88b96");

        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null)
                return;
            _client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content.Contains("https://osu.ppy.sh/u"))
            {
                string mes = message.ToString();
                string[] username = mes.Split(new string[] { "sh/" }, StringSplitOptions.None);
                string[] username2 = username[1].Split('/');
                OsuUser[] Player = _osuclient.GetUser(username2[1]);
                OsuUser player = Player[0];

                if (player == null)
                {
                    await message.Channel.SendMessageAsync("올바른 유저페이지를 입력해주세요.");
                }
                else if (player.pp_rank < 30000)
                {
                    await message.Channel.SendMessageAsync($"**{player.username}** 님의 랭킹은 **#{player.pp_rank}**이며, GBR에 참가하실 수 있습니다.");

                    if (player.pp_rank < 10)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티는 **-0.2**로, 최종 배율은 **x0.8**으로 계산됩니다.");
                    else if (player.pp_rank < 30)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티는 **-0.19**로, 최종 배율은 **x0.81**로 계산됩니다.");
                    else if (player.pp_rank < 60)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티는 **-0.18**으로, 최종 배율은 **x0.82**로 계산됩니다.");
                    else if (player.pp_rank < 100)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티는 **-0.175**로, 최종 배율은 **x0.825**로 계산됩니다.");
                    else if (player.pp_rank < 200)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티는 **-0.17**으로, 최종 배율은 **x0.83**으로 계산됩니다.");
                    else if (player.pp_rank < 400)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티는 **-0.165**로, 최종 배율은 **x0.835**로 계산됩니다.");
                    else if (player.pp_rank < 700)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티는 **-0.15**로, 최종 배율은 **x0.85**로 계산됩니다.");
                    else if (player.pp_rank < 1000)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티는 **-0.12**로, 최종 배율은 **x0.88**으로 계산됩니다.");
                    else if (player.pp_rank < 2000)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티는 **-0.095**로, 최종 배율은 **x0.905**로 계산됩니다.");
                    else if (player.pp_rank < 4000)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티는 **-0.065**로, 최종 배율은 **x0.935**로 계산됩니다.");
                    else if (player.pp_rank < 7000)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티는 **-0.04**로, 최종 배율은 **x0.96**로 계산됩니다.");
                    else if (player.pp_rank < 10000)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 패널티나 베네핏이 없습니다. 최종 배율은 **x1.00**으로 계산됩니다.");
                    else if (player.pp_rank < 15000)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 베네핏은 **0.125**로, 최종 배율은 **x1.125**로 계산됩니다.");
                    else if (player.pp_rank < 20000)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 베네핏은 **0.25**로, 최종 배율은 **x1.25**로 계산됩니다.");
                    else if (player.pp_rank < 25000)
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 베네핏은 **0.375**로, 최종 배율은 **x1.375**로 계산됩니다.");
                    else
                        await message.Channel.SendMessageAsync("랭킹에 해당하는 베네핏은 **0.5**로, 최종 배율은 **x1.5**로 계산됩니다.");
                }
                else
                {
                    await message.Channel.SendMessageAsync($"**{player.username}** 님의 랭킹은 **#{player.pp_rank}**이며, GBRC에 참가하실 수 있습니다.");
                }
            }
            else return;
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}
