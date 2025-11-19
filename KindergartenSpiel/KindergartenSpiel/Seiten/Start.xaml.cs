using System.Windows;
using System.Windows.Controls;
using KindergartenSpiel.Models;
using KindergartenSpiel.Seiten;
using System.Windows.Input;
using System.Windows.Navigation;

namespace KindergartenSpiel.Seiten
{
	/// <summary>
	/// Interaktionslogik für Start.xaml
	/// </summary>
	public partial class Start : Page
	{
		public Start()
		{
            InitializeComponent();
		}

		private void Play_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Seiten/GamePage.xaml", UriKind.Relative));
        }


    }
}
