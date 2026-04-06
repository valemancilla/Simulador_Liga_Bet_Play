using System.Globalization;

namespace Simulador_Liga_Bet_Play;

public class Torneo
{
    private readonly List<Equipo> _equipos = [];

    public IReadOnlyList<Equipo> Equipos => _equipos;

    public void CargarEstado(IReadOnlyList<Equipo> equipos)
    {
        _equipos.Clear();
        _equipos.AddRange(equipos);
    }

    public bool RegistrarEquipo(string nombre)
    {
        var normalizado = nombre.Trim();
        if (string.IsNullOrWhiteSpace(normalizado))
            return false;

        var existe = _equipos.Any(e =>
            string.Equals(e.Nombre, normalizado, StringComparison.OrdinalIgnoreCase));
        if (existe)
            return false;

        _equipos.Add(new Equipo { Nombre = normalizado });
        return true;
    }

    public Equipo? BuscarPorNombre(string nombre)
    {
        var n = nombre.Trim();
        return _equipos.FirstOrDefault(e =>
            string.Equals(e.Nombre, n, StringComparison.OrdinalIgnoreCase));
    }

    public IReadOnlyList<Equipo> ObtenerTablaPosiciones()
    {
        return _equipos
            .OrderByDescending(e => e.TP)
            .ThenByDescending(e => e.DiferenciaGol)
            .ThenByDescending(e => e.GF)
            .ThenBy(e => e.Nombre, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public IReadOnlyList<Equipo> ObtenerEquiposOrdenadosPorPuntos()
    {
        return ObtenerTablaPosiciones();
    }

    public IReadOnlyList<string> ObtenerLineasTablaPosicionesCompleta()
    {
        return ObtenerTablaPosiciones()
            .Select((e, i) => string.Create(CultureInfo.InvariantCulture,
                $"{i + 1,2}. {e.Nombre,-24} PTS:{e.TP,3} DG:{e.DiferenciaGol,4} GF:{e.GF,3} GC:{e.GC,3} PJ:{e.PJ} PG:{e.PG} PE:{e.PE} PP:{e.PP}"))
            .ToList();
    }

    public IReadOnlyList<Equipo> ObtenerEquiposQueNoHanPerdidoNingunPartido()
    {
        return ObtenerEquiposInvictos();
    }

    public void SimularPartido(Equipo local, Equipo visitante, int golesLocal, int golesVisitante)
    {
        if (local == visitante)
            throw new ArgumentException("Los equipos deben ser distintos.", nameof(visitante));

        local.AplicarResultado(golesLocal, golesVisitante);
        visitante.AplicarResultado(golesVisitante, golesLocal);
    }

    public Equipo? ObtenerLider()
    {
        return ObtenerTablaPosiciones().FirstOrDefault();
    }

    public IReadOnlyList<Equipo> ObtenerEquiposMasGolesAFavor()
    {
        if (_equipos.Count == 0)
            return [];

        var max = _equipos.Max(e => e.GF);
        return _equipos.Where(e => e.GF == max).ToList();
    }

    public IReadOnlyList<Equipo> ObtenerEquiposMenosGolesEnContra()
    {
        if (_equipos.Count == 0)
            return [];

        var min = _equipos.Min(e => e.GC);
        return _equipos.Where(e => e.GC == min).ToList();
    }

    public IReadOnlyList<Equipo> ObtenerEquiposMasVictorias()
    {
        if (_equipos.Count == 0)
            return [];

        var max = _equipos.Max(e => e.PG);
        return _equipos.Where(e => e.PG == max).ToList();
    }

    public IReadOnlyList<Equipo> ObtenerEquiposMasEmpates()
    {
        if (_equipos.Count == 0)
            return [];

        var max = _equipos.Max(e => e.PE);
        return _equipos.Where(e => e.PE == max).ToList();
    }

    public IReadOnlyList<Equipo> ObtenerEquiposMasDerrotas()
    {
        if (_equipos.Count == 0)
            return [];

        var max = _equipos.Max(e => e.PP);
        return _equipos.Where(e => e.PP == max).ToList();
    }

    public string ObtenerResumenMasEmpatesYMasDerrotas()
    {
        if (_equipos.Count == 0)
            return "No hay equipos registrados.";

        static string Nombres(IReadOnlyList<Equipo> lista) =>
            lista.Count > 0 ? string.Join(", ", lista.Select(e => e.Nombre)) : "—";

        var masEmpates = ObtenerEquiposMasEmpates();
        var masDerrotas = ObtenerEquiposMasDerrotas();
        return
            $"Equipos con más partidos empatados: {Nombres(masEmpates)}\n" +
            $"Equipos con más partidos perdidos: {Nombres(masDerrotas)}";
    }

    public IReadOnlyList<Equipo> ObtenerEquiposInvictos()
    {
        return _equipos.Where(e => e.PP == 0).ToList();
    }

    public IReadOnlyList<Equipo> ObtenerEquiposSinVictorias()
    {
        return _equipos.Where(e => e.PG == 0).ToList();
    }

    public IReadOnlyList<Equipo> ObtenerTop3()
    {
        return ObtenerTablaPosiciones().Take(3).ToList();
    }

    public IReadOnlyList<Equipo> ObtenerEquiposDiferenciaGolPositiva()
    {
        return _equipos.Where(e => e.DiferenciaGol > 0).ToList();
    }

    public IReadOnlyList<Equipo> ObtenerEquiposConMasPuntosQue(int puntosMinimos)
    {
        return _equipos.Where(e => e.TP > puntosMinimos).ToList();
    }

    public double PromedioGolesAFavor()
    {
        if (_equipos.Count == 0)
            return 0;
        return _equipos.Average(e => e.GF);
    }

    public double PromedioGolesEnContra()
    {
        if (_equipos.Count == 0)
            return 0;
        return _equipos.Average(e => e.GC);
    }

    public int TotalGolesMarcadosTorneo()
    {
        return _equipos.Sum(e => e.GF);
    }

    public int TotalPuntosSumados()
    {
        return _equipos.Sum(e => e.TP);
    }

    public IReadOnlyList<Equipo> ObtenerEquiposOrdenadosAlfabeticamente()
    {
        return _equipos.OrderBy(e => e.Nombre, StringComparer.OrdinalIgnoreCase).ToList();
    }

    public double PromedioPuntosPorEquipo()
    {
        if (_equipos.Count == 0)
            return 0;
        return _equipos.Average(e => e.TP);
    }

    public IReadOnlyList<Equipo> ObtenerEquiposPorDebajoDelPromedioPuntos()
    {
        if (_equipos.Count == 0)
            return [];

        var promedio = PromedioPuntosPorEquipo();
        return _equipos.Where(e => e.TP < promedio).ToList();
    }

    public IReadOnlyList<(int Posicion, string Nombre, int TP, int DG, int GF, int GC)> ObtenerTablaProyeccionPersonalizada()
    {
        return ObtenerTablaPosiciones()
            .Select((e, i) => (Posicion: i + 1, e.Nombre, e.TP, DG: e.DiferenciaGol, e.GF, e.GC))
            .ToList();
    }

    public IOrderedEnumerable<IGrouping<int, Equipo>> ObtenerRankingAgrupadoPorPuntos()
    {
        return _equipos
            .GroupBy(e => e.TP)
            .OrderByDescending(g => g.Key);
    }

    public string ObtenerResumenGeneralTorneo()
    {
        var n = _equipos.Count;
        if (n == 0)
            return "No hay equipos registrados.";

        var partidosTotales = _equipos.Sum(e => e.PJ) / 2;
        var lider = ObtenerLider();
        var masGf = ObtenerEquiposMasGolesAFavor();
        var menosGc = ObtenerEquiposMenosGolesEnContra();

        var liderTxt = lider?.Nombre ?? "—";
        var ofensiva = masGf.Count > 0 ? string.Join(", ", masGf.Select(e => e.Nombre)) : "—";
        var defensiva = menosGc.Count > 0 ? string.Join(", ", menosGc.Select(e => e.Nombre)) : "—";

        return string.Create(CultureInfo.InvariantCulture,
            $"Equipos: {n} | Partidos disputados (total): {partidosTotales} | " +
            $"GF totales: {TotalGolesMarcadosTorneo()} | Puntos totales (suma equipos): {TotalPuntosSumados()} | " +
            $"Líder: {liderTxt} | Mejor ofensiva (GF): {ofensiva} | Mejor defensiva (menos GC): {defensiva}");
    }

    public string ObtenerTextoEstadisticasDestacadas()
    {
        if (_equipos.Count == 0)
            return "No hay equipos registrados.";

        static string Nombres(IEnumerable<Equipo> lista) =>
            lista.Any() ? string.Join(", ", lista.Select(e => e.Nombre)) : "—";

        var sb = new System.Text.StringBuilder();
        var lider = ObtenerLider();
        sb.AppendLine($"Líder del torneo: {lider?.Nombre ?? "—"} ({lider?.TP ?? 0} pts, DG {lider?.DiferenciaGol ?? 0})");
        sb.AppendLine($"Más goles a favor: {Nombres(ObtenerEquiposMasGolesAFavor())}");
        sb.AppendLine($"Menos goles en contra: {Nombres(ObtenerEquiposMenosGolesEnContra())}");
        sb.AppendLine($"Más victorias: {Nombres(ObtenerEquiposMasVictorias())}");
        sb.AppendLine($"Más empates: {Nombres(ObtenerEquiposMasEmpates())}");
        sb.AppendLine($"Más derrotas: {Nombres(ObtenerEquiposMasDerrotas())}");
        sb.AppendLine($"Invictos (0 derrotas): {Nombres(ObtenerEquiposInvictos())}");
        sb.AppendLine($"Sin victorias: {Nombres(ObtenerEquiposSinVictorias())}");
        sb.AppendLine($"Top 3: {Nombres(ObtenerTop3())}");
        sb.AppendLine($"Diferencia de gol positiva: {Nombres(ObtenerEquiposDiferenciaGolPositiva())}");
        return sb.ToString().TrimEnd();
    }
}
