using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolTableSample {
    class SymbolTable {

        Dictionary<string, DataTypeValue> DataTypes; // TODO: public?
        HashSet<string> KeyWords;    // TODO: public?

        string[] KeyWords_Alternate;

//---------------------------------------------------------------------------------------

        public enum DataTypeValue {        // TODO: public?
            Unknown = 0,
            @bool,
            @byte,
            @sbyte,
            @short,
            @ushort,
            @char,
            @decimal,
            @double,
            @float,
            @int,
            @uint,
            @long,
            @ulong,
            @string,
            @object,

            UserTypeBase = 100
        }

//---------------------------------------------------------------------------------------

        // Constructor
        public SymbolTable() {
            SetKeywords();
            SetDataTypes();
        }

//---------------------------------------------------------------------------------------

        private void SetKeywords_Alternate() {
            // Note: This contains data types as well (e.g. int, float, etc). We
            //       could (read: should) remove them since they're in the
            //       DataTypes dictionary. But I sort of liked having all the C#
            //       keywords in one place. But this does mean we have to check
            //       the first token in a line against DataTypes first and
            //       KeyWords second.
            string kws = "abstract/as/base/bool/break/byte/case/catch/" +
                "char/checked/class/const/continue/decimal/default/delegate/" +
                "do/double/else/enum/event/explicit/extern/false/finally/fixed/" +
                "float/for/foreach/goto/if/implicit/in/int/interface/internal/is/" +
                "lock/long/namespace/new/null/object/operator/out/override/params/" +
                "private/protected/public/readonly/ref/regurn/sbyte/sealed/short/sizeof/" +
                "stackalloc/static/string/struct/switch/this/throw/true/try/typeof/" +
                "uint/ulong/unchecked/unsafe/ushort/using/virtual/void/volatile/while";
            string[] kwArray = SetKeywordHashTable(kws);
            KeyWords_Alternate = kwArray;
        }

//---------------------------------------------------------------------------------------

        private void SetKeywords() {
            string kws = "class/endclass/proc/endproc";
            SetKeywordHashTable(kws);
        }

//---------------------------------------------------------------------------------------

        private string[] SetKeywordHashTable(string kws) {
            var kwArray = kws.Split('/');
            KeyWords = new HashSet<string>();
            foreach (var keyword in kwArray) {
                KeyWords.Add(keyword);
            }
            return kwArray;
        }

//---------------------------------------------------------------------------------------

        public bool IsKeyword_Alternate(string Token) {  // Alternate implementation
            foreach (var kw in KeyWords_Alternate) {
                if (Token == kw) {
                    return true;
                }
            }
            return false;
        }

//---------------------------------------------------------------------------------------

        public bool IsKeyword(string Token) {
            return KeyWords.Contains(Token);
        }

//---------------------------------------------------------------------------------------

        private void SetDataTypes() {
            DataTypes = new Dictionary<string, DataTypeValue> {
                ["bool"] = DataTypeValue.@bool,
                ["byte"] = DataTypeValue.@byte,
                ["sbyte"] = DataTypeValue.@sbyte,
                ["short"] = DataTypeValue.@short,
                ["ushort"] = DataTypeValue.@ushort,
                ["char"] = DataTypeValue.@char,
                ["decimal"] = DataTypeValue.@decimal,
                ["double"] = DataTypeValue.@double,
                ["float"] = DataTypeValue.@float,
                ["int"] = DataTypeValue.@int,
                ["uint"] = DataTypeValue.@uint,
                ["long"] = DataTypeValue.@long,
                ["ulong"] = DataTypeValue.@ulong,
                ["string"] = DataTypeValue.@string,
                ["object"] = DataTypeValue.@object
            };
        }

//---------------------------------------------------------------------------------------

        public DataTypeValue GetDataType(string Token) {
            DataTypeValue value;
            DataTypes.TryGetValue(Token, out value);
            return value;
        }
    }
}
