using TutoresApp.ViewModel;

namespace TutoresApp.Views;

public partial class ListaAlumnosView : ContentPage
{
	public ListaAlumnosView()
	{
		InitializeComponent();
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		var vm = (AlumnoViewModel)this.BindingContext;
		var alumno = ((Border)sender).BindingContext;
		vm.VerCalificacionesCommand.Execute(alumno);
    }
}