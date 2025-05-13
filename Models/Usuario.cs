using System;

namespace AA.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Rol { get; set; } // "Paciente" o "Admin"
        public bool EstaActivo { get; set; } = true;
    }
}



