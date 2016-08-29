using System.Collections.Generic;

namespace TcgSdk.Common.Cards
{
    /// <summary>
    /// Representation of a deck of ITcgCards.
    /// </summary>
    /// <typeparam name="T">The type of cards to work with.</typeparam>
    public class ITcgCardDeck
    {
        /// <summary>
        /// The name of the deck
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// All cards in the deck
        /// </summary>
        public IDictionary<ITcgCard, int> AllCards { get; private set; }
        /// <summary>
        /// Remaining cards after draws
        /// </summary>
        public IDictionary<ITcgCard, int> RemainingCards { get; private set; }
        /// <summary>
        /// The count of all cards in the deck
        /// </summary>
        public int AllCardsCount { get { return TcgSdkUtility.getCount(AllCards); } }
        /// <summary>
        /// The count of cards remaining in the deck
        /// </summary>
        public int RemainingCardsCount { get { return TcgSdkUtility.getCount(RemainingCards); } }

        // Constructors

        /// <summary>
        /// Construct an empty ITcgCardDeck object.
        /// </summary>
        private ITcgCardDeck()
        {
            AllCards = new Dictionary<ITcgCard, int>();
            RemainingCards = new Dictionary<ITcgCard, int>();
        }

        /// <summary>
        /// Construct an ITcgCardDeck object from a dictionary of ITcgCard objects and the number of each card in the deck.
        /// </summary>
        public ITcgCardDeck(string name, IDictionary<ITcgCard, int> cards)
        {
            Name = name;
            AllCards = cards;
            RemainingCards = cards;
        }

        public ITcgCardDeck(string name, IEnumerable<ITcgCard> cards)
        {
            Name = name;
            AllCards = TcgSdkUtility.listToDict((List<ITcgCard>)cards);
            RemainingCards = AllCards;
        }

        // Deck methods

        /// <summary>
        /// Draw cards from the remaining cards in the deck.
        /// </summary>
        /// <param name="numberOfCards">The number of cards to draw.</param>
        /// <returns></returns>
        public IDictionary<ITcgCard, int> DrawCards(int numberOfCards)
        {
            List<ITcgCard> cardsToReturn = new List<ITcgCard>();

            List<ITcgCard> workingCardList = TcgSdkUtility.dictToList(RemainingCards);

            for (int i = 0; i < numberOfCards; i++)
            {
                int cardNumber = TcgSdkUtility.GetRandomInt(1, (workingCardList.Count + 1));

                ITcgCard cardToReturn = workingCardList[cardNumber];

                cardsToReturn.Add(cardToReturn);

                workingCardList.Remove(cardToReturn);
            }

            RemainingCards = TcgSdkUtility.listToDict(workingCardList);

            return TcgSdkUtility.listToDict(cardsToReturn);
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
