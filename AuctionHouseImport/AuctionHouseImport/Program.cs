using Microsoft.Data.SqlClient;
using AuctionHouseImport.DatabaseConfig;
using System.Data;

namespace AuctionHouseImport
{
    internal class Program
    {
        private const string RaritiesFileName = "rarities.csv";

        private const string ItemsFileName = "items.csv";

        static void Main(string[] args)
        {
            Console.WriteLine("+++ AuctionHouse Dataset Import +++");
            Console.WriteLine("Importing rarities...\n");

            ImportRarities();

            Console.WriteLine("\nDone. Press any key to start importing items.");
            Console.ReadKey();
            
            Console.WriteLine("\nImporting items...\n");
            ImportItems();
            Console.WriteLine("\nDone. Press any key to exit.");
            Console.ReadKey();



            //Console.WriteLine("Testing connection...");

            //TestConnection();

        }


        //private static void TestConnection()
        //{
        //    Console.WriteLine("Testing SQL connection...");

        //    try
        //    {
        //        using (var connection = new SqlConnection(DataBaseConnection.ConnectionString))
        //        {
        //            connection.Open();
        //            Console.WriteLine("SUCCESS: Connected to database!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("ERROR: Could not connect.");
        //        Console.WriteLine(ex.Message);
        //    }
        //}


