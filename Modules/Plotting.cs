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
        private string plotterList, senatorList;
        public int Aye, Nay;

        [Command("plot join")]
        public async Task Plot()
        {
            var senator = Configuration.Senator;

            foreach (var member in Context.Guild.GetUsersAsync().Result)
            foreach (var role in member.RoleIds)
                if (role == senator)
                {
                    senate.Add(member);
                    senatorList += member.Username + ",";
                }

            foreach (SocketRole role in ((SocketGuildUser) Context.Message.Author).Roles)
            {
                if (role.Id == senator)
                {
                    plotters.Add(Context.User.Username);
                    foreach (var member in plotters)
                        plotterList += member + ", ";

                    power = plotters.Count;
                    Console.WriteLine($"{Context.Message.Author.Username} has joined a plot!");
                    Console.WriteLine($"There are currently {power} plotters, and {senate.Count} senators!");
                    Console.WriteLine($"The members of the plot are: {plotterList} ");
                    Console.WriteLine($"The members of senate are: {senatorList} ");
                    var plotterWindow = new EmbedBuilder()
                    {
                        Title = "Uh oh, there's a plot to kill the Caesar!",
                        Description =
                            $"There are currently **{power} plotters**, and **{senate.Count} senators!**",
                        Color = Color.Blue,
                        ImageUrl = Configuration.Plot,
                    };
                    await ReplyAsync(embed: plotterWindow.Build());
                }
            }
        }

        [Command("plot start")]
        public async Task StartPlot()
        {
            Console.WriteLine($"{Context.User.Username} started a vote with {plotters.Count} plotters!");
            var plotVoteWindow = new EmbedBuilder()
            {
                Title = "Where does your loyalty lie?",
                Description =
                    "The time has come to act. The Caesar must die. Type plot yes/no to choose if now is the time to move forward with the plot.",
                ImageUrl = Configuration.PlotVote,
            };
            await ReplyAsync(embed: plotVoteWindow.Build());
        }

        [Command("plot yes")]
        public async Task PlotYes()
        {
            Aye++;
            await ReplyAsync($"Aye's: {Aye} | Nay's: {Nay}");
        }
        
        [Command("plot no")]
        public async Task PlotNo()
        {
            Nay++;
            await ReplyAsync($"Aye's: {Aye} | Nay's: {Nay}");
        }
    }
}