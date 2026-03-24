using System.Globalization;
using Simulador_Liga_Bet_Play;

internal static class Program
{
    private static readonly Torneo Torneo = new();

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Simulador de la Liga BetPlay";

        bool salir;
        do
        {
            LimpiarYMensaje(null);
            Console.WriteLine("1. Listar equipos");
            Console.WriteLine("2. Registrar equipos");
            Console.WriteLine("3. Simular partidos");
            Console.WriteLine("4. Ver tabla de posiciones");
            Console.WriteLine("5. Consultar estadísticas");
            Console.WriteLine("6. Salir");
            Console.Write("\nSeleccione una opción: ");

            var opcion = Console.ReadLine()?.Trim();
            salir = opcion == "6";

            LimpiarYMensaje(null);

            switch (opcion)
            {
                case "1":
                    ListarEquiposPantalla();
                    break;
                case "2":
                    RegistrarEquiposPantalla();
                    break;
                case "3":
                    SimularPartidosPantalla();
                    break;
                case "4":
                    VerTablaPosicionesPantalla();
                    break;
                case "5":
                    ConsultarEstadisticasPantalla();
                    break;
                case "6":
                    Console.WriteLine("Gracias por usar el Simulador de la Liga BetPlay.");
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    PausaYVolver();
                    break;
            }

            if (opcion is "1" or "2" or "3" or "4")
            {
                PausaYVolver();
            }
        } while (!salir);
    }

    /// <summary>
    /// Limpia la consola antes de cada pantalla o prompt (interacción).
    /// </summary>
    private static void LimpiarYMensaje(string? subtitulo)
    {
        Console.Clear();
        MostrarTituloPrincipal();
        if (!string.IsNullOrWhiteSpace(subtitulo))
        {
            Console.WriteLine(subtitulo);
            Console.WriteLine();
        }
    }

    private static void MostrarTituloPrincipal()
    {
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("   SIMULADOR DE LA LIGA BETPLAY");
        Console.WriteLine("═══════════════════════════════════════\n");
    }

    private static void PausaYVolver()
    {
        Console.WriteLine("\nPresione una tecla para continuar...");
        Console.ReadKey(true);
        LimpiarYMensaje(null);
    }

    private static void ListarEquiposPantalla()
    {
        LimpiarYMensaje("--- LISTADO DE EQUIPOS ---");

        if (Torneo.Equipos.Count == 0)
        {
            Console.WriteLine("No hay equipos registrados.");
            return;
        }

        var ordenados = Torneo.ObtenerEquiposOrdenadosAlfabeticamente();
        foreach (var e in ordenados)
            Console.WriteLine(e.ToString());
    }

    private static void RegistrarEquiposPantalla()
    {
        LimpiarYMensaje("--- REGISTRAR EQUIPO ---");
        Console.Write("Nombre del equipo (vacío para cancelar): ");
        var nombre = Console.ReadLine() ?? string.Empty;

        LimpiarYMensaje("--- REGISTRAR EQUIPO — RESULTADO ---");

        if (string.IsNullOrWhiteSpace(nombre))
        {
            Console.WriteLine("Registro cancelado.");
            return;
        }

        if (Torneo.RegistrarEquipo(nombre))
            Console.WriteLine($"Equipo '{nombre.Trim()}' registrado correctamente.");
        else
            Console.WriteLine("No se pudo registrar: nombre vacío, duplicado o ya existe un equipo igual (ignora mayúsculas).");
    }

    private static void SimularPartidosPantalla()
    {
        LimpiarYMensaje("--- SIMULAR PARTIDO ---");

        if (Torneo.Equipos.Count < 2)
        {
            Console.WriteLine("Se necesitan al menos dos equipos para simular un partido.");
            return;
        }

        var lista = Torneo.ObtenerEquiposOrdenadosAlfabeticamente().ToList();

        LimpiarYMensaje("--- SIMULAR PARTIDO — ELEGIR LOCAL ---");
        MostrarIndiceEquipos(lista);
        Console.Write("Índice del equipo LOCAL: ");
        var lineaLocal = Console.ReadLine();
        if (!TryParseIndice(lineaLocal, lista.Count, out var iLocal))
        {
            LimpiarYMensaje("--- SIMULAR PARTIDO ---");
            Console.WriteLine("Operación cancelada o índice no válido.");
            return;
        }

        var local = lista[iLocal];

        LimpiarYMensaje("--- SIMULAR PARTIDO — ELEGIR VISITANTE ---");
        Console.WriteLine($"Equipo local: {local.Nombre}\n");
        MostrarIndiceEquipos(lista);
        Console.Write("Índice del equipo VISITANTE: ");
        var lineaVisita = Console.ReadLine();
        if (!TryParseIndice(lineaVisita, lista.Count, out var iVisita))
        {
            LimpiarYMensaje("--- SIMULAR PARTIDO ---");
            Console.WriteLine("Operación cancelada o índice no válido.");
            return;
        }

        if (iLocal == iVisita)
        {
            LimpiarYMensaje("--- SIMULAR PARTIDO ---");
            Console.WriteLine("Debe elegir dos equipos distintos.");
            return;
        }

        var visita = lista[iVisita];

        LimpiarYMensaje("--- SIMULAR PARTIDO — MARCADOR ---");
        Console.WriteLine($"{local.Nombre} (local) vs {visita.Nombre} (visitante)\n");
        Console.WriteLine("¿Cómo desea definir el marcador?");
        Console.WriteLine("1. Ingresar goles manualmente");
        Console.WriteLine("2. Generar goles al azar (0 a 5 por equipo)");
        Console.Write("Opción: ");
        var modo = Console.ReadLine()?.Trim();

        int gl;
        int gv;

        switch (modo)
        {
            case "1":
                LimpiarYMensaje("--- SIMULAR PARTIDO — GOLES DEL LOCAL ---");
                Console.WriteLine($"{local.Nombre} vs {visita.Nombre}\n");
                Console.Write("Goles del LOCAL: ");
                var lineaGl = Console.ReadLine();
                if (!TryParseNoNegativo(lineaGl, out gl))
                {
                    LimpiarYMensaje("--- SIMULAR PARTIDO ---");
                    Console.WriteLine("Operación cancelada o valor no válido.");
                    return;
                }

                LimpiarYMensaje("--- SIMULAR PARTIDO — GOLES DEL VISITANTE ---");
                Console.WriteLine($"{local.Nombre} {gl} - ? {visita.Nombre}\n");
                Console.Write("Goles del VISITANTE: ");
                var lineaGv = Console.ReadLine();
                if (!TryParseNoNegativo(lineaGv, out gv))
                {
                    LimpiarYMensaje("--- SIMULAR PARTIDO ---");
                    Console.WriteLine("Operación cancelada o valor no válido.");
                    return;
                }

                break;

            case "2":
                var rnd = Random.Shared;
                gl = rnd.Next(0, 6);
                gv = rnd.Next(0, 6);
                LimpiarYMensaje("--- SIMULAR PARTIDO — MARCADOR ALEATORIO ---");
                Console.WriteLine($"Marcador generado: {local.Nombre} {gl} - {gv} {visita.Nombre}");
                break;

            default:
                LimpiarYMensaje("--- SIMULAR PARTIDO ---");
                Console.WriteLine("Opción no válida.");
                return;
        }

        Torneo.SimularPartido(local, visita, gl, gv);

        LimpiarYMensaje("--- SIMULAR PARTIDO — PARTIDO REGISTRADO ---");
        Console.WriteLine("Partido registrado. Estadísticas actualizadas para ambos equipos.");
        Console.WriteLine($"Resultado final: {local.Nombre} {gl} - {gv} {visita.Nombre}");
    }

    private static void VerTablaPosicionesPantalla()
    {
        LimpiarYMensaje("--- TABLA DE POSICIONES ---");
        Console.WriteLine("(Orden: Puntos ↓, Diferencia de gol ↓, Goles a favor ↓, Nombre ↑)\n");

        if (Torneo.Equipos.Count == 0)
        {
            Console.WriteLine("No hay equipos registrados.");
            return;
        }

        foreach (var linea in Torneo.ObtenerLineasTablaPosicionesCompleta())
            Console.WriteLine(linea);
    }

    private static void ConsultarEstadisticasPantalla()
    {
        do
        {
            LimpiarYMensaje("--- CONSULTAR ESTADÍSTICAS (LINQ) ---");
            Console.WriteLine(" 1. Líder del torneo");
            Console.WriteLine(" 2. Equipo(s) con más goles a favor");
            Console.WriteLine(" 3. Equipo(s) con menos goles en contra");
            Console.WriteLine(" 4. Equipo(s) con más victorias");
            Console.WriteLine(" 5. Equipo(s) con más empates");
            Console.WriteLine(" 6. Equipo(s) con más derrotas");
            Console.WriteLine(" 7. Equipos invictos (sin derrotas)");
            Console.WriteLine(" 8. Equipos sin victorias");
            Console.WriteLine(" 9. Top 3 de la tabla");
            Console.WriteLine("10. Equipos con diferencia de gol positiva");
            Console.WriteLine("11. Equipos con más de X puntos");
            Console.WriteLine("12. Buscar equipo por nombre");
            Console.WriteLine("13. Promedio de goles a favor del torneo");
            Console.WriteLine("14. Promedio de goles en contra del torneo");
            Console.WriteLine("15. Total de goles marcados en el torneo");
            Console.WriteLine("16. Total de puntos sumados por todos los equipos");
            Console.WriteLine("17. Tabla con proyección personalizada");
            Console.WriteLine("18. Equipos ordenados alfabéticamente");
            Console.WriteLine("19. Resumen general del torneo");
            Console.WriteLine("20. Equipos por debajo del promedio de puntos");
            Console.WriteLine("21. Ranking agrupado por puntos");
            Console.WriteLine("22. Panel de estadísticas destacadas");
            Console.WriteLine("23. Equipos ordenados por puntos");
            Console.WriteLine("24. Equipos que no han perdido ningún partido");
            Console.WriteLine("25. Tabla de posiciones (método completo, LINQ)");
            Console.WriteLine("26. Más empates y más derrotas (doc. sección 9.6)");
            Console.WriteLine(" 0. Volver al menú principal");
            Console.Write("\nOpción: ");

            var op = Console.ReadLine()?.Trim();

            if (op == "0")
            {
                LimpiarYMensaje(null);
                break;
            }

            switch (op)
            {
                case "1":
                    MostrarLider();
                    break;
                case "2":
                    MostrarLista("Más goles a favor", Torneo.ObtenerEquiposMasGolesAFavor());
                    break;
                case "3":
                    MostrarLista("Menos goles en contra", Torneo.ObtenerEquiposMenosGolesEnContra());
                    break;
                case "4":
                    MostrarLista("Más victorias", Torneo.ObtenerEquiposMasVictorias());
                    break;
                case "5":
                    MostrarLista("Más empates", Torneo.ObtenerEquiposMasEmpates());
                    break;
                case "6":
                    MostrarLista("Más derrotas", Torneo.ObtenerEquiposMasDerrotas());
                    break;
                case "7":
                    MostrarLista("Invictos (PP = 0)", Torneo.ObtenerEquiposInvictos());
                    break;
                case "8":
                    MostrarLista("Sin victorias (PG = 0)", Torneo.ObtenerEquiposSinVictorias());
                    break;
                case "9":
                    MostrarLista("Top 3", Torneo.ObtenerTop3());
                    break;
                case "10":
                    MostrarLista("Diferencia de gol > 0", Torneo.ObtenerEquiposDiferenciaGolPositiva());
                    break;
                case "11":
                    ConsultaMasDeXPuntos();
                    break;
                case "12":
                    BuscarEquipoPorNombreConsulta();
                    break;
                case "13":
                    LimpiarYMensaje("--- PROMEDIO GOLES A FAVOR ---");
                    Console.WriteLine($"Promedio GF (por equipo): {Torneo.PromedioGolesAFavor():F2}");
                    break;
                case "14":
                    LimpiarYMensaje("--- PROMEDIO GOLES EN CONTRA ---");
                    Console.WriteLine($"Promedio GC (por equipo): {Torneo.PromedioGolesEnContra():F2}");
                    break;
                case "15":
                    LimpiarYMensaje("--- TOTAL GOLES MARCADOS ---");
                    Console.WriteLine($"Total goles marcados (suma GF): {Torneo.TotalGolesMarcadosTorneo()}");
                    break;
                case "16":
                    LimpiarYMensaje("--- TOTAL PUNTOS SUMADOS ---");
                    Console.WriteLine($"Total puntos sumados: {Torneo.TotalPuntosSumados()}");
                    break;
                case "17":
                    MostrarProyeccionPersonalizada();
                    break;
                case "18":
                    MostrarLista("Orden alfabético", Torneo.ObtenerEquiposOrdenadosAlfabeticamente());
                    break;
                case "19":
                    LimpiarYMensaje("--- RESUMEN GENERAL DEL TORNEO ---");
                    Console.WriteLine(Torneo.ObtenerResumenGeneralTorneo());
                    break;
                case "20":
                    MostrarLista(
                        $"Por debajo del promedio de puntos ({Torneo.PromedioPuntosPorEquipo():F2})",
                        Torneo.ObtenerEquiposPorDebajoDelPromedioPuntos());
                    break;
                case "21":
                    MostrarRankingAgrupado();
                    break;
                case "22":
                    LimpiarYMensaje("--- ESTADÍSTICAS DESTACADAS ---");
                    Console.WriteLine(Torneo.ObtenerTextoEstadisticasDestacadas());
                    break;
                case "23":
                    MostrarLista("Equipos ordenados por puntos", Torneo.ObtenerEquiposOrdenadosPorPuntos());
                    break;
                case "24":
                    MostrarLista("Equipos que no han perdido ningún partido (PP = 0)", Torneo.ObtenerEquiposQueNoHanPerdidoNingunPartido());
                    break;
                case "25":
                    MostrarTablaPosicionesMetodoCompleto();
                    break;
                case "26":
                    LimpiarYMensaje("--- MÁS EMPATES Y MÁS DERROTAS ---");
                    Console.WriteLine(Torneo.ObtenerResumenMasEmpatesYMasDerrotas());
                    break;
                default:
                    LimpiarYMensaje("--- CONSULTA ---");
                    Console.WriteLine("Opción no válida.");
                    break;
            }

            PausaYVolver();
        } while (true);
    }

    private static void MostrarLider()
    {
        LimpiarYMensaje("--- LÍDER DEL TORNEO ---");
        var l = Torneo.ObtenerLider();
        if (l == null)
            Console.WriteLine("No hay equipos registrados.");
        else
            Console.WriteLine($"Líder: {l.Nombre} | {l.TP} pts | DG {l.DiferenciaGol} | GF {l.GF} GC {l.GC}");
    }

    private static void MostrarLista(string titulo, IReadOnlyList<Equipo> equipos)
    {
        LimpiarYMensaje($"--- {titulo} ---");
        if (equipos.Count == 0)
        {
            Console.WriteLine("Sin resultados o no hay equipos.");
            return;
        }

        foreach (var e in equipos)
            Console.WriteLine(e.ToString());
    }

    private static void ConsultaMasDeXPuntos()
    {
        LimpiarYMensaje("--- EQUIPOS CON MÁS DE X PUNTOS ---");
        Console.Write("Ingrese el umbral mínimo de puntos (entero): ");
        var linea = Console.ReadLine();

        LimpiarYMensaje("--- EQUIPOS CON MÁS DE X PUNTOS — RESULTADO ---");

        if (!int.TryParse(linea, NumberStyles.Integer, CultureInfo.InvariantCulture, out var x))
        {
            Console.WriteLine("Valor no válido.");
            return;
        }

        var lista = Torneo.ObtenerEquiposConMasPuntosQue(x);
        if (lista.Count == 0)
            Console.WriteLine($"Ningún equipo tiene más de {x} puntos.");
        else
            foreach (var e in lista)
                Console.WriteLine(e.ToString());
    }

    private static void BuscarEquipoPorNombreConsulta()
    {
        LimpiarYMensaje("--- BUSCAR EQUIPO POR NOMBRE ---");
        Console.Write("Nombre a buscar: ");
        var nombre = Console.ReadLine() ?? string.Empty;

        LimpiarYMensaje("--- BUSCAR EQUIPO — RESULTADO ---");

        var e = Torneo.BuscarPorNombre(nombre);
        if (e == null)
            Console.WriteLine("No se encontró el equipo.");
        else
            Console.WriteLine(e.ToString());
    }

    private static void MostrarTablaPosicionesMetodoCompleto()
    {
        LimpiarYMensaje("--- TABLA DE POSICIONES (método completo) ---");
        Console.WriteLine("(Orden: Puntos ↓, Diferencia de gol ↓, Goles a favor ↓, Nombre ↑)\n");

        if (Torneo.Equipos.Count == 0)
        {
            Console.WriteLine("No hay equipos registrados.");
            return;
        }

        foreach (var linea in Torneo.ObtenerLineasTablaPosicionesCompleta())
            Console.WriteLine(linea);
    }

    private static void MostrarProyeccionPersonalizada()
    {
        LimpiarYMensaje("--- TABLA (proyección personalizada) ---");
        if (Torneo.Equipos.Count == 0)
        {
            Console.WriteLine("No hay equipos.");
            return;
        }

        foreach (var fila in Torneo.ObtenerTablaProyeccionPersonalizada())
        {
            Console.WriteLine(
                string.Create(CultureInfo.InvariantCulture,
                    $"{fila.Posicion,2}. {fila.Nombre,-22} PTS:{fila.TP,3} DG:{fila.DG,4} GF:{fila.GF,3} GC:{fila.GC,3}"));
        }
    }

    private static void MostrarRankingAgrupado()
    {
        LimpiarYMensaje("--- RANKING AGRUPADO POR PUNTOS ---");
        if (Torneo.Equipos.Count == 0)
        {
            Console.WriteLine("No hay equipos.");
            return;
        }

        foreach (var grupo in Torneo.ObtenerRankingAgrupadoPorPuntos())
        {
            Console.WriteLine($"Puntos {grupo.Key}:");
            foreach (var e in grupo.OrderBy(x => x.Nombre, StringComparer.OrdinalIgnoreCase))
                Console.WriteLine($"   • {e.Nombre}");
            Console.WriteLine();
        }
    }

    private static void MostrarIndiceEquipos(IReadOnlyList<Equipo> lista)
    {
        for (var i = 0; i < lista.Count; i++)
            Console.WriteLine($"{i,3}. {lista[i].Nombre}");
        Console.WriteLine();
    }

    private static bool TryParseIndice(string? linea, int cantidad, out int idx)
    {
        idx = -1;
        if (string.IsNullOrWhiteSpace(linea))
            return false;

        if (!int.TryParse(linea.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out idx))
            return false;

        return idx >= 0 && idx < cantidad;
    }

    private static bool TryParseNoNegativo(string? linea, out int n)
    {
        n = -1;
        if (string.IsNullOrWhiteSpace(linea))
            return false;

        if (!int.TryParse(linea.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out n))
            return false;

        return n >= 0;
    }
}
