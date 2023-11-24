using System;
using System.Data;

namespace DatabaseTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string server = "DESKTOP-LTUDAGK\\SQLEXPRESS";
            string database = "Lab4";

            DatabaseLibrary.DatabaseHelper dbHelper = new DatabaseLibrary.DatabaseHelper(server, database);

            dbHelper.Add(1, "John Doe", "Hello, World!");

            var record = dbHelper.GetByID(1);
            if (record != null)
            {
                Console.WriteLine($"ID: {record["ID"]}, Name: {record["name"]}, Message: {record["message"]}");
            }
            else
            {
                Console.WriteLine("Record not found.");
            }

            var recordsByName = dbHelper.GetByName("John Doe");
            if (recordsByName.Rows.Count > 0)
            {
                Console.WriteLine("Records found by name:");
                foreach (DataRow row in recordsByName.Rows)
                {
                    Console.WriteLine($"ID: {row["ID"]}, Name: {row["name"]}, Message: {row["message"]}");
                }
            }
            else
            {
                Console.WriteLine("No records found by name.");
            }

            dbHelper.Update(1, "Updated message!");

            record = dbHelper.GetByID(1);
            if (record != null)
            {
                Console.WriteLine($"Updated message: {record["message"]}");
            }

            dbHelper.Delete(1);

            record = dbHelper.GetByID(1);
            if (record == null)
            {
                Console.WriteLine("Record deleted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to delete record.");
            }

        }
    }
}
