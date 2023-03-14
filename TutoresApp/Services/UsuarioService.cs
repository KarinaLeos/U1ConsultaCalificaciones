using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using TutoresApp.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TutoresApp.Services
{
   public  class UsuarioService
    {
        HttpClient cliente = new HttpClient();
        public UsuarioService()
        {
            cliente.BaseAddress = new Uri("https://padres.sistemas19.com/");
        }
        public event Action<string> Error;

        public async Task<Usuario>GetUsuario(string user, string password)
        {
            if (string.IsNullOrWhiteSpace(user) && string.IsNullOrWhiteSpace(password))
            {
                Error?.Invoke("Nombre de usuario o contrasena incorrecto." + "Intente de nuevo");
            }
            Usuario usuario = new Usuario()
            {
                NombreUsuario = user,
                Password = password
            };
            var json = JsonConvert.SerializeObject(usuario);
            var response = cliente.PostAsync("api/usuario/login", new StringContent(json, Encoding.UTF8, "application/json"));
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                json = await response.Result.Content.ReadAsStringAsync();
                usuario= JsonConvert.DeserializeObject<Usuario>(json);
                return usuario;
            }
            else
            {
                var errores = await response.Result.Content.ReadAsStringAsync();
                Error?.Invoke(errores);
                return null;
            }

        }

    }
}

