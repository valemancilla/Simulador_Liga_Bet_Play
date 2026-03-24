namespace Simulador_Liga_Bet_Play;

/// <summary>
/// Representa un equipo de la Liga BetPlay con sus estadísticas competitivas.
/// </summary>
public class Equipo
{
    public string Nombre { get; set; } = string.Empty;

    public int PJ { get; set; }

    public int PG { get; set; }

    public int PE { get; set; }

    public int PP { get; set; }

    public int GF { get; set; }

    public int GC { get; set; }

    public int TP { get; set; }

    public int DiferenciaGol => GF - GC;

    /// <summary>
    /// Actualiza estadísticas tras un partido desde la perspectiva de este equipo.
    /// </summary>
    /// <param name="golesMarcados">Goles anotados por el equipo.</param>
    /// <param name="golesRecibidos">Goles encajados por el equipo.</param>
    public void AplicarResultado(int golesMarcados, int golesRecibidos)
    {
        PJ++;
        GF += golesMarcados;
        GC += golesRecibidos;

        if (golesMarcados > golesRecibidos)
        {
            PG++;
            TP += 3;
        }
        else if (golesMarcados == golesRecibidos)
        {
            PE++;
            TP += 1;
        }
        else
        {
            PP++;
        }
    }

    public override string ToString()
    {
        return $"{Nombre} | PJ:{PJ} PG:{PG} PE:{PE} PP:{PP} GF:{GF} GC:{GC} DG:{DiferenciaGol} TP:{TP}";
    }
}
