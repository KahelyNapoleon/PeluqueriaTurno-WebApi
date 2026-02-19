using PracticaDelegado.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaDelegado.Clases
{
    public class Cliente 
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;

        public Cliente() { }
        public Cliente(string name, string surname) { Name = name; Surname = surname; }

        

        

    }
}
