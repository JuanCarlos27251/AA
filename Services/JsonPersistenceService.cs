using System.Text.Json;
using AA.Models;

namespace AA.Services
{
    public class JsonPersistenceService
    {
        private readonly string _dataDirectory;
        
        private readonly string _usuariosFile;
        private readonly string _medicosFile;
        private readonly string _citasFile;
        
        public JsonPersistenceService(string dataDirectory = "Data")
        {
            _dataDirectory = dataDirectory;
            
            // Asegurar que el directorio de datos exista
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
            
            _usuariosFile = Path.Combine(_dataDirectory, "usuarios.json");
            _medicosFile = Path.Combine(_dataDirectory, "medicos.json");
            _citasFile = Path.Combine(_dataDirectory, "citas.json");
        }
        
        // Métodos para Usuarios
        public void GuardarUsuarios(List<Usuario> usuarios)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(usuarios, options);
            File.WriteAllText(_usuariosFile, jsonString);
        }
        
        public List<Usuario> CargarUsuarios()
        {
            if (!File.Exists(_usuariosFile))
            {
                return new List<Usuario>();
            }
            
            string jsonString = File.ReadAllText(_usuariosFile);
            return JsonSerializer.Deserialize<List<Usuario>>(jsonString) ?? new List<Usuario>();
        }
        
        // Métodos para Médicos
        public void GuardarMedicos(List<Medico> medicos)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(medicos, options);
            File.WriteAllText(_medicosFile, jsonString);
        }
        
        public List<Medico> CargarMedicos()
        {
            if (!File.Exists(_medicosFile))
            {
                return new List<Medico>();
            }
            
            string jsonString = File.ReadAllText(_medicosFile);
            return JsonSerializer.Deserialize<List<Medico>>(jsonString) ?? new List<Medico>();
        }
        
        // Métodos para Citas
        public void GuardarCitas(List<Cita> citas)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(citas, options);
            File.WriteAllText(_citasFile, jsonString);
        }
        
        public List<Cita> CargarCitas()
        {
            if (!File.Exists(_citasFile))
            {
                return new List<Cita>();
            }
            
            string jsonString = File.ReadAllText(_citasFile);
            return JsonSerializer.Deserialize<List<Cita>>(jsonString) ?? new List<Cita>();
        }
    }
}