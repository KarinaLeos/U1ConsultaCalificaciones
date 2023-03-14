using TutoresApp.Models;
using TutoresApp.Views;

namespace TutoresApp;

public partial class App : Application
{
	public static Usuario Usuario { get; set; } = new Usuario();
	public App()
	{
		InitializeComponent();

		MainPage = new InicioView();
       
    }
}
