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
            // Detectar si estamos en Docker
        bool isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
        
        // Establecer el directorio de datos según el entorno
        _dataDirectory = isDocker 
            ? "/app/data"  // Ruta en Docker
            : Path.Combine(Directory.GetCurrentDirectory(), dataDirectory); // Ruta local
        
        Console.WriteLine($"Usando directorio de datos: {_dataDirectory}");
        
        if (!Directory.Exists(_dataDirectory))
        {
            Directory.CreateDirectory(_dataDirectory);
            Console.WriteLine($"Directorio creado: {_dataDirectory}");
        }
            
            _usuariosFile = Path.Combine(_dataDirectory, "usuarios.json");
            _medicosFile = Path.Combine(_dataDirectory, "medicos.json");
            _citasFile = Path.Combine(_dataDirectory, "citas.json");
        }
        
        // Métodos para Usuarios
        public void GuardarUsuarios(List<Usuario> usuarios)
        {
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true
                };
                
                string jsonString = JsonSerializer.Serialize(usuarios, options);
                File.WriteAllText(_usuariosFile, jsonString);
                Console.WriteLine($"Usuarios guardados correctamente en: {_usuariosFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar usuarios en JSON: {ex.Message}");
                throw;
            }
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
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true
                };
                string jsonString = JsonSerializer.Serialize(medicos, options);
                File.WriteAllText(_medicosFile, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar médicos: {ex.Message}");
                throw;
            }
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
            try{
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true
                };
                string jsonString = JsonSerializer.Serialize(citas, options);
                File.WriteAllText(_citasFile, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar citas: {ex.Message}");
                throw;
            }
        
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