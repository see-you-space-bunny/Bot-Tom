using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTom
{
    /// <summary>
    /// Class <c>EclipsePhaseDiceModule</c> 
    /// (<see cref="DSharpPlus.SlashCommands.ApplicationCommandModule"/>).<br/>
    /// <br/>
    /// Accepts and handles commands passed to the bot by users?
    /// </summary>
    internal class BotTomDiceModule : ApplicationCommandModule
    {
        // ----------------------------------------------------------------------------------
        /// <summary>
        /// Task <c>DiceCommand</c>, that is called by the user with 
        /// the slash-command "roll".
        /// </summary>
        /// <param name="context"><c>InteractionContext</c> ???</param>
        /// <param name="targetNumber">An <c>int</c> representing the Test/Check target 
        /// number that the user needs to match or roll under.</param>
        /// <param name="opponentTargetNumber">An <c>int</c> (or <c>null</c>) which contains 
        /// the opposition's <c>targetNumber</c> in an opposed test. This not being 
        /// <c>null</c> switches the output to opposed test outputs.</param>
        /// <param name="label">A <c>string</c> (or <c>null</c>) containing markdown-
        /// formatted text.</param>
        /// <param name="damage">A <c>string</c> (or <c>null</c>) representing a math-
        /// parseable dice-string.</param>
        /// <returns>A response object containing a markdown-formatted string that 
        /// represents the roll outcome.</returns>
        /// <remarks><h2>Remarks</h2><br/><i>
        /// <c>InteractionContext</c> appears to be both the user writing the 
        /// command, but also executing the command.<br/>
        /// <br/>
        /// The <c>Option</c> objects are positional arguments that also 
        /// contain the prompt information displayed to the user.<br/>
        /// <br/>
        /// The <c>Option</c> objects with a <c>null</c> are optional.
        /// </i></remarks>
        [SlashCommand("pf", "Roll a d20 for Pathfinder. Adding a Difficulty Class (DC) will display degree of success.")]
        internal Task PathfinderDice(
            InteractionContext context,
            [Option("dm", "The bonus or penalty you have to your roll.")] long diceModifier,
            [Option("dc", "An optional field to specify the roll's Difficulty Class (DC).")] long? difficultyClass = null,
            [Option("specialdc", "An optional field to set the Difficulty Class (DC) from \"level\" or \"spell\".")] string? specialDC = null,
            [Option("label", "An optional label to identify what the roll is for.")] string? label = null//,
            //[Option("secret", "WIP An optional field to put in the GM's discord user-id. It sends the result to them in secret.")] string? secret = null
            )
        {
            Tuple<PathfinderRoll.DifficultyClassType,long?>? difficultyClassTup;

            if (difficultyClass == null)
            {
                difficultyClassTup = null;
            }
            else if (!(specialDC == null))
            {
                PathfinderRoll.DifficultyClassType dct;
                if (specialDC.Trim() == "level")
                    dct = PathfinderRoll.DifficultyClassType.Level;
                else if (specialDC.Trim() == "spell")
                    dct = PathfinderRoll.DifficultyClassType.SpellLevel;
                else
                    dct = PathfinderRoll.DifficultyClassType.None;

                difficultyClassTup = new Tuple<PathfinderRoll.DifficultyClassType,long?> (dct, difficultyClass);
            }
            else
            {
                difficultyClassTup = new Tuple<PathfinderRoll.DifficultyClassType,long?> (PathfinderRoll.DifficultyClassType.Basic, difficultyClass);
            }

            var p2e_r = new PathfinderRoll(diceModifier, difficultyClassTup, label);
            p2e_r.Roll();

            return context.CreateResponseAsync(p2e_r.ToString());
        }
    }    
}