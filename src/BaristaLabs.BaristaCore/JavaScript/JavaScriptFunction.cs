﻿namespace BaristaLabs.BaristaCore.JavaScript
{
    using Extensions;
    using Interop;
    using Interop.SafeHandles;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    public sealed class JavaScriptFunction : JavaScriptObject
    {
        internal JavaScriptFunction(JavaScriptValueSafeHandle handle, JavaScriptValueType type, JavaScriptContext context) :
            base(handle, type, context)
        {

        }

        public JavaScriptValue Invoke()
        {
            return Invoke(Enumerable.Empty<JavaScriptValue>());
        }

        public JavaScriptValue Invoke(IEnumerable<JavaScriptValue> args)
        {
            var argsArray = args.PrependWith(this).Select(val => val.m_handle.DangerousGetHandle()).ToArray();
            if (argsArray.Length > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(args));

            var eng = GetContext();
            JavaScriptValueSafeHandle resultHandle;
            Errors.CheckForScriptExceptionOrThrow(m_api.JsCallFunction(m_handle, argsArray, (ushort)argsArray.Length, out resultHandle), eng);
            if (resultHandle.IsInvalid)
                return eng.UndefinedValue;

            return eng.CreateValueFromHandle(resultHandle);
        }

        public JavaScriptObject Construct(IEnumerable<JavaScriptValue> args)
        {
            var argsArray = args.PrependWith(this).Select(val => val.m_handle.DangerousGetHandle()).ToArray();
            if (argsArray.Length > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(args));

            var eng = GetContext();
            JavaScriptValueSafeHandle resultHandle;
            Errors.CheckForScriptExceptionOrThrow(m_api.JsConstructObject(m_handle, argsArray, (ushort)argsArray.Length, out resultHandle), eng);
            if (resultHandle.IsInvalid)
                return eng.NullValue;

            return eng.CreateObjectFromHandle(resultHandle);
        }

        public JavaScriptFunction Bind(JavaScriptObject thisObject, IEnumerable<JavaScriptValue> args)
        {
            var eng = GetContext();

            if (thisObject == null)
                thisObject = eng.NullValue;
            if (args == null)
                args = Enumerable.Empty<JavaScriptValue>();

            var bindFn = GetBuiltinFunctionProperty("bind", "Function.prototype.bind");
            return bindFn.Invoke(args.PrependWith(thisObject)) as JavaScriptFunction;
        }

        public JavaScriptValue Apply(JavaScriptObject thisObject, JavaScriptArray args = null)
        {
            var eng = GetContext();
            if (thisObject == null)
                thisObject = eng.NullValue;

            var applyFn = GetBuiltinFunctionProperty("apply", "Function.prototype.apply");

            List<JavaScriptValue> resultList = new List<JavaScriptValue>();
            resultList.Add(thisObject);
            if (args != null)
                resultList.Add(args);

            return applyFn.Invoke(resultList);
        }

        public JavaScriptValue Call(JavaScriptObject thisObject, IEnumerable<JavaScriptValue> args)
        {
            var eng = GetContext();
            if (thisObject == null)
                thisObject = eng.NullValue;

            if (args == null)
                args = Enumerable.Empty<JavaScriptValue>();

            var argsArray = args.PrependWith(this).Select(val => val.m_handle.DangerousGetHandle()).ToArray();
            if (argsArray.Length > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(args));

            JavaScriptValueSafeHandle result;
            Errors.CheckForScriptExceptionOrThrow(m_api.JsCallFunction(m_handle, argsArray, unchecked((ushort)argsArray.Length), out result), eng);
            return eng.CreateValueFromHandle(result);
        }

        #region DynamicObject overrides
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            var e = GetContext();
            var c = e.Converter;
            result = Invoke(args.Select(a => c.FromObject(a)));

            return true;
        }

        public override bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result)
        {
            var e = GetContext();
            var c = e.Converter;
            result = Construct(args.Select(a => c.FromObject(a)));

            return true;
        }
        #endregion
    }
}
