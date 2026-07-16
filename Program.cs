using System;
using System.Diagnostics;

namespace ColaAtraccionParque
{
    // ==========================================================
    // Nombre: Nathalia Teresa Barreiro Castro (B)
    // curso: Estructura de datos
    // CLASE: Persona
    // Representa a cada visitante que llega a hacer fila para
    // subir a la atracción del parque de diversiones.
    // ==========================================================
    public class Persona
    {
        public int NumeroTurno { get; set; }      // Orden de llegada a la cola
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public DateTime HoraLlegada { get; set; }
        public int? AsientoAsignado { get; set; }  // Se asigna al desencolar (1..30)

        public Persona(int numeroTurno, string nombre, string cedula)
        {
            NumeroTurno = numeroTurno;
            Nombre = nombre;
            Cedula = cedula;
            HoraLlegada = DateTime.Now;
            AsientoAsignado = null;
        }

        public override string ToString()
        {
            string asiento = AsientoAsignado.HasValue ? AsientoAsignado.Value.ToString() : "En espera";
            return $"Turno #{NumeroTurno,-3} | {Nombre,-20} | Cédula: {Cedula,-10} | " +
                   $"Llegada: {HoraLlegada:HH:mm:ss} | Asiento: {asiento}";
        }
    }

    // ==========================================================
    // CLASE: NodoCola<T>
    // Nodo genérico enlazado que almacena un dato y una
    // referencia al siguiente nodo, es la unidad básica de la
    // estructura de datos "Cola" implementada desde cero
    // ==========================================================
    public class NodoCola<T>
    {
        public T Valor { get; set; }
        public NodoCola<T> Siguiente { get; set; }

        public NodoCola(T valor)
        {
            Valor = valor;
            Siguiente = null;
        }
    }

    // ==========================================================
    // CLASE: ColaGenerica<T>
    // Implementación propia (desde cero) del TAD Cola (Queue)
    // bajo el principio FIFO (First In, First Out) mediante
    // una lista enlazada simple con referencias a Frente y Final
    // ==========================================================
    public class ColaGenerica<T>
    {
        private NodoCola<T> frente;   // Primer elemento (el próximo en salir)
        private NodoCola<T> final;    // Último elemento (el último en entrar)
        private int contador;

        public int Cantidad => contador;
        public bool EstaVacia => contador == 0;

        public ColaGenerica()
        {
            frente = null;
            final = null;
            contador = 0;
        }

        // Encolar (Enqueue): agrega un elemento al final de la cola. O(1)
        public void Encolar(T elemento)
        {
            NodoCola<T> nuevo = new NodoCola<T>(elemento);
            if (EstaVacia)
            {
                frente = nuevo;
                final = nuevo;
            }
            else
            {
                final.Siguiente = nuevo;
                final = nuevo;
            }
            contador++;
        }

        // Desencolar (Dequeue): retira y retorna el elemento del frente. O(1)
        public T Desencolar()
        {
            if (EstaVacia)
                throw new InvalidOperationException("La cola está vacía. No hay personas por atender.");

            T valor = frente.Valor;
            frente = frente.Siguiente;
            contador--;

            if (frente == null)
                final = null;

            return valor;
        }

        // Ver el elemento en el frente sin retirarlo. O(1)
        public T VerFrente()
        {
            if (EstaVacia)
                throw new InvalidOperationException("La cola está vacía");
            return frente.Valor;
        }

        // Recorre la cola de frente a final sin modificarla (para reportería). O(n)
        public System.Collections.Generic.List<T> ObtenerListado()
        {
            var lista = new System.Collections.Generic.List<T>();
            NodoCola<T> actual = frente;
            while (actual != null)
            {
                lista.Add(actual.Valor);
                actual = actual.Siguiente;
            }
            return lista;
        }
    }

    // ==========================================================
    // CLASE: ColaAtraccion
    // Encapsula la lógica del sistema: administra la cola de
    // espera, la asignación de los 30 asientos y la reportería.
    // ==========================================================
    public class ColaAtraccion
    {
        private ColaGenerica<Persona> colaEspera;
        private System.Collections.Generic.List<Persona> historialAtendidos;
        private const int CUPO_MAXIMO = 30;
        private int asientosAsignados;

        public ColaAtraccion()
        {
            colaEspera = new ColaGenerica<Persona>();
            historialAtendidos = new System.Collections.Generic.List<Persona>();
            asientosAsignados = 0;
        }

        // Registrar la llegada de una persona a la fila
        public void RegistrarLlegada(Persona persona)
        {
            if (colaEspera.Cantidad + asientosAsignados >= CUPO_MAXIMO)
            {
                Console.WriteLine($"  [RECHAZADO] {persona.Nombre}: cupo de {CUPO_MAXIMO} asientos completo");
                return;
            }
            colaEspera.Encolar(persona);
            Console.WriteLine($"  [OK] {persona.Nombre} ingresó a la cola con el turno #{persona.NumeroTurno}");
        }

        // Asignar un asiento a la siguiente persona en el orden de llegada (FIFO)
        public Persona AsignarSiguienteAsiento()
        {
            if (colaEspera.EstaVacia)
            {
                Console.WriteLine("  No hay personas en espera");
                return null;
            }

            Persona persona = colaEspera.Desencolar();
            asientosAsignados++;
            persona.AsientoAsignado = asientosAsignados;
            historialAtendidos.Add(persona);
            return persona;
        }

