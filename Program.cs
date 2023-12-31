﻿using System.Diagnostics;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.VisualBasic;
using org.mariuszgromada.math.mxparser;

namespace BotTom
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            Envy.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

            if(Convert.ToBoolean(Environment.GetEnvironmentVariable("OFFLINE_DEBUG")))
            {
                StorytellerRoll sta_r;

                sta_r = new StorytellerRoll(6,8,10,5,0,false,"testing 1!");
                sta_r.Roll();
                Console.WriteLine(sta_r.ToString());


                sta_r = new StorytellerRoll(6,8,10,3,0,false,"testing 2!");
                sta_r.Roll();
                Console.WriteLine(sta_r.ToString());


                sta_r = new StorytellerRoll(6,8,10,5,0,true,"testing 3!");
                sta_r.Roll();
                Console.WriteLine(sta_r.ToString());
            }
            else 
            {
                var discordClient = new DiscordClient(new DiscordConfiguration
                {
                    Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMessages | DiscordIntents.MessageContents
                });
                
                // ----------------------------------------------------------------------------------
                // mXparser required
                // Non-Commercial Use Confirmation
                bool isCallSuccessful = License.iConfirmNonCommercialUse(Environment.GetEnvironmentVariable("USER_NAME"));
                
                // Verification if use type has been already confirmed
                bool isConfirmed = License.checkIfUseTypeConfirmed();
                
                // Checking use type confirmation message
                String message = License.getUseTypeConfirmationMessage();
                
                // ----------------------------------------------------------------------------------
                Console.WriteLine("isCallSuccessful = " + isCallSuccessful);
                Console.WriteLine("isConfirmed = " + isConfirmed);
                Console.WriteLine("message = " + message);
                // ----------------------------------------------------------------------------------

                var slash = discordClient.UseSlashCommands();
                slash.RegisterCommands<BotTomDiceModule>();

                await discordClient.ConnectAsync();
                await Task.Delay(-1);
            }
        }
    }
}

