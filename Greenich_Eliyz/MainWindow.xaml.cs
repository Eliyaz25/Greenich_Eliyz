using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using Greenich_Eliyz.GreenichService;


namespace Greenich_Eliyz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServiceProjectClient greenichService;
        private User user;
        private bool pass;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = user=new User();
            greenichService = new ServiceProjectClient();            
        }

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();


        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if(IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme=true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void signupBtn_Click(object sender, RoutedEventArgs e)
        {
            SignupWindow signup = new SignupWindow();
            signup.Show();
            this.Close();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidPassword valid = new ValidPassword();
            ValidationResult result = valid.Validate(txtPassword.Password, null);
            if (result.IsValid)
            {
                pass = true;
                txtPassword.BorderBrush = Brushes.LightGray;

            }
            else
            {
                pass = false;
                txtPassword.BorderBrush = Brushes.Red;

            }
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == null || txtPassword.Password == null)
            {
                MessageBox.Show("Youre not good", "NOT OKAY", MessageBoxButton.OK);
                return;
            }
            if (!Validation.GetHasError(txtUsername) && pass)
            {
                user.Username = txtUsername.Text;
                user.Password = txtPassword.Password;
                User temp= greenichService.Login(user);
                if(temp==null)
                {
                    MessageBox.Show("User info inccurcert", "login", MessageBoxButton.OK);
                    return;
                }
                //UserWindow userWindow = new UserWindow(user);
                //userWindow.Show();
                ActivityWindow activityWindow = new ActivityWindow(user);
                activityWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Youre not good", "NOT OKAY", MessageBoxButton.OK);
                return;
            }

        }
    }
}
