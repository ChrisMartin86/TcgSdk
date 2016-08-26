namespace TcgSdk.Common
{
    internal class ITcgCardResponse<T>
    {
        /// <summary>
        /// Array of requested cards.
        /// </summary>
        public T[] Cards { get; set; }

        private ITcgCardResponse()
        {
            
        }
    }
}
