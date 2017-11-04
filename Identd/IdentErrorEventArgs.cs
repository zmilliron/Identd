/****************************************************************************************
 * Copyright (c) Zachary Milliron
 *
 * This source is subject to the Microsoft Public License.
 * See https://opensource.org/licenses/MS-PL.
 * All other rights worth reserving are reserved.
 ****************************************************************************************/
using System;

namespace Identd
{
    /// <summary>
    /// Provides data for an event raised when an error occurs while negotiating the
    /// Identd protocol.
    /// </summary>
    public class IdentErrorEventArgs : EventArgs
    {
        private string _message;

        /// <summary>
        /// Gets the message describing the error.
        /// </summary>
        public string Message
        {
            get
            {
                return (_message ?? Exception?.Message);
            }
            private set
            {
                _message = value;
            }
        }

        /// <summary>
        /// The exception that raised the error event, or null if no exception or occurred.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ident.IdentErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public IdentErrorEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ident.IdentErrorEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception that raised the error event, or null if no exception or occurred.</param>
        public IdentErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ident.IdentErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="exception">The exception that raised the error event, or null if no exception or occurred.</param>
        public IdentErrorEventArgs(string message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }
    }
}
