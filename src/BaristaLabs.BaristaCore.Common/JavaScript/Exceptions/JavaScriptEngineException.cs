﻿namespace BaristaLabs.BaristaCore.JavaScript
{
    using System;

    /// <summary>
    ///     An exception that occurred in the workings of the JavaScript engine itself.
    /// </summary>
    public sealed class JavaScriptEngineException : JavaScriptException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="JavaScriptEngineException"/> class. 
        /// </summary>
        /// <param name="code">The error code returned.</param>
        public JavaScriptEngineException(JavaScriptErrorCode code) :
            this(code, "A fatal exception has occurred in a JavaScript runtime")
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JavaScriptEngineException"/> class. 
        /// </summary>
        /// <param name="code">The error code returned.</param>
        /// <param name="message">The error message.</param>
        public JavaScriptEngineException(JavaScriptErrorCode code, string message) :
            base(code, message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JavaScriptEngineException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        private JavaScriptEngineException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
