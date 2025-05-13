using AA.Models;

namespace AA.Services
{
    public class MedicoService
    {
        private List<Medico> medicos;
        private readonly JsonPersistenceService _persistenceService;

        public MedicoService(JsonPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
            medicos = _persistenceService.CargarMedicos();
            
            // Si no hay médicos, crear algunos por defecto
            if (!medicos.Any())
            {
                CrearMedicosPredeterminados();
            }
        }

        private void CrearMedicosPredeterminados()
        {
            var medicosDefault = new List<Medico>
            {
                new Medico
                {
                    Id = 1,
                    Nombre = "Dr. García López",
                    Especialidad = "Medicina General",
                    Email = "garcia@citasmedicas.com",
                    Telefono = "600123456",
                    FechaAlta = DateTime.Now.AddMonths(-6),
                    Disponible = true
                },
                new Medico
                {
                    Id = 2,
                    Nombre = "Dra. Martínez Ruiz",
                    Especialidad = "Cardiologia",
                    Email = "martinez@citasmedicas.com",
                    Telefono = "600789012",
                    FechaAlta = DateTime.Now.AddMonths(-3),
                    Disponible = true
                },
                new Medico
                {
                    Id = 3,
                    Nombre = "Dr. Fernández Santos",
                    Especialidad = "Pediatria",
                    Email = "fernandez@citasmedicas.com",
                    Telefono = "600345678",
                    FechaAlta = DateTime.Now.AddMonths(-1),
                    Disponible = true
                }
            };
            
            medicos.AddRange(medicosDefault);
            GuardarCambios();
        }

        public void AgregarMedico(Medico medico)
        {
            medico.Id = medicos.Count > 0 ? medicos.Max(m => m.Id) + 1 : 1;
            medico.FechaAlta = DateTime.Now;
            medicos.Add(medico);
            GuardarCambios();
        }
     public bool BorrarMedico(int id)
    {
        var medico = medicos.FirstOrDefault(m => m.Id == id);
        if (medico != null)
        {
            medicos.Remove(medico);
            GuardarCambios();
            return true;
        }
        return false;
    }
        

        public List<Medico> ObtenerMedicos()
        {
            return medicos;
        }

        public Medico? ObtenerMedicoPorId(int id)
        {
            return medicos.FirstOrDefault(m => m.Id == id);
        }
        
        public List<Medico> BuscarMedicosPorEspecialidad(string especialidad)
        {
            return medicos.Where(m => m.Especialidad.Contains(especialidad, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        public List<Medico> BuscarMedicosPorNombre(string nombre)
        {
            return medicos.Where(m => m.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        public void ActualizarMedico(Medico medico)
        {
            int index = medicos.FindIndex(m => m.Id == medico.Id);
            if (index != -1)
            {
                medicos[index] = medico;
                GuardarCambios();
            }
        }
        
        private void GuardarCambios()
        {
            _persistenceService.GuardarMedicos(medicos);
        }
    }
}