using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MicrosoftZuneLibrary;
using LRS.Zune.Classes;

namespace TestLRSZuneClasses_1 {


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class LRSTest {
		
//---------------------------------------------------------------------------------------

		static Type[] ZuneDbTypes = new Type[] {
                // bool
                typeof(bool),
                // Fairly standard extra types
                typeof(string), typeof(DateTime), 
                // Integer types first (sbyte intentionally missing)
                typeof(byte), typeof(short), typeof(int), typeof(long),
                // According to the above comments, SSCE doesn't seem to support unsigned
                // types. But we'll put them in, just in case a later release has them.
                typeof(ushort), typeof(ulong), 
                // Floating point
                typeof(float), typeof(double),
                // Uh, decimal
                typeof(decimal), 
                // Lists
                typeof(List<string>), typeof(List<int>),
                typeof(List<byte>),      // varbinary. Also Unicode?
                // Presumably this next is for album covers
                typeof(System.Drawing.Image),
                typeof(Guid),                   // Usually overridden by List<string> above
                typeof(byte[,])                 // Dunno if this can occur
		};

/*
let GetFieldValExists (zQList: ZuneQueryList) (ix: uint32) tipe (usmt: uint32) = 
    try
        // Very infrequently a request to GetFieldValue blows up (usually with an
        // Access (i.e. bad pointer) exception). My guess is that when we create the
        // ZuneQueryList we always pass in a null QueryPropertyBag, and that some
        // requests need that field defined (and, bug in Zune software, doesn't check
        // that it's defined before trying to use it).
        let x = zQList.GetFieldValue(ix, tipe, usmt)
        x <> null
    with 
        | _ -> false

*/

//---------------------------------------------------------------------------------------

		public static Dictionary<string, object> GetFieldsFromZuneQueryList(ZuneQueryList zql) {
			var res = new Dictionary<string, object>();
			if (zql == null) {
				return res;
			}
			for (int i = 0; i < zql.Count; i++) {
				
			}
			// let typelist = types |> List.filter (fun t -> GetFieldValExists zQList 0u t usmt)

			return res;
		}
	}
}
