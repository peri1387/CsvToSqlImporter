using System;                        // Για την κονσόλα
using System.Collections.Generic;   // Για τις λίστες
using System.Globalization;        // Για την κουλτούρα (CSV)
using System.IO;                  // Για την ανάγνωση αρχείων
using System.Linq;               // Για τις λειτουργίες με λίστες
using Microsoft.Data.SqlClient; // Το εργαλείο για την SQL
using CsvHelper;               // Το εργαλείο για το CSV

namespace CsvToSqlImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1. Ορίζουμε πού είναι το αρχείο και πού η βάση
            string csvPath = @"C:\Users\peria\Documents\data.csv";
            string connectionString = @"Server=.\SQLEXPRESS;Database=AnalyticsDB;Trusted_Connection=True;TrustServerCertificate=True;";

            Console.WriteLine("Start data import process...");

            try
            {
                // 2. Διαβάζουμε το CSV αρχείο
                using (var reader = new StreamReader(csvPath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    // Μετατρέπουμε τις γραμμές του CSV σε μια λίστα από αντικείμενα "Product"
                    var records = csv.GetRecords<Product>().ToList();

                    // 3. Σύνδεση με την SQL και μαζική εισαγωγή (Bulk Copy)
                    // 3. Σύνδεση με την SQL
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // --- Καθαρισμός του πίνακα πριν το import ---
                        Console.WriteLine("Καθαρισμός παλιών δεδομένων από τη βάση...");
                        string truncateQuery = "TRUNCATE TABLE Products;";
                        using (SqlCommand command = new SqlCommand(truncateQuery, connection))
                        {
                            command.ExecuteNonQuery(); // Εκτελεί το άδειασμα στην SQL
                        }
                        // --------------------------------------------------------

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = "Products"; // Ο πίνακας στην SQL

                            // Αντιστοίχιση στηλών CSV -> SQL
                            bulkCopy.ColumnMappings.Add("ID", "ID");
                            bulkCopy.ColumnMappings.Add("ProductName", "ProductName");
                            bulkCopy.ColumnMappings.Add("Price", "Price");

                            // Εκτέλεση της μεταφοράς
                            var dataTable = ConvertListToDataTable(records);
                            bulkCopy.WriteToServer(dataTable);
                        }
                    }
                }
                Console.WriteLine("The import was completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit....");
            Console.ReadKey();
        }

        // Βοηθητική μέθοδος για να "καταλάβει" η SQL τη λίστα μας
        static System.Data.DataTable ConvertListToDataTable(List<Product> products)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));

            foreach (var p in products)
            {
                dt.Rows.Add(p.ID, p.ProductName, p.Price);
            }
            return dt;
        }
    }

    // Η "μακέτα" των δεδομένων μας
    public class Product
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}