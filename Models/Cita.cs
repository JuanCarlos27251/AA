using System;

namespace AA.Models
{
    public class Cita
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }    // Relación con Usuario
        public int IdMedico { get; set; }     // Relación con Medico
        public DateTime FechaCita { get; set; }
        public string Motivo { get; set; }
        public bool Confirmada { get; set; }
        

        public string? NombrePaciente { get; set; }   // Se asigna al mostrar la cita
        public string? NombreMedico { get; set; }     // Se asigna al mostrar la cita
    }
}
