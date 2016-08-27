using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TcgSdk.Common.Cards
{
    /// <summary>
    /// Represents a collection of cards
    /// </summary>
    class ITcgCardCollection
    {
        /// <summary>
        /// All cards, including those in decks
        /// </summary>
        public IDictionary<ITcgCard, int> AllCards { get; set; }
        /// <summary>
        /// All cards not currently in decks
        /// </summary>
        public IDictionary<ITcgCard, int> CardsNotInDecks { get { return getCardsNotInDecks(); } }
        /// <summary>
        /// Cards in decks
        /// </summary>
        public IEnumerable<ITcgCardDeck> Decks { get; set; }
        /// <summary>
        /// The count of all cards in the collection
        /// </summary>
        public int AllCardsCount { get { return TcgSdkUtility.getCount(AllCards); } }

        /// <summary>
        /// Create a new collection from a list of cards and decks. Deck cards should be included in the list of cards.
        /// </summary>
        /// <param name="cards">The list of cards, including all of those in decks. This parameter can be null as long as decks is not null.</param>
        /// <param name="decks">The decks to add. Cards should be included in cards parameter.</param>
        public ITcgCardCollection(IDictionary<ITcgCard, int> cards = null, IEnumerable<ITcgCardDeck> decks = null)
        {
            if ((null == cards) && (null != decks))
                throw new NullReferenceException("cards cannot be null if the input contains one or more decks. Add the cards from the deck to the collection.");

            AllCards = cards ?? new Dictionary<ITcgCard, int>();
            Decks = decks ?? new List<ITcgCardDeck>();
        }

        /// <summary>
        /// Create a new collection by importing
        /// </summary>
        /// <param name="pathToExportedCollection"></param>
        public ITcgCardCollection(string pathToExportedCollection)
        {
            if (string.IsNullOrWhiteSpace(pathToExportedCollection))
                throw new NullReferenceException("pathToExportedCollection must not be null or empty.");

            if (!File.Exists(pathToExportedCollection))
                throw new CollectionNotFoundException(pathToExportedCollection);

            string fileContents;

            try
            {
                fileContents = File.ReadAllText(pathToExportedCollection);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Unable to read file contents at {0}", pathToExportedCollection), e);
            }

            ITcgCardCollection collection = null;
            try
            {
                collection = JsonConvert.DeserializeObject<ITcgCardCollection>(fileContents);
            }
            catch (Exception e)
            {
                throw new Exception("There was a problem deserializing the file contents.", e);
            }

            if ((0 == collection.AllCardsCount) && (0 != collection.Decks.Count()))
                throw new InvalidInitialCollectionConfigurationException("cards cannot be null if the input contains one or more decks. Add the cards from the deck to the collection.");

            AllCards = collection.AllCards;
        }

        public void ExportCollection(string pathToExportCollection)
        {
            if (string.IsNullOrWhiteSpace(pathToExportCollection))
                throw new NullReferenceException("pathToExportCollection must not be null or empty.");

            string serializedObjectString;

            try
            {
                serializedObjectString = JsonConvert.SerializeObject(this);
            }
            catch (Exception e)
            {
                throw new Exception("There was a problem serializing the object.", e);
            }

            try
            {
                File.WriteAllText(pathToExportCollection, serializedObjectString);
            }
            catch (Exception e)
            {
                throw new Exception("There was a problem writing the serialized object to disk.", e);
            }



        }

        public static ITcgCardCollection ImportCollection(string pathToExportedCollection)
        {
            return new ITcgCardCollection(pathToExportedCollection);
        }

        public void AddCards(IDictionary<ITcgCard, int> cards)
        {
            foreach (var item in cards)
            {
                try
                {
                    for (int i = 0; i < item.Value; i++)
                    {
                        try
                        {
                            AllCards.Add(item.Key, 1);
                        }
                        catch
                        {
                            AllCards[item.Key]++;
                        }
                    }
                }
                catch (Exception e)
                {
                    // Serious problem with my logic, this shouldn't get hit ever, but just in case.
                    throw new Exception("Your developer has made a grave mistake.", e);
                }
            }
        }

        public void AddDecks(IEnumerable<ITcgCardDeck> decks, bool fromCollection = false)
        {
            List<ITcgCardDeck> deckList = (List<ITcgCardDeck>)Decks;

            foreach (var item in decks)
            {
                if (!fromCollection)
                {
                    AddCards(item.AllCards);
                }

                IDictionary<ITcgCard, int> invalidCards = validateDeck(item);

                if (invalidCards.Count > 0)
                    throw new CardNotInCollectionException(invalidCards);

                deckList.Add(item);
            }

            Decks = deckList;
        }

        /// <summary>
        /// Helper method for CardsNotInDecks property.
        /// </summary>
        /// <returns>All cards not currently in decks.</returns>
        private IDictionary<ITcgCard, int> getCardsNotInDecks()
        {
            if (null == AllCards)
                return null;

            if (null == Decks)
                return AllCards;

            List<ITcgCard> cardList = TcgSdkUtility.dictToList(AllCards);

            foreach (ITcgCardDeck deck in Decks)
            {
                var deckCardList = TcgSdkUtility.dictToList(deck.AllCards);

                foreach (ITcgCard card in deckCardList)
                {
                    AllCards[card]--;
                }
            }

            return (IDictionary<ITcgCard, int>)AllCards.Where(c => c.Value > 0);
        }

        private IDictionary<ITcgCard, int> validateDeck(ITcgCardDeck deck)
        {
            IDictionary<ITcgCard, int> invalidCards = new Dictionary<ITcgCard, int>();

            IDictionary<ITcgCard, int> workingCards = AllCards;

            foreach (var item in deck.AllCards)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    try
                    {
                        workingCards[item.Key]--;
                    }
                    catch
                    {
                        try
                        {
                            invalidCards.Add(item.Key, 1);
                        }
                        catch
                        {
                            try
                            {
                                invalidCards[item.Key]++;
                            }
                            catch (Exception e)
                            {
                                // Serious problem with my logic, this shouldn't get hit ever, but just in case.
                                throw new Exception("Your developer has made a grave mistake.", e);
                            }
                        }
                    }
                }
            }

            return invalidCards;
        }

        public class CardNotInCollectionException : Exception
        {
            public IDictionary<ITcgCard, int> InvalidCards { get; private set; }

            public CardNotInCollectionException(IDictionary<ITcgCard, int> invalidCards) : base("Deck card not in collection.")
            {
                InvalidCards = invalidCards;
            }
        }

        public class InvalidInitialCollectionConfigurationException : Exception
        {
            public InvalidInitialCollectionConfigurationException(string message) : base(message)
            {

            }
        }

        public class CollectionNotFoundException : Exception
        {
            public CollectionNotFoundException(string path) : base(string.Format("Collection not found at {0}", path))
            {

            }
        }
    }


}
