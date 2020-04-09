using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://sqlite.org/index.html

namespace TestConsoleWithSqlite_Nuget {
	class Program {

		static void Main(string[] args) {
			string dbName = @"G:\LRS\SqlLite\MyDatabase.sqlite";
			// LrsSQLite.CreateDatabase(dbName);
			var test = new LrsSQLite(dbName);

			RunSqliteTest(test);

			Console.WriteLine("Press any key to exit");
			Console.ReadKey();
		}

//---------------------------------------------------------------------------------------

		public static void RunSqliteTest(LrsSQLite db) {
			db.Connect();
			string g = Guid.NewGuid().ToString().Replace('-', '_');

			string sql = $"create table Test___{g} ("
				+ "ID integer primary key autoincrement, "
				+ "name varchar(20), "
				+ "score int)";
			db.ExecuteNonQuery(sql);

			bool bTable = db.IsTableExists("Test1");

			sql = "insert into Test1(name, score) values('Tony', 100)";
			db.ExecuteNonQuery(sql);
			sql = "insert into Test1(name, score) values('Laura', 1000)";
			db.ExecuteNonQuery(sql);
			sql = "insert into Test1(name, score) values('Larry', 10)";
			db.ExecuteNonQuery(sql);

			sql = "select * from Test1 "
				+ "order by score desc";
			var rdr = db.ExecuteQuery(sql);
			while (rdr.Read()) {
				int ID = Convert.ToInt32(rdr["ID"]);
				string name = (string)rdr["name"];
				int score = (int)rdr["score"];
				Console.WriteLine($"{ID}: {name,-20}, {score}");
			}
		}
	}
}
