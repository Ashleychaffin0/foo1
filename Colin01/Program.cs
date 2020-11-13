using System;
using System.Collections.Generic;
using System.Runtime;
using System.IO;    // For using 'Path.Combine'

namespace Beatles_Dictionary {
	class Program {
		static Dictionary<string, float> band;

		// class myDict<string, float> {
		//     List<string> Keys = new List<string>();
		//     List<float> Values = new List<float>();

//---------------------------------------------------------------------------------------

		static void Main(string[] args) {
			Console.WriteLine(DateTime.Now.ToString("MM/dd}"));

			Console.WriteLine($"Starting program at {DateTime.Now.ToShortTimeString()}");

			string path = "/Users/colinvanvulpen/Desktop/Tech_Talk/Coding/Larry/assignment_02a/";
			path = @"G:\lrs\$People\colin";
			string filename = "BeatlesInc.txt";
			string fn = Path.Combine(path, filename);

			band = new Dictionary<string, float>();

			Execute(fn);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Reads a file and populates the "band" field with the file contents
		/// </summary>
		/// <param name="filename">The name of the file</param>
		static void Execute(string filename) {
			var rdr = new StreamReader(filename);
			string line;
			while ((line = rdr.ReadLine()) != null) {
				line = line.Trim();
				Console.WriteLine($"> {line}");
				string[] elements = line.Split(' ');
				string name = elements[0];
				// Error check that elements[1] == '='
				// Must support a = b / 5
				if (elements.Length == 1) {
					bool bOK = band.TryGetValue(name, out float value);
					if (!bOK) {
						Console.WriteLine($"{name} is undefined.");
					} else {
						Console.WriteLine($"The value of {name} is {value}.");
					}
				} else {
					string value = elements[2];
					bool kOK = float.TryParse(elements[2], out float fvalue);
					if (!kOK) {
						fvalue = band[value];
					} 
					band[name] = fvalue;
				}
			}
		}
	}
}

/*
    static float? ToNum(string field) {   // the "?" allows flexibility to return "null".
                                                  // fn is named <ToNum.string>
    bool bOK = float.TryParse(field, out float num);
    if (bOK) {
        return num;
    }
    // logic: So 'field' is not a float; it might be a string in the dictionary...
    bOK = band.TryGetValue(field, out num);
    if (bOK) {      // if a value was there for the key 'field'...
        return num; // then return the value
    }
    return null;    // otherwise return null
}

*/