        // ---------------- REPORTERÍA ----------------

        // Reporte 1: personas que aún están en la cola de espera
        public void ReportarColaEspera()
        {
            Console.WriteLine("\n===== REPORTE: PERSONAS EN COLA DE ESPERA =====");
            var listado = colaEspera.ObtenerListado();
            if (listado.Count == 0)
            {
                Console.WriteLine("  (Sin personas en espera)");
            }
            else
            {
                foreach (var p in listado)
                    Console.WriteLine("  " + p);
            }
            Console.WriteLine($"Total en espera: {listado.Count}");
        }

        // Reporte 2: personas que ya subieron a la atracción (asientos asignados)
        public void ReportarAsientosAsignados()
        {
            Console.WriteLine("\n===== REPORTE: ASIENTOS ASIGNADOS =====");
            if (historialAtendidos.Count == 0)
            {
                Console.WriteLine("  (Aún no se han asignado asientos)");
            }
            else
            {
                foreach (var p in historialAtendidos)
                    Console.WriteLine("  " + p);
            }
            Console.WriteLine($"Asientos ocupados: {historialAtendidos.Count} / {CUPO_MAXIMO}");
        }

        // Reporte 3: consulta puntual de una persona por cédula
        public void ConsultarPersona(string cedula)
        {
            Console.WriteLine($"\n===== CONSULTA: Cédula {cedula} =====");
            var enEspera = colaEspera.ObtenerListado();
            foreach (var p in enEspera)
            {
                if (p.Cedula == cedula)
                {
                    Console.WriteLine("  Estado: EN COLA DE ESPERA");
                    Console.WriteLine("  " + p);
                    return;
                }
            }
            foreach (var p in historialAtendidos)
            {
                if (p.Cedula == cedula)
                {
                    Console.WriteLine("  Estado: YA SUBIÓ A LA ATRACCIÓN");
                    Console.WriteLine("  " + p);
                    return;
                }
            }
            Console.WriteLine("  No se encontró ninguna persona con esa cédula.");
        }
    }

    // ==========================================================
    // CLASE: Program
    // Punto de entrada. Simula la llegada de 30 personas y su
    // paso ordenado por la atracción, además mide el tiempo de
    // ejecución del proceso completo.
    // ==========================================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("==================================================");
            Console.WriteLine(" SIMULACIÓN: COLA DE ESPERA - ATRACCIÓN DE PARQUE");
            Console.WriteLine(" Estructura de Datos: Cola (FIFO) - Implementación propia");
            Console.WriteLine("==================================================\n");

            ColaAtraccion sistema = new ColaAtraccion();
            Stopwatch cronometro = new Stopwatch();
            cronometro.Start();

            // 1) Simular la llegada de 30 personas en orden
            string[] nombres = {
                "Ana Torres","Luis Paredes","Carla Vega","Diego Ramos","María Suárez",
                "Pedro Ortiz","Sofía León","Jorge Castillo","Elena Vásquez","Iván Molina",
                "Paula Herrera","Andrés Chávez","Lucía Rojas","Marco Peña","Gabriela Salas",
                "Ricardo Flores","Daniela Cruz","Esteban Guerrero","Valeria Andrade","Julio Nieto",
                "Camila Reyes","Fernando Vargas","Isabel Campos","Tomás Delgado","Nicole Guevara",
                "Sebastián Cuenca","Fernanda Alvarado","Diego Espín","Karen Tapia","Oscar Jácome"
            };

            Console.WriteLine("--- Ingreso de personas a la cola de espera ---");
            for (int i = 0; i < nombres.Length; i++)
            {
                string cedula = (1700000000 + i).ToString();
                Persona persona = new Persona(i + 1, nombres[i], cedula);
                sistema.RegistrarLlegada(persona);
            }

            // 2) Reporte de la cola completa antes de subir a la atracción
            sistema.ReportarColaEspera();

            // 3) Asignar los 30 asientos en el mismo orden de llegada (FIFO)
            Console.WriteLine("\n--- Asignación de asientos en orden de llegada ---");
            for (int i = 0; i < nombres.Length; i++)
            {
                Persona atendido = sistema.AsignarSiguienteAsiento();
                if (atendido != null)
                    Console.WriteLine($"  Asiento {atendido.AsientoAsignado}: {atendido.Nombre}");
            }

            // 4) Reportes finales
            sistema.ReportarColaEspera();          // debe mostrar 0 personas en espera
            sistema.ReportarAsientosAsignados();    // debe mostrar los 30 asientos ocupados

            // 5) Consulta puntual de ejemplo (reportería)
            sistema.ConsultarPersona("1700000005");

            cronometro.Stop();

            Console.WriteLine("\n===== ANÁLISIS DE TIEMPO DE EJECUCIÓN =====");
            Console.WriteLine($"Tiempo total de ejecución: {cronometro.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"Ticks: {cronometro.ElapsedTicks}");

            Console.WriteLine("\nPresione una tecla para salir...");
            Console.ReadKey();
        }
    }
}