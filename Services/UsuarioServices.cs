using AA.Models;

namespace AA.Services
{
    public class UsuarioService
    {
        private List<Usuario> usuarios;
        private readonly JsonPersistenceService _persistenceService;

        public UsuarioService(JsonPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
            usuarios = _persistenceService.CargarUsuarios();
            
            // Si no hay usuarios, crear algunos por defecto
            if (!usuarios.Any())
            {
                CrearUsuariosPredeterminados();
            }
        }

        private void CrearUsuariosPredeterminados()
        {
            var usuariosDefault = new List<Usuario>
            {
                new Usuario
                {
                    Id = 1,
                    Nombre = "admin",
                    Email = "admin@citasmedicas.com",
                    Contrasena = "admin123",
                    FechaRegistro = DateTime.Now,
                    Rol = "Admin",
                    EstaActivo = true
                },
                new Usuario
                {
                    Id = 2,
                    Nombre = "Pepe",
                    Email = "paciente1@gmail.com",
                    Contrasena = "123456",
                    FechaRegistro = DateTime.Now,
                    Rol = "Paciente",
                    EstaActivo = true
                },
                new Usuario
                {
                    Id = 3,
                    Nombre = "Maria",
                    Email = "paciente2@gmail.com",
                    Contrasena = "123456",
                    FechaRegistro = DateTime.Now,
                    Rol = "Paciente",
                    EstaActivo = true
                }
            };
            
            usuarios.AddRange(usuariosDefault);
            GuardarCambios();
        }

        public void AgregarUsuario(Usuario usuario)
        {
            usuario.Id = usuarios.Count > 0 ? usuarios.Max(u => u.Id) + 1 : 1;
            usuario.FechaRegistro = DateTime.Now;
            usuarios.Add(usuario);
            GuardarCambios();
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return usuarios;
        }

        public Usuario? ObtenerUsuarioPorCredenciales(string nombre, string contrasena)
        {
            return usuarios.FirstOrDefault(u => u.Nombre == nombre && u.Contrasena == contrasena && u.EstaActivo);
        }
        
        public Usuario? ObtenerUsuarioPorId(int id)
        {
            return usuarios.FirstOrDefault(u => u.Id == id);
        }
        
        public List<Usuario> BuscarUsuariosPorNombre(string nombre)
        {
            return usuarios.Where(u => u.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        public void ActualizarUsuario(Usuario usuario)
        {
            int index = usuarios.FindIndex(u => u.Id == usuario.Id);
            if (index != -1)
            {
                usuarios[index] = usuario;
                GuardarCambios();
            }
        }
        
        
        private void GuardarCambios()
        {
            try
            {
                _persistenceService.GuardarUsuarios(usuarios);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar los cambios: {ex.Message}");
            }
        }
    }
}