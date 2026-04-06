using System.Text.Json;

namespace Simulador_Liga_Bet_Play;

public static class TorneoPersistencia
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static string ObtenerRutaArchivo() =>
        Path.Combine(AppContext.BaseDirectory, "torneo.json");

    public static void CargarSiExiste(Torneo torneo)
    {
        var ruta = ObtenerRutaArchivo();
        if (!File.Exists(ruta))
            return;

        try
        {
            var json = File.ReadAllText(ruta);
            var snapshot = JsonSerializer.Deserialize<TorneoSnapshot>(json, JsonOptions);
            if (snapshot?.Equipos == null)
                return;

            torneo.CargarEstado(snapshot.Equipos);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Advertencia: no se pudo cargar los datos guardados ({ex.Message}).");
            Console.WriteLine("Se continúa con un torneo vacío. Presione una tecla...");
            Console.ReadKey(true);
        }
    }

    public static void Guardar(Torneo torneo)
    {
        var snapshot = new TorneoSnapshot { Equipos = [.. torneo.Equipos] };
        var json = JsonSerializer.Serialize(snapshot, JsonOptions);
        File.WriteAllText(ObtenerRutaArchivo(), json);
    }

    private sealed class TorneoSnapshot
    {
        public List<Equipo> Equipos { get; set; } = [];
    }
}
