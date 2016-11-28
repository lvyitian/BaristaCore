﻿namespace BaristaLabs.BaristaCore.JavaScript
{
    using Internal;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Represents a handle to a JavaScript Runtime
    /// </summary>
    public sealed class JavaScriptRuntimeSafeHandle : JavaScriptReference<JavaScriptRuntimeSafeHandle>
    {
        public JavaScriptRuntimeSafeHandle()
            : base()
        {
        }

        public JavaScriptRuntimeSafeHandle(IntPtr handle)
            : base(handle)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsClosed)
            {
                //Ensure that a context is not active, otherwise the runtime will throw a "Runtime In Use" exception.
                var error = LibChakraCore.JsSetCurrentContext(JavaScriptContextSafeHandle.Invalid);
                Debug.Assert(error == JavaScriptErrorCode.NoError);

                error = LibChakraCore.JsDisposeRuntime(handle);
                Debug.Assert(error == JavaScriptErrorCode.NoError);
            }

            //base.Dispose(disposing);
        }

        /// <summary>
        /// Gets an invalid runtime.
        /// </summary>
        public static readonly JavaScriptRuntimeSafeHandle Invalid = new JavaScriptRuntimeSafeHandle();
    }
}
