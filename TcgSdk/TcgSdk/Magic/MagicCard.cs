using TcgSdk.Common;
using TcgSdk.Common.Cards;
using System;
using System.Collections.Generic;

namespace TcgSdk.Magic
{
    /// <summary>
    /// Magic the Gathering card from https://api.magicthegathering.io
    /// </summary>
    public class MagicCard : ITcgSdkObject, ITcgCard
    {
        /// <summary>
        /// The artist of the card. This may not match what is on the card as MTGJSON corrects many card misprints.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// The type of response. Always returns TcgSdkResponseType.MagicCard for this class.
        /// </summary>
        public TcgSdkResponseType ResponseType { get { return TcgSdkResponseType.MagicCard; } }

        /// <summary>
        /// Converted mana cost. Always a number.
        /// </summary>
        public int CMC { get; set; }

        /// <summary>
        /// The card colors. Usually this is derived from the casting cost, but some cards are special (like the back of dual sided cards and Ghostfire).
        /// </summary>
        public string[] Colors { get; set; }

        /// <summary>
        /// The flavor text of the card.
        /// </summary>
        public string Flavor { get; set; }

        /// <summary>
        /// A unique id for this card. It is made up by doing an SHA1 hash of setCode + cardName + cardImageName
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The image url for a card. Only exists if the card has a multiverse id.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The card layout. Possible values: normal, split, flip, double-faced, token, plane, scheme, phenomenon, leveler, vanguard
        /// </summary>
        public string Layout { get; set; }

        /// <summary>
        /// Which formats this card is legal, restricted or banned in. An array of objects, each object having 'format’ and 'legality’.
        /// </summary>
        public _Legality[] Legalities { get; set; }

        /// <summary>
        /// The loyalty of the card. This is only present for planeswalkers.
        /// </summary>
        public string Loyalty { get; set; }

        /// <summary>
        /// The mana cost of this card. Consists of one or more mana symbols. (use cmc and colors to query)
        /// </summary>
        public string ManaCost { get; set; }

        /// <summary>
        /// The multiverseid of the card on Wizard’s Gatherer web page. Cards from sets that do not exist on Gatherer will NOT have a multiverseid. Sets not on Gatherer are: ATH, ITP, DKM, RQS, DPA and all sets with a 4 letter code that starts with a lowercase 'p’.
        /// </summary>
        public int MultiverseId { get; set; }

        /// <summary>
        /// The card name. For split, double-faced and flip cards, just the name of one side of the card. Basically each ‘sub-card’ has its own record.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Only used for split, flip and dual cards. Will contain all the names on this card, front or back.
        /// </summary>
        public string[] Names { get; set; }

        /// <summary>
        /// The card number. This is printed at the bottom-center of the card in small text. This is a string, not an integer, because some cards have letters in their numbers.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// The original text on the card at the time it was printed. This field is not available for promo cards.
        /// </summary>
        public string OriginalText { get; set; }

        /// <summary>
        /// The original type on the card at the time it was printed. This field is not available for promo cards.
        /// </summary>
        public string OriginalType { get; set; }

        /// <summary>
        /// The power of the card. This is only present for creatures. This is a string, not an integer, because some cards have powers like: “1+*”
        /// </summary>
        public string Power { get; set; }

        /// <summary>
        /// The sets that this card was printed in, expressed as an array of set codes.
        /// </summary>
        public string[] Printings { get; set; }

        /// <summary>
        /// The rarity of the card. Examples: Common, Uncommon, Rare, Mythic Rare, Special, Basic Land
        /// </summary>
        public string Rarity { get; set; }

        /// <summary>
        /// The set the card belongs to (set code).
        /// </summary>
        public string Set { get; set; }

        /// <summary>
        /// The set the card belongs to.
        /// </summary>
        public string SetName { get; set; }

        /// <summary>
        /// The subtypes of the card. These appear to the right of the dash in a card type. Usually each word is its own subtype. Example values: Trap, Arcane, Equipment, Aura, Human, Rat, Squirrel, etc.
        /// </summary>
        public string[] SubTypes { get; set; }

        /// <summary>
        /// The supertypes of the card. These appear to the far left of the card type. Example values: Basic, Legendary, Snow, World, Ongoing
        /// </summary>
        public string[] Supertypes { get; set; }

        /// <summary>
        /// The oracle text of the card. May contain mana symbols and other symbols.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The toughness of the card. This is only present for creatures. This is a string, not an integer, because some cards have toughness like: “1+*”
        /// </summary>
        public string Toughness { get; set; }

        /// <summary>
        /// The card type. This is the type you would see on the card if printed today. Note: The dash is a UTF8 'long dash’ as per the MTG rules
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The types of the card. These appear to the left of the dash in a card type. Example values: Instant, Sorcery, Artifact, Creature, Enchantment, Land, Planeswalker
        /// </summary>
        public string[] Types { get; set; }

        /// <summary>
        /// If a card has alternate art (for example, 4 different Forests, or the 2 Brothers Yamazaki) then each other variation’s multiverseid will be listed here, NOT including the current card’s multiverseid.
        /// </summary>
        public string[] Variations { get; set; }

        /// <summary>
        /// Return name of the card
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// The legality of the card.
        /// </summary>
        // Name conflict forced the underscore. I'm open to renaming suggestions -CM.
        public struct _Legality
        {
            public string Format { get; set; }
            public string Legality { get; set; }
        }
        /// <summary>
        /// Keep the constructor internal.
        /// </summary>
        internal MagicCard()
        {

        }

        public IEnumerable<MagicSet> GetSets()
        {
            try
            {
                var requestParameters = new TcgSdkRequestParameter("name", Set, false, false);

                var response = ITcgSdkResponseFactory<MagicSet>.Get(TcgSdkResponseType.MagicSet, new TcgSdkRequestParameter[] { requestParameters });

                return response.Sets;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
