using System;
using TcgSdk.Common;
using System.Collections.Generic;


namespace TcgSdk.Magic
{
    public class MagicSet : ITcgSdkObject 
    {
        /// <summary>
        /// The name of the set
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The code name of the set
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// The type of set
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The type of border on the cards, either “white”, “black” or “silver”
        /// </summary>
        public string Border { get; set; }
        public string Mkm_Name { get; set; }
        public string Mkm_Id { get; set; }
        /// <summary>
        /// When the set was released (YYYY-MM-DD). For promo sets, the date the first card was released.
        /// </summary>
        public DateTime? ReleaseDate { get; set; }
        /// <summary>
        ///  	The code that Gatherer uses for the set. Only present if different than ‘code’
        /// </summary>
        public string GathererCode { get; set; }
        /// <summary>
        /// The code that magiccards.info uses for the set. Only present if magiccards.info has this set
        /// </summary>
        public string MagicCardsInfoCode { get; set; }
        /// <summary>
        /// The TcgSdkResponseType. Always returns TcgSdkResponseType.MagicSet for this object.
        /// </summary>
        public TcgSdkResponseType ResponseType { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public IEnumerable<MagicCard> GetCardsInSet()
        {
            try
            {
                var requestParameters = new TcgSdkRequestParameter("set", Code, false, false);

                var cards = ITcgSdkResponseFactory<MagicCard>.Get(TcgSdkResponseType.MagicCard, new TcgSdkRequestParameter[] { requestParameters });

                return cards.Cards;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}