using System;
using System.Collections.Generic;
using System.Management.Automation;
using TcgSdk.Common;
using TcgSdk.Magic;
using TcgSdk.Pokemon;

namespace PSTcgSdkCompiled
{
    /// <summary>
    /// Get a response object matching the input parameters
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "ITcgResponse")]
    public class GetITcgResponse : PSCmdlet
    {
        private int pageNumber = 1;
        private int pageSize = 100;

        /// <summary>
        /// The response type you are requesting
        /// </summary>
        [Parameter(
            Mandatory = true,
            Position = 0)]
        public TcgSdkResponseType ResponseType { get; set; }

        /// <summary>
        /// The list of parameters
        /// </summary>
        [Parameter(
            Mandatory = true,
            Position = 1)]
        public IEnumerable<TcgSdkRequestParameter> Parameters { get; set; }

        /// <summary>
        /// The page number of your request. Defaults to 1.
        /// </summary>
        [Parameter(
            Mandatory = false,
            Position = 2)]
        public int PageNumber { get { return pageNumber; } set { pageNumber = value; } }

        /// <summary>
        /// The page size of your request. Should not be greater than 1000 for Pokemon, and 100 for Magic. Defaults to 100.
        /// </summary>
        [Parameter(
            Mandatory = false,
            Position = 3)]
        [ValidateRange(1, 1000)]
        public int PageSize { get { return pageSize; } set { pageSize = value; } }

        protected override void BeginProcessing()
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ResponseType.ToString().ToUpper(), "MAGIC"))
            {
                var errorRecord = new ErrorRecord(
                    new ArgumentOutOfRangeException(
                        string.Format(
                            "ITcgCardType {0} only supports a page size of up to 100", ResponseType)),
                    "ArgumentOutOfRange", ErrorCategory.InvalidArgument, ResponseType);

                WriteError(errorRecord);
                return;
            }

            ITcgSdkObject[] responseObjects = null;

            switch (ResponseType)
            {
                case TcgSdkResponseType.PokemonCard:
                    {
                        responseObjects = ITcgSdkResponseFactory<PokemonCard>.Get(TcgSdkResponseType.PokemonCard, Parameters, PageNumber, PageSize).Cards;
                        break;
                    }
                case TcgSdkResponseType.MagicCard:
                    {
                        responseObjects = ITcgSdkResponseFactory<PokemonCard>.Get(TcgSdkResponseType.MagicCard, Parameters, PageNumber, PageSize).Cards;
                        break; ;
                    }
                case TcgSdkResponseType.PokemonSet:
                    {
                        responseObjects = ITcgSdkResponseFactory<PokemonSet>.Get(TcgSdkResponseType.PokemonSet, Parameters, PageNumber, PageSize).Sets;
                        break; ;
                    }
                case TcgSdkResponseType.MagicSet:
                    {
                        responseObjects = ITcgSdkResponseFactory<MagicSet>.Get(TcgSdkResponseType.PokemonSet, Parameters, PageNumber, PageSize).Sets;
                        break; ;
                    }
                default:
                    {
                        var errorRecord = new ErrorRecord(
                            new NotImplementedException(
                                string.Format(
                                    "ITcgCardType {0} not implemented", ResponseType)), 
                            "NotImplemented", ErrorCategory.NotImplemented, ResponseType);

                        WriteError(errorRecord);
                        return;
                    }
            }

            foreach (ITcgSdkObject item in responseObjects)
            {
                WriteObject(item);
            }
        }
    }
}
