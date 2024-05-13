using MVP_Sounds_GoldKingZ.Config;
using MySqlConnector;
using System.Data;

namespace MVP_Sounds_GoldKingZ;

public class MySqlDataManager
{
    public class MySqlConnectionSettings
    {
        public string? MySqlHost { get; set; }
        public string? MySqlDatabase { get; set; }
        public string? MySqlUsername { get; set; }
        public string? MySqlPassword { get; set; }
        public int MySqlPort { get; set; }
    }

    public class PersonData
    {
        public ulong PlayerSteamID { get; set; }
        public string? MusicKit { get; set; }
        public bool Client_Mute_MVP { get; set; }
        public DateTime DateAndTime { get; set; }
    }

    public static async Task CreatePersonDataTableIfNotExistsAsync(MySqlConnection connection)
    {
        string query = @"CREATE TABLE IF NOT EXISTS PersonData (
                    PlayerSteamID BIGINT UNSIGNED PRIMARY KEY,
                    MusicKit VARCHAR(255),
                    Client_Mute_MVP BOOLEAN,
                    DateAndTime DATETIME
                );";

        try
        {
            using (var command = new MySqlCommand(query, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"======================== ERROR =============================");
            Console.WriteLine($"Error creating PersonData table: {ex.Message}");
            Console.WriteLine($"======================== ERROR =============================");
            throw;
        }
    }

    public static async Task SaveToMySqlAsync(ulong PlayerSteamID, string MusicKit, bool Client_Mute_MVP, DateTime DateAndTime, MySqlConnection connection, MySqlConnectionSettings connectionSettings)
    {
        int days = Configs.GetConfigData().MVP_AutoRemovePlayerMySqlOlderThanXDays;
        string deleteOldRecordsQuery = $"DELETE FROM PersonData WHERE DateAndTime < NOW() - INTERVAL {days} DAY";

        string insertOrUpdateQuery = @"
        INSERT INTO PersonData (PlayerSteamID, MusicKit, Client_Mute_MVP, DateAndTime)
        VALUES (@PlayerSteamID, @MusicKit, @Client_Mute_MVP, @DateAndTime)
        ON DUPLICATE KEY UPDATE 
            MusicKit = VALUES(MusicKit), 
            Client_Mute_MVP = VALUES(Client_Mute_MVP), 
            DateAndTime = VALUES(DateAndTime)";

        try
        {
            using (var deleteCommand = new MySqlCommand(deleteOldRecordsQuery, connection))
            {
                await deleteCommand.ExecuteNonQueryAsync();
            }

            using (var command = new MySqlCommand(insertOrUpdateQuery, connection))
            {
                command.Parameters.AddWithValue("@PlayerSteamID", PlayerSteamID);
                command.Parameters.AddWithValue("@MusicKit", MusicKit);
                command.Parameters.AddWithValue("@Client_Mute_MVP", Client_Mute_MVP);
                command.Parameters.AddWithValue("@DateAndTime", DateAndTime);

                await command.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"======================== ERROR =============================");
            Console.WriteLine($"Error saving data to MySQL: {ex.Message}");
            Console.WriteLine($"======================== ERROR =============================");
            throw;
        }
    }
    public static async Task RemoveFromMySqlAsync(ulong PlayerSteamID, MySqlConnection connection, MySqlConnectionSettings connectionSettings)
    {
        try
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM PersonData WHERE PlayerSteamID = @PlayerSteamID";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PlayerSteamID", PlayerSteamID);

                await command.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing data from MySQL: {ex.Message}");
            throw;
        }
        finally
        {
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }
    }

    public static async Task<PersonData> RetrievePersonDataByIdAsync(ulong targetId, MySqlConnection connection)
    {
        int days = Configs.GetConfigData().MVP_AutoRemovePlayerMySqlOlderThanXDays;
        string deleteOldRecordsQuery = $"DELETE FROM PersonData WHERE DateAndTime < NOW() - INTERVAL {days} DAY";

        string retrieveQuery = "SELECT * FROM PersonData WHERE PlayerSteamID = @PlayerSteamID";
        var personData = new PersonData();

        try
        {
            using (var deleteCommand = new MySqlCommand(deleteOldRecordsQuery, connection))
            {
                await deleteCommand.ExecuteNonQueryAsync();
            }

            using (var command = new MySqlCommand(retrieveQuery, connection))
            {
                command.Parameters.AddWithValue("@PlayerSteamID", targetId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        personData = new PersonData
                        {
                            PlayerSteamID = Convert.ToUInt64(reader["PlayerSteamID"]),
                            MusicKit = Convert.ToString(reader["MusicKit"]),
                            Client_Mute_MVP = Convert.ToBoolean(reader["Client_Mute_MVP"]),
                            DateAndTime = Convert.ToDateTime(reader["DateAndTime"])
                        };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"======================== ERROR =============================");
            Console.WriteLine($"Error retrieving data from MySQL: {ex.Message}");
            Console.WriteLine($"======================== ERROR =============================");
            throw;
        }
        return personData;
    }
}