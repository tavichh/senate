using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Senate.Modules
{
    public class Commands : ModuleBase<CommandContext>
    {
        private int power;
        private List<string> plotters = new List<string>();

        [Command("plot")]
        public async Task Plot()
        {
            plotters.Add(Context.User.Username);
            Console.WriteLine($"{Context.User.Username} has joined a plot!");
            Console.WriteLine($"There are {plotters.Count} plotters!");
            await ReplyAsync($"The plot to assassinate the Caesar has {plotters.Count} backers!");
        }

        public async Task Initiate()
        {
            
        }
    }
}