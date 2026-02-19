using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace MyApp
{
    public delegate int Calcular(int x, int y);
    public delegate string EnviarMensaje(string mensaje);
    public delegate void Saludos(string s);

    public class Calculadora
    {
        public static int Sumar(int x, int y) { return x + y; }
        public static int Restar(int x, int y) { return x - y; }
        public static int Multiplicar(int x, int y) { return x * y; }
        public static int Dividir(int x, int y)
        {
            if (y == 0) throw new DivideByZeroException();
            return x / y;
        }
    }

    public class EnviarNotificacion
    {
        public static string Mensaje(string msj)
        {
            return msj;
        }
    }

    public class TestClass
    {
        public static void Hola(string nombre)
        {
            Console.WriteLine("Hola! {0}", nombre);
        }

        public static void Adios(string nombre)
        {
            Console.WriteLine("Adios! {0}", nombre);
        }
    }

    public class Animal { }
    public class Dog : Animal { }

    public delegate Animal AnimalDelegate(Dog dog);

    //Covarianza de Delgados fuertemente tipados
    public class Person { }
    public class Employee : Person { }

    
    //covarianza en tipos genericos
    interface ICovariant<out T> { T Get(); }

    public class ExampleCovariant : ICovariant<Dog>
    {
        public Dog Get() => new Dog();
    }

    //CONTRAVARIANZA DE TIPOS GENERICOS
    interface IContravariant<in T> { void Set(T value); }

    public class ContravariantExample : IContravariant<Animal>
    {
        public void Set(Animal value) {}
    }
   

    public class Program
    {

        public static Animal GetAnimal(Dog dog) => new Animal();
        public static Dog GetDog(Dog dog) => new Dog();
        public static Animal GetAnimalFromAnimal (Animal animal) => new Animal();
        public static void Main()
        {
            Calcular sumar = new Calcular(Calculadora.Sumar);
            EnviarMensaje mensaje = new EnviarMensaje(EnviarNotificacion.Mensaje);

            //CONVERSION DE GRUPO DE METODOS -> Sin la palabra clave new para instancias el delegado
            Calcular restar = Calculadora.Restar;
            //CREAR INSTANCIA DE DELEGADO CON METODO ANONIMO con la palabra clave delegate adelante.
            Calcular metodoAnonimoDeDelegado = delegate(int x, int y){ return x * x * y; };
            //CREAR INSTANCIA DE DELEGADO CON EPRESION LAMBDA -> de un lado los parametros y del otro la expresion
            Calcular metodoQueCalculaAlgo = ( x,  y) => { return x + y + y; } ;
            //DELEGADOS DE MULTIDIFUSION -> Se pueden anidar delegados en variables de delegados
            //y quitar como una resta instancias de delegados almacenados en una variable de delegado
            Saludos hola, adios, multiSaludar, multiMenosSaludar;

            hola = TestClass.Hola;
            adios = TestClass.Adios;
            multiSaludar = hola + adios;
            multiMenosSaludar = (multiSaludar - hola)!;

            Console.WriteLine("Delegado hola");
            hola("Gabriel");
            Console.WriteLine("Delegado adios");
            adios("Gabriel");
            Console.WriteLine("Delegado multiSaludar");
            multiSaludar("Gabriel");
            Console.WriteLine("Delegado MultiMenosSaludar");
            multiMenosSaludar("Gabriel");

            //Covarianza de delegados
            AnimalDelegate del;

            //Tipo de variable coincide ocn la firma del delegado
            del = GetAnimal;
            Animal animal = GetAnimal(new Dog());

            //Covarianza el valor de retorno es de menor derivacion que el tipo de retorno del delegado
            del = GetDog;
            animal = GetDog(new Dog());

            //Contravarianza el tipo del parametro es de mayor derivacion al tipo de parametro del delegado
            del = GetAnimalFromAnimal;
            animal = GetAnimalFromAnimal(new Dog());

            //Covarianza en Delegados fuertemente tipados como Func<T,N>
            Employee FindEmployee(string title) => new Employee();

            Func<string, Person> func = FindEmployee;
            func("Manager"); //Aqui el metodo encapsulado retorna un tipo Employee pero el delegado Func retorna un tipo person
                             //por ende se permite ya que Employee es mas derivado que Person (deriva de este por eso mas derivado


            //CONTRAVARIANZA DE METODO FUERTEMENTE TIPADO
            void HandleAnimal(Animal animal) { };

            Action<Dog> action = HandleAnimal;
            action(new Dog()); //en este ejemplo el metodo de delegado acepta como parametro un tipo Animal
            // que es menos derivado que Dog por lo que es permitido que el parametro sea de un tipo derivado
            //de este.

            //COVARIANZA EN TIPO GENERICO
            
            ICovariant<Animal> covariant = new ExampleCovariant();
            Animal animal2 = covariant.Get();

            //CONTRAVARIANZA DE TIPO GENERICO
            IContravariant<Dog> contravariant = new ContravariantExample();
            contravariant.Set(new Dog());




            Console.WriteLine();

            //OutPuts
            Console.WriteLine("Metodo de delegado que suma {0}",sumar(5,6));
            Console.WriteLine("Metodo de delegado que calcula algo {0}",metodoQueCalculaAlgo(4,7));

            string respuesta = mensaje("Hola como andas envio este mensaje por medio de un metodo que referencia un delegado");
            Console.WriteLine(respuesta);
        }
    }
}
