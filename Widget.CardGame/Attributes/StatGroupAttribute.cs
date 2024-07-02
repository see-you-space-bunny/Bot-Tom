using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Widget.CardGame.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class StatGroupAttribute : Attribute
    {
    // See the attribute guidelines at
    //  http://go.microsoft.com/fwlink/?LinkId=85236
    readonly CharacterStatGroup _statGroup;
    
    // This is a positional argument
    public StatGroupAttribute(CharacterStatGroup statGroup)
    {
        this._statGroup = statGroup;
        
        // TODO: Implement code here
        /* throw new System.NotImplementedException(); */
    }
    
    public CharacterStatGroup StatGroup => _statGroup;
    
    // This is a named argument
    /* public int NamedInt { get; set; } */
    }
}