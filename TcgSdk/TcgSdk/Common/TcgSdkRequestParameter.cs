using System;
using System.Collections.Generic;
using System.Text;

namespace TcgSdk.Common
{
    /// <summary>
    /// A request parameter for TcgSdk
    /// </summary>
    public class TcgSdkRequestParameter
    {
        /// <summary>
        /// Internal and switch
        /// </summary>
        private bool and_ = false;
        /// <summary>
        /// Internal multivalue switch
        /// </summary>
        private bool multiValue_ = false;
        /// <summary>
        /// Internal name variable
        /// </summary>
        internal string name_ = null;
        /// <summary>
        /// Internal value variable, before being cast to a usable type
        /// </summary>
        private object value_ = null;
        /// <summary>
        /// True or false if the request is validated already
        /// </summary>
        private bool validated = false;
        /// <summary>
        /// Populated when the value_ is cast to a string during a call to validate()
        /// </summary>
        private string singleValueParameterValue = string.Empty;
        /// <summary>
        /// Populated when the value_ is cast to a List of string during a call to validate()
        /// </summary>
        private IEnumerable<string> multiValueParameterValues = new List<string>();
        /// <summary>
        /// True or false if the request is confirmed, which means it is a valid parameter and is ready to be used.
        /// </summary>
        public bool Confirmed { get; private set; }

        /// <summary>
        /// Instantiate a new request parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter, either a string or a List of strings</param>
        /// <param name="useAnd">Use the and operator instead of the or operator for this request</param>
        /// <param name="multiValue">Signifies a multi-value parameter. Use if value is a List of strings</param>
        public TcgSdkRequestParameter(string name, object value, bool useAnd = false, bool multiValue = false)
        {
            name_ = name;
            value_ = value;
            and_ = useAnd;
            multiValue_ = multiValue;

            try
            {
                validate();
            }
            catch
            {
                throw;
            }

        }
        /// <summary>
        /// Instantiate a new request parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter, either a string or a List of strings</param>
        /// <param name="useAnd">Use the and operator instead of the or operator for this request</param>
        /// <param name="multiValue">Signifies a multi-value parameter. Use if value is a List of strings</param>
        public static TcgSdkRequestParameter New(string name, object value, bool useAnd = false, bool multiValue = false)
        {
            return new TcgSdkRequestParameter(name, value, useAnd, multiValue);
        }

        /// <summary>
        /// Create a new List of TcgSdkRequestParameter. Mostly useful in PowerShell because it has a hard time easily converting from single-object to multi-object.
        /// </summary>
        /// <returns>New List of TcgSdkRequestParameter</returns>
        public static List<TcgSdkRequestParameter> NewList()
        {
            return new List<TcgSdkRequestParameter>();
        }

        /// <summary>
        /// Validate the object and populate the inner parameter values
        /// </summary>
        private void validate()
        {
            if (validated)
                throw new ParameterAlreadyValidatedException();

            if ((name_.ToUpper() == "PAGENUMBER") || (name_.ToUpper() == "PAGESIZE"))
                throw new ParameterAlreadyExistsException(name_);

            try
            {
                if (multiValue_)
                {
                    multiValueParameterValues = (IEnumerable<string>)value_;
                }
                else
                {
                    singleValueParameterValue = (string)value_;
                }

                validated = true;
                Confirmed = true;

                return;
            }
            catch (Exception e)
            {
                validated = true;
                throw new InvalidParameterException(e);
            }

        }
        /// <summary>
        /// Get the url string filter of the parameter
        /// </summary>
        /// <returns>The url string filter of the parameter</returns>
        public override string ToString()
        {
            string nameValue = name_ ?? string.Empty;

            var sb = new StringBuilder();

            sb.Append(string.Format("{0}=", nameValue));

            if (multiValue_)
            {
                if (and_)
                {
                    foreach (string item in multiValueParameterValues)
                    {
                        string itemValue = item ?? string.Empty;

                        sb.Append(string.Format("{0},", itemValue));
                    }
                }
                else
                {
                    foreach (string item in multiValueParameterValues)
                    {
                        string itemValue = item ?? string.Empty;

                        sb.Append(string.Format("{0}|", itemValue));
                    }
                }

                sb.Remove(sb.Length - 1, 1);

            }
            else
            {
                sb.Append(singleValueParameterValue);
            }

            sb.Append("&");

            return sb.ToString();
        }

        /// <summary>
        /// Thrown when validate() is called on a parameter that's already been validated.
        /// </summary>
        public class ParameterAlreadyValidatedException : Exception
        {
            /// <summary>
            /// This instance has already been validated.
            /// </summary>
            public override string Message
            {
                get { return "This instance has already been validated."; }
            }
        }

        /// <summary>
        /// Thrown when a parameter is created with the same name as an already added parameter.
        /// </summary>
        public class ParameterAlreadyExistsException : Exception
        {
            public string ParameterName { get; private set; }

            /// <summary>
            /// A parameter with this name is already present.
            /// </summary>
            public override string Message
            {
                get { return string.Format("Parameter {0} already exists", ParameterName); }
            }

            public ParameterAlreadyExistsException(string parameterName)
            {
                ParameterName = parameterName;
            }
        }
        /// <summary>
        /// Thrown when a parameter is unable to be validated.
        /// </summary>
        public class InvalidParameterException : Exception
        {
            public IEnumerable<TcgSdkRequestParameter> InvalidParameters { get; private set; }

            /// <summary>
            /// Instantiate a new InvalidParameterException with a thrown exception
            /// </summary>
            /// <param name="e">The exception that caused this exception</param>
            public InvalidParameterException(Exception e) : base("This instance could not be validated.", e)
            {

            }

            /// <summary>
            /// Instantiate a new InvalidParameterException with a thrown exception
            /// </summary>
            /// <param name="e">The exception that caused this exception</param>
            /// <param name="invalidParameters">The invalid parameters that caused this exception</param>
            public InvalidParameterException(IEnumerable<TcgSdkRequestParameter> invalidParameters) : base("This instance could not be validated.")
            {
                InvalidParameters = invalidParameters;
            }
        }

    }
}
