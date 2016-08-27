namespace TcgSdk.Common.Cards
{
    /// <summary>
    /// Common interface for all supported cards
    /// </summary>
    public interface ITcgCard
    {
        /// <summary>
        /// The artist of the card. This may not match what is on the card as MTGJSON corrects many card misprints.
        /// </summary>
        string Artist { get; }
        /// <summary>
        /// The type of response/card
        /// </summary>
        TcgSdkResponseType ResponseType { get; }

        /// <summary>
        /// The URL to the image of the card
        /// </summary>
        string ImageUrl { get; }

        /// <summary>
        /// The name of the card
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The card number. This is printed at the bottom-center of the card in small text. This is a string, not an integer, because some cards have letters in their numbers.
        /// </summary>
        string Number { get; }

        /// <summary>
        /// The rarity of the card. Examples: Common, Uncommon, Rare, Mythic Rare, Special, Basic Land
        /// </summary>
        string Rarity { get; }

        /// <summary>
        ///	The set the card appears in (ex. BREAKthrough, Phantom Forces, Jungle, etc.)
        /// </summary>
        string Set { get; set; }

        /// <summary>
        /// Must override the ToString method to make lists pretty
        /// </summary>
        /// <returns>Name of card</returns>
        string ToString();
    }

}