        private static void ImportRarities()
        {
            var errorLines = new List<string>();
            if (!File.Exists(RaritiesFileName))
            {
                Console.WriteLine($"ERROR: File '{RaritiesFileName}' not found.");
                return;
            }
            Console.WriteLine($"Importing rarities from '{RaritiesFileName}'...");

            using (var connection = DataBaseConnection.CreateConnection())
            {
                connection.Open();
                using (var cmd = new SqlCommand("TRUNCATE TABLE Rarities;", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            string[] lines = File.ReadAllLines(RaritiesFileName);
            int lineNumber = 0;
            bool isFirstLine = true;

            foreach (string rawLine in lines)
            {
                lineNumber++;
                string line = rawLine.Trim();


                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (isFirstLine)
                {
                    isFirstLine = false;
                   
                    var headerLower = line.ToLowerInvariant();
                    if (headerLower.Contains("name") && (headerLower.Contains("cost") || headerLower.Contains("base")))
                    {
                        Console.WriteLine($"[INFO] Skipping header line: '{line}'");
                        continue;
                    }
                }

                
                string[] parts = line.Split(',');
                if (parts.Length != 2)
                {
                    Console.WriteLine($"[Line {lineNumber}] INVALID FORMAT (expected 2 columns): '{line}'");
                    errorLines.Add($"Invalid Format Line {lineNumber}: {line}");
                    continue;
                }

                string name = parts[0].Trim().Trim('"');
                string baseCostText = parts[1].Trim().Trim('"');

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine($"[Line {lineNumber}] INVALID NAME: '{line}' , skipping");
                    errorLines.Add($"Invalid Name Line {lineNumber}: {line}");
                    continue;
                }

                if (!int.TryParse(baseCostText, out int baseCost) || baseCost <= 0)
                {
                    Console.WriteLine($"[Line {lineNumber}] INVALID BASE COST: '{line}' , skipping");
                    errorLines.Add($"Invalid Base Cost Line {lineNumber}: {line}");
                    continue;
                }

                
                Console.WriteLine($"[OK] Line {lineNumber}: Name = '{name}', BaseCost = {baseCost}");

                try
                {
                    using (var connection = DataBaseConnection.CreateConnection())
                    {
                        connection.Open();

                        using (var cmd = new SqlCommand(
                            "INSERT INTO Rarities (Name, BaseCost) VALUES (@name, @cost);",
                            connection))
                        {
                            cmd.Parameters.Add(new SqlParameter("@name", name));
                            cmd.Parameters.Add(new SqlParameter("@cost", baseCost));

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DB ERROR] Could not insert rarity '{name}': {ex.Message}");
                }

            }
            if (errorLines.Count > 0)
            {
                File.WriteAllLines("rarities_import_errors.txt", errorLines);
                Console.WriteLine($"\nSaved {errorLines.Count} error lines to 'rarity_import_errors.txt'");
            }
            else
            {
                Console.WriteLine("No errors detected during item import.");
            }

        }
        private static void ImportItems()
        {
            var errorLines = new List<string>();
            if (!File.Exists(ItemsFileName))
            {
                Console.WriteLine($"ERROR: File '{ItemsFileName}' not found.");
                return;
            }
            Console.WriteLine($"Importing items from '{ItemsFileName}'...");
            string[] lines = File.ReadAllLines(ItemsFileName);
            int lineNumber = 0;

            using (var connection = DataBaseConnection.CreateConnection())
            {
                connection.Open();
                using (var truncateCmd = new SqlCommand("TRUNCATE TABLE Items;", connection))
                {
                    truncateCmd.ExecuteNonQuery();
                }


                var rarityCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                using (var readerCmd = new SqlCommand("SELECT Id, Name FROM Rarities;", connection))
                {
                    using (var reader = readerCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            rarityCache[name] = id;
                        }
                    }
                }
                using (var insertCmd = new SqlCommand(
                    "INSERT INTO Items (Name, RarityId) VALUES (@name, @rarityId);",
                    connection))
                {
                    var nameParam = new SqlParameter("@name", SqlDbType.NVarChar, 200);
                    var rarityIdParam = new SqlParameter("@rarityId", SqlDbType.Int);
                    insertCmd.Parameters.Add(nameParam);
                    insertCmd.Parameters.Add(rarityIdParam);
                    foreach (string rawLine in lines)
                    {
                        lineNumber++;
                        string line = rawLine.Trim();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;
                        line = line.Replace(";", ",");
                        string[] rawParts = line.Split(',');
                        var parts = rawParts.Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                        if (parts.Count < 2)
                        {
                            Console.WriteLine($"[Line {lineNumber}] INVALID FORMAT (expected name + rarity): '{line}', skipping");
                            errorLines.Add($"Invalid Format Line {lineNumber}: {line}");
                            continue;
                        }
                        string rawName = parts[0];
                        string rawRarity = parts[1];
                        string cleanName = rawName.Replace("#", "").Trim();
                        if (string.IsNullOrWhiteSpace(cleanName))
                        {
                            Console.WriteLine($"[Line {lineNumber}] MISSING NAME: '{line}' , skipping");
                            errorLines.Add($"Missing Name Line {lineNumber}: {line}");
                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(rawRarity))
                        {
                            Console.WriteLine($"[Line {lineNumber}] MISSING RARITY: '{line}' , skipping");
                            errorLines.Add($"Missing Rarity Line {lineNumber}: {line}");
                            continue;
                        }
                        string cleanRarity = rawRarity.Trim().Trim('"');
                        if (!rarityCache.TryGetValue(cleanRarity, out int rarityId))
                        {
                            Console.WriteLine($"[Line {lineNumber}] UNKNOWN RARITY '{cleanRarity}': '{line}' , skipping");
                            errorLines.Add($"Unknown Rarity Line {lineNumber}: {line}");
                            continue;
                        }
                        nameParam.Value = cleanName;
                        rarityIdParam.Value = rarityId;
                        try
                        {
                            insertCmd.ExecuteNonQuery();
                            Console.WriteLine($"[OK] Line {lineNumber}: Name = '{cleanName}', Rarity = '{cleanRarity}'");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[DB ERROR] Could not insert item '{cleanName}': {ex.Message}");
                        }
                    }
                    if (errorLines.Count > 0)
                    {
                        File.WriteAllLines("item_import_errors.txt", errorLines);
                        Console.WriteLine($"\nSaved {errorLines.Count} error lines to 'item_import_errors.txt'");
                    }
                    else
                    {
                        Console.WriteLine("No errors detected during item import.");
                    }
                }



            }
            
            //foreach (string rawLine in lines)
            //{
            //    lineNumber++;
            //    string line = rawLine.Trim();

            //    if (string.IsNullOrWhiteSpace(line))
            //        continue;
            //    line = line.Replace(";", ",");
            //    string[] rawParts = line.Split(',');
            //    var parts = rawParts.Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            //    if (parts.Count < 2)
            //    {
            //        Console.WriteLine($"[Line {lineNumber}] INVALID FORMAT (expected name + rarity): '{line}'");
            //        continue;
            //    }
            //    string rawName = parts[0];
            //    string rawRarity = parts[1];
            //    string cleanName = rawName.Replace("#", "").Trim();

            //    if (string.IsNullOrWhiteSpace(cleanName))
            //    {
            //        Console.WriteLine($"[Line {lineNumber}] MISSING NAME: '{line}' , skipping");
            //        continue;
            //    }
            //    if (string.IsNullOrWhiteSpace(rawRarity))
            //    {
            //        Console.WriteLine($"[Line {lineNumber}] MISSING RARITY: '{line}' , skipping");
            //        continue;
            //    }

            //    string cleanRarity = rawRarity.Trim();
            //    Console.WriteLine($"[OK] Line {lineNumber}: Name = '{cleanName}', Rarity = '{cleanRarity}'");

            //}
            
        }
    }
}