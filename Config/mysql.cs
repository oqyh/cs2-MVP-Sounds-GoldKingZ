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
        public DateTime DateAndTime { get; set; }
    }

    public static async Task CreatePersonDataTableIfNotExistsAsync(MySqlConnection connection)
    {
        string query = @"CREATE TABLE IF NOT EXISTS PersonData (
                    PlayerSteamID BIGINT UNSIGNED PRIMARY KEY,
                    MusicKit VARCHAR(255),
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

    public static async Task SaveToMySqlAsync(ulong PlayerSteamID, string MusicKit, DateTime DateAndTime, MySqlConnection connection, MySqlConnectionSettings connectionSettings)
    {
        string query = @"INSERT INTO PersonData (PlayerSteamID, MusicKit, DateAndTime)
                        VALUES (@PlayerSteamID, @MusicKit, @DateAndTime)
                        ON DUPLICATE KEY UPDATE MusicKit = VALUES(MusicKit), DateAndTime = VALUES(DateAndTime)";

        try
        {
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PlayerSteamID", PlayerSteamID);
                command.Parameters.AddWithValue("@MusicKit", MusicKit);
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
        string query = "SELECT * FROM PersonData WHERE PlayerSteamID = @PlayerSteamID";
        var personData = new PersonData();

        using (var command = new MySqlCommand(query, connection))
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
                        DateAndTime = Convert.ToDateTime(reader["DateAndTime"])
                    };
                }
            }
        }
        return personData;
    }
}