using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcgSdk.Pokemon
{
    /// <summary>
    /// A Pokemon attack
    /// </summary>
    public class PokemonAttack
    {
        /// <summary>
        /// The specific energy required for this attack.
        /// </summary>
        public string[] Cost { get; set; }
        /// <summary>
        /// The name of the attack.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The text of the attack.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// The amount of damage done by the attack.
        /// </summary>
        public string Damage { get; set; }
        /// <summary>
        /// The total energy cost of the attack.
        /// </summary>
        public int? ConvertedEnergyCost { get; set; }
    }
}
