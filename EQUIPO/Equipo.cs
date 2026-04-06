using System.Text.Json.Serialization;

namespace Simulador_Liga_Bet_Play;

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

    [JsonIgnore]
    public int DiferenciaGol => GF - GC;

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
