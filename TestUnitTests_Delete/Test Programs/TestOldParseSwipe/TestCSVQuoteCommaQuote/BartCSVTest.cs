using Bartizan.Input.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestCSVQuoteCommaQuote
{
    
    
    /// <summary>
    ///This is a test class for BartCSVTest and is intended
    ///to contain all BartCSVTest Unit Tests
    ///</summary>
	[TestClass()]
	public class BartCSVTest {


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for Parse
		///</summary>
		[TestMethod()]
		public void ParseTest1() {
			string text = "Hello"; // TODO: Initialize to an appropriate value
			List<string> Values = null; // TODO: Initialize to an appropriate value
			List<string> ValuesExpected = new List<string>() {"Hello"}; // TODO: Initialize to an appropriate value
			BartCSV.Parse(text, out Values);
			Assert.AreEqual(ValuesExpected, Values);
			Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}

		/// <summary>
		///A test for Parse
		///</summary>
		[TestMethod()]
		public void ParseTest() {
			string text = "Hello"; // TODO: Initialize to an appropriate value
			List<string> Values = null; // TODO: Initialize to an appropriate value
			List<string> ValuesExpected = new List<string>() { "Hello" }; // TODO: Initialize to an appropriate value
			bool SupportQuoteCommaQuote = false; // TODO: Initialize to an appropriate value
			BartCSV.Parse(text, out Values, SupportQuoteCommaQuote);
			Assert.AreEqual(ValuesExpected, Values);
			Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}
	}
}
