using System;

namespace TcgSdk.Common
{
    internal class TcgSdkResponse<T> : ITcgSdkResponse<T>
    {
        /// <summary>
        /// Array of requested cards.
        /// </summary>
        public T[] Cards { get; set; }

        /// <summary>
        /// Array of requested sets.
        /// </summary>
        public T[] Sets { get; set; }

        private TcgSdkResponse()
        {
            
        }

        internal class TcgSdkResponseException : Exception
        {
            public TcgSdkResponseException(Exception e = null) : base("There was a problem getting a response from the api.", e)
            {

            }
        }
    }
}
