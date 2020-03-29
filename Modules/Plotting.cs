using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Senate.Modules
{
    public class Commands : ModuleBase<CommandContext>
    {
        private int power;
        private List<string> plotters = new List<string>();
        private List<IGuildUser> senate = new List<IGuildUser>();

        [Command("plot")]
        public async Task Plot()
        {
            var senator = Configuration.Senator;

            foreach (var member in Context.Guild.GetUsersAsync().Result)
            foreach (var role in member.RoleIds)
                if (role == senator)
                    senate.Add(member);


            foreach (SocketRole role in ((SocketGuildUser) Context.Message.Author).Roles)
            {
                if(role.Id==senator)
                {
                    plotters.Add(Context.User.Username);
                    power = plotters.Count;
                    Console.WriteLine($"There are {power} plotters!");
                    var plotterWindow = new EmbedBuilder()
                    {
                        Title = "Uh oh, there's a plot to kill the Caesar!",
                        Description =
                            $"There are currently {power} plotters, and {senate.Count} senators!",
                        Color = Color.Blue,
                        ImageUrl = Configuration.Plot,
                    };
                    await ReplyAsync(embed: plotterWindow.Build());
                }
            }
        }

        [Command("startplot")]
        public async Task StartPlot()
        {
            Console.WriteLine($"{Context.User.Username} is trying to execute the plot to assassinate the Caesar!");
        }
    }
}