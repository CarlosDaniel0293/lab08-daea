using System;
using StackExchange.Redis;
using System.Threading.Tasks;
using Npgsql;  // Asegúrate de agregar esta línea

class Program
{
    private static ConnectionMultiplexer redis;
    private static string postgresConnectionString = "Host=postgres;Port=5432;Username=your_user;Password=your_password;Database=your_database"; // Actualiza estos valores

    static async Task Main(string[] args)
    {
        // Conectarse a Redis
        redis = await ConnectionMultiplexer.ConnectAsync("redis:6379");

        // Mantener la aplicación activa
        while (true)
        {
            await GetVotes();
            await Task.Delay(5000); // Esperar 5 segundos antes de volver a obtener los votos
        }
    }

    private static async Task GetVotes()
    {
        var db = redis.GetDatabase();

        // Obtener los votos almacenados en Redis
        string option1Votes = await db.StringGetAsync("option1");
        string option2Votes = await db.StringGetAsync("option2");

        // Mostrar los resultados
        Console.WriteLine($"Option 1: {option1Votes ?? "0"} votos");
        Console.WriteLine($"Option 2: {option2Votes ?? "0"} votos");

        // Insertar los votos en PostgreSQL
        await InsertVotesIntoPostgres(option1Votes, option2Votes);

        // Restablecer los votos en Redis después de insertarlos en PostgreSQL
        await db.StringSetAsync("option1", "0");
        await db.StringSetAsync("option2", "0");
    }


    private static async Task InsertVotesIntoPostgres(string option1Votes, string option2Votes)
    {
        using (var conn = new NpgsqlConnection(postgresConnectionString))
        {
            await conn.OpenAsync();

            // Convierte las cadenas de votos a enteros, o 0 si no son válidas
            int option1VotesInt = int.TryParse(option1Votes, out var temp1) ? temp1 : 0;
            int option2VotesInt = int.TryParse(option2Votes, out var temp2) ? temp2 : 0;

            try
            {
                // Actualiza los votos en PostgreSQL
                using (var cmd = new NpgsqlCommand("UPDATE votes SET option1_votes = option1_votes + @option1, option2_votes = option2_votes + @option2 WHERE id = 1", conn))
                {
                    cmd.Parameters.AddWithValue("option1", option1VotesInt);
                    cmd.Parameters.AddWithValue("option2", option2VotesInt);
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        // Si no se actualizó ninguna fila, significa que no existe el registro
                        Console.WriteLine("No se pudo encontrar la fila para actualizar. Verifique la tabla.");
                    }
                    else
                    {
                        Console.WriteLine("Votes updated successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating votes: {ex.Message}");
            }
        }
    }


}
