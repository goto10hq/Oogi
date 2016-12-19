using System;
using System.Collections.Generic;
using Sushi;

namespace Oogi
{
    public static class Converter
    {        
        private static readonly Dictionary<Type, Func<object, string>> _processors = new Dictionary<Type, Func<object, string>>
                                                                     {
                                                                         { typeof(string), StringProcessor },
                                                                         { typeof(char), StringProcessor },
                                                                         { typeof(bool), BooleanProcessor },
                                                                         { typeof(bool?), BooleanProcessor }
                                                                     };

        public static string Process(object val)
        {
            if (val == null)
                return "null";

            var t = val.GetType();

            return _processors.ContainsKey(t) ? _processors[t].Invoke(val) : UniversalProcessor(val);
        }

        private static string UniversalProcessor(object val)
        {
            var formattable = val as IFormattable;

            return formattable?.ToString(null, Cultures.English) ?? val.ToString();
        }
        
        private static string StringProcessor(object val)
        {            
            return "'" + val.ToString().ToEscapedString() + "'";
        }

        private static string BooleanProcessor(object val)
        {
            return val.ToString().ToLower();
        }
    }
}
