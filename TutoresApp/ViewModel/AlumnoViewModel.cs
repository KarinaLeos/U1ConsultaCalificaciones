using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TutoresApp.Models;
using TutoresApp.Services;
using TutoresApp.Views;

namespace TutoresApp.ViewModel
{
    public class AlumnoViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<AlumnoDTO> ListaAlumnos { get; set; } = new ObservableCollection<AlumnoDTO>();
        public ObservableCollection<CalificacionDTO> ListaDeCalifiaciones { get; set; } = new ObservableCollection<CalificacionDTO>();

        public string Errores { get; set; }
        private AlumnosService AlumnosService { get; set; }
        public AlumnoDTO alumnoDTO;

        //Comandos
        public ICommand IniciarCommand { get; set; }
        public ICommand VerListaAlumnos { get; set; }
        public ICommand VerCalificacionesCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public Usuario Usuario { get; set; }
        public AlumnoViewModel(Usuario user)
        {
            Usuario = user;
            VerListaAlumnos = new Command(CargarAlumnos);
            AlumnosService = new AlumnosService();
            IniciarCommand = new Command(Iniciar);
            CancelarCommand = new Command(Cancelar);
            VerCalificacionesCommand = new Command<AlumnoDTO>(VerCalifiacionesAsync);
            Iniciar();
        }

        private void Cancelar(object obj)
        {
            throw new NotImplementedException();
        }

        private void Iniciar()
        {
            CargarAlumnos();
            Actualizar(nameof(ListaAlumnos));
        }

        private void Actualizar(string nombre)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        private async void CargarAlumnos()
        {
            var datos = await AlumnosService.GetAlumnos(Usuario.Id);
            datos.ForEach(x => ListaAlumnos.Add(x));
        }
        CalificacionesView calificacionesView;
        private async void VerCalifiacionesAsync(AlumnoDTO alumno)
        {
            alumnoDTO = new AlumnoDTO();
            alumnoDTO = alumno;
            Actualizar(nameof(alumnoDTO));
            ListaDeCalifiaciones = new ObservableCollection<CalificacionDTO>();
            alumno.Calificaciones.ForEach(x => ListaDeCalifiaciones.Add(x));
            Actualizar(nameof(ListaDeCalifiaciones));
            calificacionesView = new CalificacionesView() { BindingContext = this };
            Application.Current.MainPage.Navigation.PushAsync(calificacionesView);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
