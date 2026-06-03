# High-Performance C# ETL Data Importer

A lightweight, production-ready ETL (Extract, Transform, Load) utility built in C# utilizing .NET 8. This tool automates the process of importing large-scale datasets from flat CSV files into a structured Microsoft SQL Server database using high-performance streaming techniques.

## 🚀 Key Features
* **Optimized Data Ingestion:** Uses `SqlBulkCopy` instead of row-by-row inserts, reducing database overhead and processing thousands of rows in milliseconds.
* **Idempotent Pipeline:** Implements a `TRUNCate TABLE` routine prior to execution to avoid Primary Key violations and guarantee data consistency.
* **Robust CSV Parsing:** Integrated with `CsvHelper` to handle complex CSV formatting, encodings, and types safely.

## 📊 Data Architecture
The data pipeline follows a strict structural mapping from the data source to the relational database:

| Source Column (CSV) | Target Column (SQL) | Data Type | Key Type |
| :--- | :--- | :--- | :--- |
| ID | ID | INT | PRIMARY KEY |
| ProductName | ProductName | NVARCHAR(100) | - |
| Price | Price | DECIMAL(10,2) | - |

## 🛠️ Tech Stack & Tools
* **Language:** C# (.NET 10 Console Application)
* **Database:** Microsoft SQL Server Express 2022
* **Database Management:** SQL Server Management Studio (SSMS)
* **Libraries (NuGet):** `Microsoft.Data.SqlClient`, `CsvHelper`

## 💻 How To Run

1. **Database Setup:** Run the following script in SSMS to prepare the schema:
   ```sql
   CREATE DATABASE AnalyticsDB;
   USE AnalyticsDB;
   CREATE TABLE Products (
       ID INT PRIMARY KEY,
       ProductName NVARCHAR(100),
       Price DECIMAL(10, 2)
   );

2. **Configuration:** Update the csvPath and connectionString variables in Program.cs to match your local environment.

3. **Execute:** Run the application via Visual Studio. The console will display a success message once the batch upload is complete.

## 📈 Portfolio Value (Data Analytics Perspective)
In real-world data engineering scenarios, data analysts often face the challenge of extracting raw data from unoptimized legacy formats (like CSVs/Logs) and storing them in analytical warehouses. This project demonstrates:

- Deep understanding of relational database constraints.

- Ability to build scalable data infrastructure (handling large rows efficiently).

- Mastery of backend programming to bridge the gap between software development and data analysis.

## 📸 Execution Screenshots

### 1. Console Application Output
Below is the execution of the ETL pipeline processing 20,000 rows of sales data:
<img width="1483" height="343" alt="console_running" src="https://github.com/user-attachments/assets/e6c7c9f0-447c-4535-aa50-664a5b628fbc" />

### 2. SQL Server Verification
Verification inside SQL Server Management Studio (SSMS) showing the successfully populated `Products` table:
<img width="1920" height="1020" alt="ssms_results" src="https://github.com/user-attachments/assets/9660ddd1-9da7-495d-bd08-85355c423475" />

------------------------------

©️ PERIKLIS ANDRIANOS
