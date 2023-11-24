using NUnit.Framework;
using DatabaseLibrary;
using System.Data.SqlClient;
using System.Data;

namespace DatabaseTestProject
{
    public class Tests
    {
        [TestFixture]
        public class DatabaseTests
        {
            private string server = "DESKTOP-LTUDAGK\\SQLEXPRESS";
            private string database = "Lab4";
            private DatabaseLibrary.DatabaseHelper dbHelper;

            [SetUp]
            public void Setup()
            {
                dbHelper = new DatabaseLibrary.DatabaseHelper(server, database);

                dbHelper.DeleteAll();
            }

            [Test]
            public void AddRecord_Test()
            {
                dbHelper.Add(1, "John Doe", "Test message");

                var record = dbHelper.GetByID(1);
                Assert.IsNotNull(record, "Record not added to the database.");
            }

            [Test]
            public void AddRecord_DuplicateID_Test()
            {
                dbHelper.Add(1, "John Doe", "Original message");

                try
                {
                    dbHelper.Add(1, "Another John Doe", "Duplicate ID");
                    Assert.Fail("Expected SqlException was not thrown.");
                }
                catch (SqlException ex)
                {
                    StringAssert.Contains("PRIMARY KEY", ex.Message);
                }

                var duplicateRecord = dbHelper.GetByID(1);
                Assert.IsNotNull(duplicateRecord, "Original record should still exist.");
                Assert.AreEqual("Original message", duplicateRecord["message"], "Original record message should not be changed.");
            }

            [Test]
            public void UpdateRecord_Test()
            {
                dbHelper.Add(1, "John Doe", "Original message");

                dbHelper.Update(1, "Updated message");

                var updatedRecord = dbHelper.GetByID(1);
                Assert.AreEqual("Updated message", updatedRecord["message"], "Record not updated in the database.");
            }

            [Test]
            public void UpdateRecord_NonexistentID_Test()
            {
                dbHelper.Update(999, "Update non-existent record");
                var nonExistentRecord = dbHelper.GetByID(999);
                Assert.IsNull(nonExistentRecord, "Non-existent record should not be updated.");
            }

            [Test]
            public void GetByID_Test()
            {
                dbHelper.Add(1, "John Doe", "Test message");
                var record = dbHelper.GetByID(1);
                Assert.IsNotNull(record, "Record not retrieved by ID.");
                Assert.AreEqual("John Doe", record["name"]);
                Assert.AreEqual("Test message", record["message"]);
            }

            [Test]
            public void GetByName_Test()
            {
                dbHelper.Add(1, "John Doe", "Test message");
                dbHelper.Add(2, "John Doe", "Another message");

                var dataTable = dbHelper.GetByName("John Doe") as DataTable;

                Assert.IsNotNull(dataTable, "Records not retrieved by name.");

                Assert.AreEqual(2, dataTable.Rows.Count, "Wrong number of records retrieved by name.");
            }

            [Test]
            public void Update_Test()
            {
                dbHelper.Add(1, "John Doe", "Original message");
                dbHelper.Update(1, "Updated message");
                var updatedRecord = dbHelper.GetByID(1);
                Assert.IsNotNull(updatedRecord, "Record not updated by ID.");
                Assert.AreEqual("Updated message", updatedRecord["message"]);
            }

            [Test]
            public void Delete_Test()
            {
                dbHelper.Add(1, "John Doe", "Test message");
                dbHelper.Delete(1);
                var deletedRecord = dbHelper.GetByID(1);
                Assert.IsNull(deletedRecord, "Record not deleted by ID.");
            }

            [Test]
            public void Delete_NonexistentID_Test()
            {
                dbHelper.Add(1, "John Doe", "Test message");
                dbHelper.Delete(2);
                var remainingRecord = dbHelper.GetByID(1);
                Assert.IsNotNull(remainingRecord, "Record should not be deleted for nonexistent ID.");
            }

            [Test]
            public void GetByID_NonexistentID_Test()
            {
                dbHelper.Add(1, "John Doe", "Test message");
                var nonExistentRecord = dbHelper.GetByID(2);
                Assert.IsNull(nonExistentRecord, "Record should not be retrieved for nonexistent ID.");
            }

            [Test]
            public void GetByName_NonexistentName_Test()
            {
                dbHelper.Add(1, "John Doe", "Test message");
                var nonExistentRecords = dbHelper.GetByName("Jane Doe");

                Assert.AreEqual(0, nonExistentRecords.Rows.Count, "No records should be retrieved for nonexistent name.");
            }
        }
    }
}