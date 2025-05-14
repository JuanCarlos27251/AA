using AA.Models;

namespace AA.Services
{
    public class CitaService
    {
        private List<Cita> citas;
        private readonly JsonPersistenceService _persistenceService;

        public CitaService(JsonPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
            citas = _persistenceService.CargarCitas();
        }

        public void AgregarCita(Cita cita)
        {
            cita.Id = citas.Count > 0 ? citas.Max(c => c.Id) + 1 : 1;
            citas.Add(cita);
            GuardarCambios();
        }

        public List<Cita> ObtenerCitas()
        {
            return citas;
        }

        public List<Cita> ObtenerCitasPorUsuario(int usuarioId)
        {
            return citas.Where(c => c.IdUsuario == usuarioId).ToList();
        }

        public List<Cita> BuscarCitasPorMotivo(string motivo)
        {
            return citas.Where(c => c.Motivo.Contains(motivo, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        public List<Cita> BuscarCitasPorFecha(DateTime fecha)
        {
            return citas.Where(c => c.FechaCita.Date == fecha.Date).ToList();
        }
        
        public void ActualizarCita(Cita cita)
        {
            int index = citas.FindIndex(c => c.Id == cita.Id);
            if (index != -1)
            {
                citas[index] = cita;
                GuardarCambios();
            }
        }
        
        public bool EliminarCita(int id)
        {
            var cita = citas.FirstOrDefault(c => c.Id == id);
            if (cita != null)
            {
                citas.Remove(cita);
                GuardarCambios();
                return true;
            }
            return false;
        }
        
        
        private void GuardarCambios()
        {
            try
            {
                _persistenceService.GuardarCitas(citas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar los cambios: {ex.Message}");
            }
        }
    }
}