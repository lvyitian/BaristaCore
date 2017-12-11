﻿namespace BaristaLabs.BaristaCore
{
    using BaristaLabs.BaristaCore.JavaScript;
    using System.Collections;
    using System.Collections.Generic;

    public class JsArray : JsObject, IEnumerable<JsValue>
    {
        public JsArray(IJavaScriptEngine engine, BaristaContext context, IBaristaValueFactory valueFactory, JavaScriptValueSafeHandle value)
            : base(engine, context, valueFactory, value)
        {
        }

        public int Length
        {
            get
            {
                var result = GetPropertyByName<JsNumber>("length");
                return result.ToInt32();
            }
        }

        public override JavaScriptValueType Type
        {
            get { return JavaScriptValueType.Array; }
        }

        public IEnumerator<JsValue> GetEnumerator()
        {
            var len = Length;
            for (int i = 0; i < len; i++)
            {
                yield return this[i];
            }
        }

        public JsValue Pop()
        {
            var fn = GetPropertyByName<JsFunction>("pop");
            return fn.Invoke(new JsValue[] { this });
        }

        public void Push(JsValue value)
        {
            var fn = GetPropertyByName<JsFunction>("push");
            fn.Invoke(new JsValue[] { this, value });
        }

        public int IndexOf(JsValue valueToFind, int? startIndex = null)
        {
            var args = new List<JsValue>
            {
                this,
                valueToFind
            };

            if (startIndex.HasValue == true)
            {
                args.Add(ValueFactory.CreateNumber(startIndex.Value));
            }

            var fn = GetPropertyByName<JsFunction>("indexOf");
            var result = fn.Invoke<JsNumber>(args.ToArray());
            return result.ToInt32();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
