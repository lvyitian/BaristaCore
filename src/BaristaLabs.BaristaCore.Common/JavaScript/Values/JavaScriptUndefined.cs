﻿namespace BaristaLabs.BaristaCore.JavaScript
{
    using System.Diagnostics;

    /// <summary>
    /// Represents a JavaScript 'undefined'
    /// </summary>
    public sealed class JavaScriptUndefined : JavaScriptValue
    {
        internal JavaScriptUndefined(IJavaScriptEngine engine, JavaScriptContext context, JavaScriptValueSafeHandle handle)
            : base(engine, context, handle)
        {
        }
    }
}
