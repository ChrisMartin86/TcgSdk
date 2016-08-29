using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using TcgSdk.Common;

namespace PSTcgSdkCompiled
{
    [Cmdlet(VerbsCommon.New, "TcgSdkRequestParameter")]
    public class NewTcgSdkRequestParameter : PSCmdlet
    {
        private string name;

        /// <summary>
        /// The name of the parameter.
        /// </summary>
        [Parameter(
            Mandatory = true,
            Position = 0)]
        public string Name { get; set; }
        /// <summary>
        /// The value of the parameter.
        /// </summary>
        [Parameter(
            Mandatory = true,
            Position = 1)]
        public object Value { get; set; }
        /// <summary>
        /// Use for multivalue parameters to use the AND operator instead of the OR operator.
        /// </summary>
        [Parameter(
            Mandatory = false)]
        public SwitchParameter UseAnd { get; set; }
        /// <summary>
        /// Use for multivalue parameters.
        /// </summary>
        [Parameter(
            Mandatory = false)]
        public SwitchParameter MultiValue { get; set; }

        protected override void BeginProcessing()
        {
            WriteObject(TcgSdkRequestParameter.New(Name, Value, UseAnd, MultiValue));
        }
    }
}
