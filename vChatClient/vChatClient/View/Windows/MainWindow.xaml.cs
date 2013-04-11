using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using vChat.View.Controls;
using System.IO;
using Elysium;
using vChatClient;

namespace vChat.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Elysium.Controls.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_OnLoginSuccess(string userLogged)
        {
            MessageBox.Show("ok");
        }

        private void Login_OnLoginFailed()
        {
            MessageBox.Show("fail");
        }

        private void Login_OnSignUpClicked()
        {
            Grid.Children.Remove(LoginUC);
            SignUp signUpUC = new SignUp();
            signUpUC.OnSignUpSuccess += new SignUp.SignUpSuccessHandler(delegate
            {
                Grid.Children.Remove(signUpUC);
                Grid.Children.Add(LoginUC);
            });
            Grid.Children.Add(signUpUC);
        }

        private static readonly string Windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        private static readonly string SegoeUI = Windows + @"\Fonts\SegoeUI.ttf";
        private static readonly string Verdana = Windows + @"\Fonts\Verdana.ttf";

        private void ThemeGlyphInitialized(object sender, EventArgs e)
        {
            ThemeGlyph.FontUri = new Uri(File.Exists(SegoeUI) ? SegoeUI : Verdana);
        }

        private void AccentGlyphInitialized(object sender, EventArgs e)
        {
            AccentGlyph.FontUri = new Uri(File.Exists(SegoeUI) ? SegoeUI : Verdana);
        }

        private void ContrastGlyphInitialized(object sender, EventArgs e)
        {
            ContrastGlyph.FontUri = new Uri(File.Exists(SegoeUI) ? SegoeUI : Verdana);
        }

        private void LightClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(Theme.Light);
        }

        private void DarkClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(Theme.Dark);
        }

        private void AccentClick(object sender, RoutedEventArgs e)
        {
            var item = e.Source as MenuItem;
            if (item != null)
            {
                var accentBrush = (SolidColorBrush)((Rectangle)item.Icon).Fill;
                Application.Current.Apply(accentBrush, null);
            }
        }

        private void WhiteClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(null, Brushes.White);
        }

        private void BlackClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(null, Brushes.Black);
        }
    }
}
