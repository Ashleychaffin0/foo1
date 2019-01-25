// Copyright (c) 2007 Bartizan Connects LLP

// TODO: Add nested class for custom exceptions (e.g. calling a method when the
//		 engine hasn't been initialized (i.e. is still null)

using System;
using System.Collections.Generic;
using System.IO;

using IronPython.Hosting;
using IronPython.Compiler;

#if false		// Article on capturing stdout from within IronPython
This article came about from a need to capture standard output from code being executed simultaneously in different places.

In the process we discovered that you could create, configure and call the heart of IronPython, the PythonEngine, from inside IronPython!

This is interesting for executing code in an isolated context [1]. You can also configure the PythonEngine in interesting ways, like setting an arbitrary stream as the standard output for the engine.  

If you are interested in embedding IronPython in a .NET application, then prototyping and experimenting from the interactive interpreter makes this a much more fun experience.

Simple Embedding
The very basic case, performs the following steps:

Instantiate PythonEngine 
Create a new module, with a dictionary of values (the global scope) 
Set this as the default module 
Execute a script 
Things to note include:

No need to add a reference to the IronPython.Hosting assembly. Unsurprisingly, IronPython already has one.  
The boolean value means that our new module is published to sys.modules with the name we give it 
The dictionary we pass in is just a standard 'python' dictionary. From the C# side this would have to be some object implementing IDictionary. 
The result is that 'Hello World' is printed to the console.

from IronPython.Hosting import PythonEngine

py = PythonEngine()

values = {'string': 'Hello World'}

mod = py.CreateModule('__main__', values, True)
py.DefaultModule = mod

script = "print string"

py.Execute(script)
Module Globals
We could configure the values in our module in a slightly different way. Every module is an instance of EngineModule. This has a Globals property, which can be configured using the methods of IDictionary.

In this case we call CreateModule with a string and a boolean, and omit the values dictionary:

from IronPython.Hosting import PythonEngine

py = PythonEngine()


mod = py.CreateModule('__main__', True)
py.DefaultModule = mod
mod.Globals.Add('string', "Hello World")

script = "print string"

py.Execute(script)
Setting the Standard Output
This isn't really any more complicated, except we create a subclass of the abstract class Stream, and set it as the standard output for our engine :

from IronPython.Hosting import PythonEngine
from System.IO import Stream
from System.Text import Encoding

py = PythonEngine()

class Diverter(Stream):

    def __init__(self):
        self.string = ''

    def Write(self, bytes, offset, count):
        // Turn the byte-array back into a string -- Change // to pound
        self.string += Encoding.UTF8.GetString(bytes, offset, count)

    @property
    def CanRead(self):
        return False

    @property
    def CanSeek(self):
        return False

    @property
    def CanWrite(self):
        return True

    def Flush(self):
        pass

    def Close(self):
        pass

    @property
    def Position(self):
        return 0

output = Diverter()
py.SetStandardOutput(output)


values = {'string': 'Hello World'}

mod = py.CreateModule('__main__', values, True)
py.DefaultModule = mod

script = "print string"


py.Execute(script)

print 'Captured standard output: %s', output.string
(Thanks to Dino Viehland for setting me straight on this implementation.)

The great thing is, that on multi-core machines with separate engines running on separate threads you should get much better performance from IronPython than CPython. Embedding multiple interpreter is also something that CPython doesn't excel at. However, there are indications that in the long run this feature may not remain in IronPython, so perhaps it is better to not rely on it.

[1] To make it a real sandbox it would need to be a separate app-domain of course. 


#endif

namespace com.Bartizan.Scripting {
	public class IronPythonScripting {

		PythonEngine _ScriptEngine;
		EngineModule _mod;
		CompiledCode _code;

//---------------------------------------------------------------------------------------

		public PythonEngine ScriptEngine {
			get { return _ScriptEngine; }
		}

//---------------------------------------------------------------------------------------

		public EngineModule Mod {
			get { return _mod; }
		}

//---------------------------------------------------------------------------------------

		public CompiledCode Code {
			get { return _code; }
		}

//---------------------------------------------------------------------------------------

		public IronPythonScripting() {
			_ScriptEngine = null;
			_mod		  = null;
			_code		  = null;
		}

//---------------------------------------------------------------------------------------

		public void Init() {
			CommonInit(null);
		}

//---------------------------------------------------------------------------------------

		public void Init(EngineOptions Opts) {
		}

//---------------------------------------------------------------------------------------

		private void CommonInit(EngineOptions Opts) {
			if (Opts == null) {
				_ScriptEngine = new PythonEngine();
			} else {
				_ScriptEngine = new PythonEngine(Opts);
			}
			_mod = ScriptEngine.CreateModule("__main__", true);
		}

//---------------------------------------------------------------------------------------

		public void CompileFile(string Filename) {
			_code = ScriptEngine.CompileFile(Filename);
		}

//---------------------------------------------------------------------------------------

		public void Compile(string PyProg) {
			_code = _ScriptEngine.Compile(PyProg);
		}

//---------------------------------------------------------------------------------------

		public void Execute() {
			Code.Execute(Mod);
		}

//---------------------------------------------------------------------------------------

		public void Execute(Dictionary<string, object> Locals) {
			Code.Execute(Mod, Locals);
		}

//---------------------------------------------------------------------------------------

		public void Execute(string PyProg) {
			Execute(PyProg, null);
		}

//---------------------------------------------------------------------------------------

		public void Execute(string PyProg, Dictionary<string, object> Locals) {
			CompiledCode	code = ScriptEngine.Compile(PyProg);
			if (Locals == null) {
				code.Execute(Mod);
			} else {
				code.Execute(Mod, Locals);
			}
		}

//---------------------------------------------------------------------------------------

		public void ExecuteFile(string Filename) {
			ExecuteFile(Filename, null);
		}

//---------------------------------------------------------------------------------------

		public void ExecuteFile(string Filename, Dictionary<string, object> Locals) {
			StreamReader	rdr = File.OpenText(Filename);
			string	script = rdr.ReadToEnd();
			rdr.Close();
			if (Locals == null) {
				Execute(script);
			} else {
				Execute(script, Locals);
			}
		}

//---------------------------------------------------------------------------------------

		public object Evaluate(string PySnippet) {
			return ScriptEngine.Evaluate(PySnippet, Mod);
		}

//---------------------------------------------------------------------------------------

		public object Evaluate(string PySnippet, Dictionary<string, object> Locals) {
			return ScriptEngine.Evaluate(PySnippet, Mod, Locals);
		}
	}
}
