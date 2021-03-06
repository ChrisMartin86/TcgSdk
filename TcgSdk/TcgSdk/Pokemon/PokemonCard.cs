﻿using TcgSdk.Common;
using TcgSdk.Common.Cards;
using System;
using System.Collections.Generic;
    


namespace TcgSdk.Pokemon
{
    /// <summary>
    /// Pokemon card from https://api.pokemontcg.io
    /// </summary>
    public class PokemonCard : ITcgSdkObject, ITcgCard
    {
        /// <summary>
        /// The artist of the card.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// The Pokemon's attacks
        /// </summary>
        public Attack[] Attacks { get; set; }

        /// <summary>
        /// The type of response. Always returns TcgSdkResponseType.PokemonCard for this class.
        /// </summary>
        public TcgSdkResponseType ResponseType { get { return TcgSdkResponseType.PokemonCard; } }

        /// <summary>
        /// The hit points of the card. This typically appears in the top right corner of the card.
        /// </summary>
        public string HP { get; set; }


        /// <summary>
        /// A unique id for this card. It is made up by taking the set code and concatenating the card number to it. (ex. xy1-1)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The URL to the image of the card
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The name of the card
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The national pokedex number for a card that features a Pokémon supertype
        /// </summary>
        public int? NationalPokedexNumber { get; set; }

        /// <summary>
        /// The number of the card for the set it was released in. Found on the bottom right side of the card.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// The rarity of the card (ex. Rare, Rare Holo, Common, etc.)
        /// </summary>
        public string Rarity { get; set; }

        /// <summary>
        /// The resistances of the card (ex. Fire, Water, Grass)
        /// </summary>
        public Resistance[] Resistances { get; set; }

        /// <summary>
        /// The amount of energy it takes to retreat. Found on the bottom of the card.
        /// </summary>
        public string[] RetreatCost { get; set; }

        /// <summary>
        /// The series the card appears in (ex. Base, XY, EX, etc.)
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        ///	The set the card appears in (ex. BREAKthrough, Phantom Forces, Jungle, etc.)
        /// </summary>
        public string Set { get; set; }

        /// <summary>
        ///	The unique code of the set (ex. base1, xy5, ex3)
        /// </summary>
        public string SetCode { get; set; }

        /// <summary>
        ///	The subtype of the card. Examples include MEGA, Stage 1, BREAK, Supporter, etc.
        /// </summary>
        public string SubType { get; set; }

        /// <summary>
        /// The supertype of the card. Either Pokémon, Trainer, or Energy.
        /// </summary>
        public string SuperType { get; set; }

        /// <summary>
        /// Any additional text on a card. This includes special rules (like MEGA rules or EX), and text that appears on Trainer cards.
        /// </summary>
        public string[] Text { get; set; }

        /// <summary>
        /// The types of the card. These typically appear in the top right of card, and are denoted by energy symbol (ex. Fire, Fighting, Psychic, etc.)
        /// </summary>
        public string[] Types { get; set; }

        /// <summary>
        /// The weaknesses of the card (ex. Fire, Water, Grass)
        /// </summary>
        public Weakness[] Weaknesses { get; set; }

        /// <summary>
        /// Return name of the card
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Keep the constructor internal.
        /// </summary>
        internal PokemonCard()
        {

        }

        /// <summary>
        /// Attack data
        /// </summary>
        public struct Attack
        {
            public int ConvertedEnergyCost { get; set; }
            public string[] Cost { get; set; }
            public string Damage { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
        }
        /// <summary>
        /// Weakness data
        /// </summary>
        public struct Weakness
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }
        /// <summary>
        /// Resistance data
        /// </summary>
        public struct Resistance
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }

        public IEnumerable<PokemonSet> GetSets()
        {
            try
            {
                var requestParameters = new TcgSdkRequestParameter("name", SetCode, false, false);

                var response = ITcgSdkResponseFactory<PokemonSet>.Get(TcgSdkResponseType.PokemonSet, new TcgSdkRequestParameter[] { requestParameters });

                return response.Sets;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
