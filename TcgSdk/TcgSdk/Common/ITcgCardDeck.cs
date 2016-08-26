using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcgSdk.Common
{
    /// <summary>
    /// Representation of a deck of ITcgCards.
    /// </summary>
    /// <typeparam name="T">The type of cards to work with.</typeparam>
    public class ITcgCardDeck<T>
    {
        /// <summary>
        /// All cards in the deck
        /// </summary>
        public IEnumerable<T> AllCards { get; private set; }
        /// <summary>
        /// Remaining cards after draws
        /// </summary>
        public IEnumerable<T> RemainingCards { get; private set; }

        // Constructors

        /// <summary>
        /// Construct an empty ITcgCardDeck object.
        /// </summary>
        private ITcgCardDeck()
        {
            AllCards = new List<T>();
            RemainingCards = new List<T>();
        }

        /// <summary>
        /// Construct an ITcgCardDeck object from a list of ITcgCard objects.
        /// </summary>
        public ITcgCardDeck(IEnumerable<T> cards)
        {
            AllCards = cards;
            RemainingCards = cards;
        }

        // Deck methods

        /// <summary>
        /// Draw cards from the remaining cards in the deck.
        /// </summary>
        /// <param name="numberOfCards">The number of cards to draw.</param>
        /// <returns></returns>
        public IEnumerable<T> DrawCards(int numberOfCards)
        {
            List<T> cardsToReturn = new List<T>();

            List<T> workingCardArray = (List<T>)RemainingCards;

            for (int i = 0; i < numberOfCards; i++)
            {
                int cardNumber = Utility.GetRandomInt(0, workingCardArray.Count());

                T card = workingCardArray[cardNumber];

                cardsToReturn.Add(workingCardArray[cardNumber]);

                workingCardArray.Remove(card);                
            }

            RemainingCards = workingCardArray;

            return cardsToReturn;
        }

        /// <summary>
        /// Reset the deck to baseline (Sets RemainingCards equal to AllCards)
        /// </summary>
        public void ResetDeck()
        {
            RemainingCards = AllCards;
        }
    }


}
