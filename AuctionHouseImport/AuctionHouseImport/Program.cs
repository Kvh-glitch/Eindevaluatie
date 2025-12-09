using Microsoft.Data.SqlClient;
using AuctionHouseImport.DatabaseConfig;
using System.Data;
using Microsoft.VisualBasic;

namespace AuctionHouseImport
{
    internal class Program
    {
        private const string RaritiesFileName = "rarities.csv";

        private const string ItemsFileName = "items.csv";

        private delegate void ImportHandler(string[] lines, SqlConnection connection);
        
        private static List<string> errorLines = new List<string>();

        static void Main(string[] args)
        {
            if (!TestConnection())
            {
                Console.WriteLine("Cannot continue without a working database connection.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Importing AuctionHouse Dataset");
            Console.WriteLine("Importing rarities...\n");

            ImportRarities();

            Console.WriteLine("\nDone. Press any key to start importing items.");
            Console.ReadKey();

            Console.WriteLine("\nImporting items...\n");
            ImportItems();
            if (errorLines.Count > 0)
            {
                string errorLogFileName = "ImportErrorLog.txt";
                File.WriteAllLines(errorLogFileName, errorLines);
                Console.WriteLine($"\nImporting completed with {errorLines.Count} errors. See '{errorLogFileName}' for details.");
            }
            else
            {
                Console.WriteLine("\nImport completed successfully with no errors.");
            }
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();


        }
        private static bool TestConnection()
        {
            Console.WriteLine("Testing database connection...");

            using (var connection = DataBaseConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Database connection successful.");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database connection failed: {ex.Message}");
                    return false;
                }
            }
        }

        

        private static void ImportRarities()
        {

            if (!File.Exists(RaritiesFileName))
            {
                Console.WriteLine($"ERROR: File '{RaritiesFileName}' not found.");
                return;
            }
            Console.WriteLine($"Importing rarities from '{RaritiesFileName}'...");
            string[] lines = File.ReadAllLines(RaritiesFileName);
            int lineNumber = 0;
            bool isFirstLine = true;

            using (var connection = DataBaseConnection.CreateConnection())
            {
                connection.Open();
                using (var truncateCmd = new SqlCommand("TRUNCATE TABLE Rarities;", connection))
                {
                    truncateCmd.ExecuteNonQuery();
                }

                using (var insertCmd = new SqlCommand(
                    "INSERT INTO Rarities (Name, BaseCost) VALUES (@name, @basecost);",
                    connection))
                {
                    insertCmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 100));
                    insertCmd.Parameters.Add(new SqlParameter("@basecost", SqlDbType.Int));
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
                            if (headerLower.Contains("name") && (headerLower.Contains("basecost") || headerLower.Contains("base")))
                            {
                                Console.WriteLine($"[INFO] Skipping header line: '{line}'");
                                continue;
                            }
                        }
                        string[] parts = line.Split(',');
                        if (parts.Length != 2)
                        {
                            LogError("RARITIES", lineNumber, "INVALID FORMAT (expected 2 columns)", line);
                            continue;
                        }
                        string name = parts[0].Trim().Trim('"');
                        string baseCostText = parts[1].Trim().Trim('"');
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            LogError("RARITIES", lineNumber, "INVALID NAME", line);
                            continue;
                        }
                        if (!int.TryParse(baseCostText, out int baseCost) || baseCost <= 0)
                        {
                            LogError("RARITIES", lineNumber, "INVALID BASE COST", line);
                            continue;
                        }
                        insertCmd.Parameters["@name"].Value = name;
                        insertCmd.Parameters["@basecost"].Value = baseCost;
                        try
                        {
                            insertCmd.ExecuteNonQuery();
                            Console.WriteLine($"[OK] Line {lineNumber}: Name = '{name}', BaseCost = {baseCost}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[DB ERROR] Could not insert rarity '{name}': {ex.Message}");
                        }
                    }
                }

            }

        }
        private static void ImportItems()
        {

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
                using (var reader = readerCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        rarityCache[name] = id;
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
                            LogError("ITEMS", lineNumber, "INVALID FORMAT (expected at least 2 columns)", line);
                            continue;
                        }
                        string rawName = parts[0];
                        string rawRarity = parts[1];
                        string cleanName = rawName.Replace("#", "").Trim();
                        if (string.IsNullOrWhiteSpace(cleanName))
                        {
                            LogError("ITEMS", lineNumber, "MISSING NAME", line);
                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(rawRarity))
                        {
                            LogError("ITEMS", lineNumber, "MISSING RARITY", line);
                            continue;
                        }
                        string cleanRarity = rawRarity.Trim().Trim('"');
                        if (!rarityCache.TryGetValue(cleanRarity, out int rarityId))
                        {
                            LogError("ITEMS", lineNumber, $"UNKNOWN RARITY '{cleanRarity}'", line);
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

                }
            }

        }

        private static void LogError(string source, int lineNumber, string message, string rawLine)
        {
            string consoleLine = $"[ERROR] Line {lineNumber}: {message} | Line: '{rawLine}'";
            string fileLine = $"[{source}] Line {lineNumber}: {message}: '{rawLine}'";
            Console.WriteLine(consoleLine);
            errorLines.Add(fileLine);
        }
        
        
    } 
}