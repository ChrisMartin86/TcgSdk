using System;
using System.Collections.Generic;
using System.Management.Automation;
using TcgSdk.Common;
using TcgSdk.Common.Cards;
using TcgSdk.Magic;
using TcgSdk.Pokemon;

namespace PSTcgSdkCompiled
{
    [Cmdlet(VerbsCommon.Get, "ITcgCards")]
    public class GetITcgCards : PSCmdlet
    {
        private TcgSdkResponseType responseType = TcgSdkResponseType.MagicCard;
        private IEnumerable<TcgSdkRequestParameter> parameters = new List<TcgSdkRequestParameter>();

        [Parameter(
            Mandatory = true,
            Position = 0)]
        public TcgSdkResponseType ResponseType { get { return responseType; } set { responseType = value; } }

        [Parameter(
            Mandatory = true,
            Position = 1)]
        public IEnumerable<TcgSdkRequestParameter> Parameters { get; set; }

        protected override void BeginProcessing()
        {
            IDictionary<ITcgCard, int> cards = new Dictionary<ITcgCard, int>();

            switch (ResponseType)
            {
                case TcgSdkResponseType.PokemonCard:
                    {
                        cards = (IDictionary<ITcgCard, int>)ITcgSdkResponseFactory<PokemonCard>.Get(TcgSdkResponseType.PokemonCard, Parameters);

                        foreach (var item in cards)
                        {
                            for (int i = 0; i < item.Value; i++)
                            {
                                WriteObject(item.Key);
                            }
                        }
                        return;
                    }
                case TcgSdkResponseType.MagicCard:
                    {
                        cards = (IDictionary<ITcgCard, int>)ITcgSdkResponseFactory<MagicCard>.Get(TcgSdkResponseType.MagicCard, Parameters);

                        foreach (var item in cards)
                        {
                            for (int i = 0; i < item.Value; i++)
                            {
                                WriteObject(item.Key);
                            }
                        }

                        return;
                    }
                default:
                    {
                        var exception = new NotImplementedException(string.Format("ITcgCardType {0} not implemented", ResponseType));

                        var errorRecord = new ErrorRecord(exception, "NotImplemented", ErrorCategory.NotImplemented, ResponseType);

                        WriteError(errorRecord);

                        return;
                    }
            }
        }
    }
}
