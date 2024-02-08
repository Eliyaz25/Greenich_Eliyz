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
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using Greenich_Eliyz.GreenichService;

namespace Greenich_Eliyz
{
    /// <summary>
    /// Interaction logic for SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : Window
    {
        private ServiceProjectClient greenichService;
        private User user;
        private bool pass;

        public SignupWindow()
        {
            InitializeComponent();
            DataContext = user = new User();
            greenichService = new ServiceProjectClient();
            cmbCity.ItemsSource = greenichService.SelectAllCity();
            cmbCity.DisplayMemberPath = "Name";
            Birthday.DisplayDateStart = DateTime.Today.AddYears(-120);
            Birthday.DisplayDateEnd = DateTime.Today.AddYears(-18);
        }


        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
       
        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
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

        static int CalculateAge(DateTime birthdate)
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - birthdate.Year;
            if (birthdate > currentDate.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        static bool CheckAge(string b)
        {
            try
            {
                DateTime birthday = DateTime.Parse(b);
                if (CalculateAge(birthday) < 12)
                {
                    MessageBox.Show("too young", "NOT OKAY", MessageBoxButton.OK);
                    return false;
                }
                if (CalculateAge(birthday) > 100)
                {
                    MessageBox.Show("too old", "NOT OKAY", MessageBoxButton.OK);
                    return false;
                }
            }catch (Exception ex) { return false; }
            return true;
        }
        private void signupBtn_Click(object sender, RoutedEventArgs e)
        {

            if((txtFirstName.Text == null || txtLastName.Text == null || txtUsername.Text == null || 
                txtPassword.Password == null || txtEmail.Text == null || Birthday.Text==null || 
                Gender.Text == null || cmbCity.Text == null))
            {
                MessageBox.Show("Youre not good", "NOT OKAY", MessageBoxButton.OK);
                return ;
            }
            if (!Validation.GetHasError(txtFirstName) && !Validation.GetHasError(txtLastName) && !Validation.GetHasError(txtPassword) && pass && !Validation.GetHasError(txtEmail) && CheckAge(Birthday.Text))
            {
                greenichService = new ServiceProjectClient();
                if (!greenichService.IsUsernameFree(txtUsername.Text))
                {
                    MessageBox.Show("Username is not avilable", "NOT OKAY", MessageBoxButton.OK);
                    return;
                }
                user.Password = txtPassword.Password;
                user.Birthday = Birthday.SelectedDate.Value;
                if (Gender.Text == "Male")
                    user.Gender = true;
                else
                    user.Gender = false;
                user.IsVolunteer = true;
               if(greenichService.InsertUser(user)==1)
               {
                    user=greenichService.Login(user);
                    MessageBox.Show("Success!", "sign up", MessageBoxButton.OK);
                    //UserWindow userWindow = new UserWindow(user);
                    //userWindow.Show();
                    this.Close();
               }
                else
                {
                    MessageBox.Show("Youre not good", "NOT OKAY", MessageBoxButton.OK);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Fix the errors!", "NOT OK", MessageBoxButton.OK);
            }
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidPassword valid = new ValidPassword();
            ValidationResult result = valid.Validate(txtPassword.Password, null);
            if(result.IsValid)
            {
                pass = true;
                txtPassword.BorderBrush = Brushes.LightGray;
                
            }
            else
            {
                pass = false;
                txtPassword.BorderBrush= Brushes.Red;
                
            }
        }
    }
}
