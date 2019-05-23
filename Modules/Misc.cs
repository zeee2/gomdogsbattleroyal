using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Net;
using Newtonsoft.Json;
using CSharpOsu;
using CSharpOsu.Module;
using CSharpOsu.Util;

namespace GBR_Bot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        OsuClient _client = Program._osuclient;

        [Command("!count")]
        public async Task Plist()
        {
            string[] gbr = File.ReadAllLines("Resources/gbr.json");
            string[] gbrc = File.ReadAllLines("Resources/gbrc.json");

            await Context.Channel.SendMessageAsync($"GBR : {gbr.Length - 2}\nGBRC : {gbrc.Length - 2}");
        }

        [Command("!참가신청")]
        public async Task Participate([Remainder] string command = null)
        {
            OsuUser[] Player = _client.GetUser(command);
            OsuUser player = Player[0];

            if (player == null)
            {
                await Context.Channel.SendMessageAsync("닉네임을 입력해주세요!");
            }
            else
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>();
                pairs.Add(Context.User.Username.ToString(), player.pp_rank.ToString());
                string data = JsonConvert.SerializeObject(pairs, Formatting.Indented);

                if (player.pp_rank < 30000)
                {
                    string json = "Resources/gbr.json";
                    File.WriteAllText(json, data);
                    var user = Context.User;
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "GBR PLAYER");
                    await Context.Channel.SendMessageAsync("GBR 신청이 완료되었습니다!");
                    await (user as IGuildUser).AddRoleAsync(role);
                }
                else
                {
                    string json = "Resources/gbrc.json";
                    File.WriteAllText(json, data);
                    var user = Context.User;
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "GBRC PLAYER");
                    await Context.Channel.SendMessageAsync("GBRC 신청이 완료되었습니다!");
                    await (user as IGuildUser).AddRoleAsync(role);
                }
            }
        }

        [Command("!add")]
        public async Task Add([Remainder] string command = null)
        {
            if (command == null)
            {
                await Context.Channel.SendMessageAsync("닉네임을 입력해주세요!");
                return;
            }

            OsuUser[] Player = _client.GetUser(command);
            OsuUser player = Player[0];
            Dictionary<string, string> pairs;
            
            if (player.pp_rank < 30000)
            {
                string json = File.ReadAllText("Resources/gbr.json");
                var data = JsonConvert.DeserializeObject<dynamic>(json);

                pairs = data.ToObject<Dictionary<string, string>>();
                pairs.Add(player.username, player.pp_rank.ToString());
                string datas = JsonConvert.SerializeObject(pairs, Formatting.Indented);
                File.WriteAllText("Resources/gbr.json", datas);

                await Context.Channel.SendMessageAsync("GBR에 추가되었습니다.");
            }
            else
            {
                string json = File.ReadAllText("Resources/gbrc.json");
                var data = JsonConvert.DeserializeObject<dynamic>(json);

                pairs = data.ToObject<Dictionary<string, string>>();
                pairs.Add(player.username, player.pp_rank.ToString());
                string datas = JsonConvert.SerializeObject(pairs, Formatting.Indented);
                File.WriteAllText("Resources/gbrc.json", datas);

                await Context.Channel.SendMessageAsync("GBRC에 추가되었습니다.");
            }
        }
        

        [Command("!닉네임")]
        public async Task Nickname([Remainder]string comm)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            string json = "Resources/nickname.json";
            pairs.Add(Context.User.Id.ToString(), comm);
            string data = JsonConvert.SerializeObject(pairs, Formatting.Indented);
            File.WriteAllText(json, data);

            await Context.Channel.SendMessageAsync($"{Context.User.Username}님의 osu! 닉네임이 **{comm}**으로 변경되었습니다.");

        }

        [Command("!c")]
        public async Task Calculator([Remainder]string mes)
        {
            string[] spl = mes.Split();
            float score = float.Parse(spl[1]);

            OsuUser[] Player = _client.GetUser(spl[0]);
            OsuUser player = Player[0];

            if (player.pp_rank < 10)
                score *= 0.8f;
            else if (player.pp_rank < 30)
                score *= 0.81f;
            else if (player.pp_rank < 60)
                score *= 0.82f;
            else if (player.pp_rank < 100)
                score *= 0.825f;
            else if (player.pp_rank < 200)
                score *= 0.83f;
            else if (player.pp_rank < 400)
                score *= 0.835f;
            else if (player.pp_rank < 700)
                score *= 0.85f;
            else if (player.pp_rank < 1000)
                score *= 0.88f;
            else if (player.pp_rank < 2000)
                score *= 0.905f;
            else if (player.pp_rank < 4000)
                score *= 0.935f;
            else if (player.pp_rank < 7000)
                score *= 0.96f;
            else if (player.pp_rank < 10000)
            { }
            else if (player.pp_rank < 15000)
                score *= 1.125f;
            else if (player.pp_rank < 20000)
                score *= 1.25f;
            else if (player.pp_rank < 25000)
                score *= 1.375f;
            else if (player.pp_rank < 30000)
                score *= 1.5f;
            else
            { }

            await Context.Channel.SendMessageAsync(score.ToString());
        }
    }
}
