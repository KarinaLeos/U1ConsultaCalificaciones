using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutoresApp.Models;

namespace TutoresApp.Services
{
    public class AlumnosService
    {
        HttpClient alumno = new HttpClient
        {
            BaseAddress = new Uri("https://padres.sistemas19.com/")
        };

        public event Action<string> Error;

        void LanzarError(string mensaje) 
        {
            Error?.Invoke(mensaje);

        }
        void LanzarErrorJson(string json) 
        {
            string obj = JsonConvert.DeserializeObject<string>(json);
            if (obj != null)
            {
                Error?.Invoke(obj);
            }
        }
        public async Task<List<AlumnoDTO>> GetAlumnos(int id) 
        {
            List<AlumnoDTO> listAlumnos = new List<AlumnoDTO>();
            var response = await alumno.GetAsync("api/alumno/" + id);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                listAlumnos = JsonConvert.DeserializeObject<List<AlumnoDTO>>(json);
            }
            if (listAlumnos != null)
            {
                return listAlumnos;
            }
            else 
            {
                return new List<AlumnoDTO>();
            }
        }
    }
}
