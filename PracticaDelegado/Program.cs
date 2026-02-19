using PracticaDelegado.Clases;
using PracticaDelegado.Interfaces;
using System;

namespace PracticaDelegados;


public delegate string MostrarDatos(string dato1, string dato2);

public class Program
{
    public static string DatosCliente(string d1, string d2) => "Nombre:" + d1 + "Apellido:" + d2;
    public static void Main()
    {
        Cliente cliente = new Cliente("Gabolos","Aguirre");

        MostrarDatos mostrarDatos = DatosCliente;

        var nombreExtendsToken = (54).ToString("J35ks6");

        Console.WriteLine(nombreExtendsToken);
        
    }
}


