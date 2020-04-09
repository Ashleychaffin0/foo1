using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUnitTests;

namespace TestMyFirstTests {
	[TestClass]
	public class UnitTest1 {
		[TestMethod]
		public void TestMethod1() {
			var p = new Point(3, 4);
			var dist = p.Distance();
			Assert.AreEqual(dist, 5.0001, .001, "*BAD* hypotenuse!");
		}
	}
}
