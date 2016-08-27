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
        private string name_ = null;
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
        public bool Confirmed { get; set; }

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
                validated = true;
                Confirmed = true;
            }
            catch (InvalidParameterException e)
            {
                throw e;
            }
            catch (ParameterAlreadyValidatedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                try
                {
                    TcgSdkErrorLog.WriteLog(e, System.Diagnostics.EventLogEntryType.Error, 0);
                }
                catch
                {
                    // Error writing log to event file, stopping here to avoid endless loop of errors.
                }

                throw new Exception("There was an unknown error", e);
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
        /// Validate the object and populate the inner parameter values
        /// </summary>
        private void validate()
        {
            if (validated)
                throw new ParameterAlreadyValidatedException();

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
            if (!Confirmed)
                throw new InvalidParameterException(new TcgSdkRequestParameter[] { this });

            var sb = new StringBuilder();

            sb.Append(string.Format("{0}=", name_));

            if (multiValue_)
            {
                if (and_)
                {
                    foreach (string item in multiValueParameterValues)
                    {
                        sb.Append(string.Format("{0},", item));
                    }
                }
                else
                {
                    foreach (string item in multiValueParameterValues)
                    {
                        sb.Append(string.Format("{0}|", item));
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
