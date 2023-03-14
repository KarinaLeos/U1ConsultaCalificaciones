using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutoresApp.Models;
using TutoresApp.Services;
using TutoresApp.Views;

namespace TutoresApp.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        Usuario Usuario { get; set; }

        UsuarioService usuarioService;
        public Command IniciarSesionCommand { get; set; }
        public LoginViewModel() 
        {
            IniciarSesionCommand = new Command(IniciarSesion);

            Usuario = new ();
            usuarioService = new UsuarioService();
            usuarioService.Error += UsuarioService_Error;
        }

        [Obsolete]
        private async void UsuarioService_Error(string error)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await App.Current.MainPage.DisplayAlert("Error", error, "Ok");
            });
        }

       
        private async void IniciarSesion()
        {
            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                  
                    Usuario = await usuarioService.GetUsuario(NombreUsuario, Password);
                    if (Usuario != null)
                    {
                        App.Usuario = Usuario;
                        AlumnoViewModel alumnoViewModel = new AlumnoViewModel(Usuario);
                        ListaAlumnosView listaAlumnosView = new ListaAlumnosView() { BindingContext = alumnoViewModel};
                        App.Current.MainPage = new NavigationPage(listaAlumnosView);
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No hay conexion a internet.", "Ok");
                }    
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");

            }
        }
        private void Actualizar(string v = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
