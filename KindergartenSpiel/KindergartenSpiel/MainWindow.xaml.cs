using KindergartenSpiel.Models;
using KindergartenSpiel.Seiten;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KindergartenSpiel
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Game m_oGame;
		public MainWindow()
		{
			InitializeComponent();
			var fp = new Start();
			NavFrame.Navigate(fp);
		}

		public void SetPage(Page nextPage)
		{
			NavFrame.Navigate(nextPage);
		}

		private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Minimize_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void Maximize_Click(object sender, RoutedEventArgs e)
		{
			if (this.WindowState == WindowState.Maximized)
				this.WindowState = WindowState.Normal;
			else
				this.WindowState = WindowState.Maximized;
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			if (this.WindowState == WindowState.Maximized)
				((Border)this.Content).CornerRadius = new CornerRadius(0);
			else
				((Border)this.Content).CornerRadius = new CornerRadius(20);
		}

		private void Maximieren_Click(object sender, RoutedEventArgs e)
		{

		}

		private void PlayButtonEvent(object oSender, EventArgs oArgs)
		{

		}
	}
}