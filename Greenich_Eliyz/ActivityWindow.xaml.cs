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
    /// Interaction logic for ActivityWindow.xaml
    /// </summary>
    public partial class ActivityWindow : Window
    {
        private ServiceProjectClient greenichService;
        private Activity activity;
        private User manager;
        public ActivityWindow(User user)
        {
            InitializeComponent();
            DataContext = activity = new Activity();
            greenichService = new ServiceProjectClient();
            cmbCity.ItemsSource = greenichService.SelectAllCity();
            cmbCity.DisplayMemberPath = "Name";
            UserList operators = greenichService.OperatorsList();
            cmbOperator.ItemsSource = operators;
            cmbOperator.DisplayMemberPath = "FirstName";
            cmbType.ItemsSource = greenichService.SelectAllActivityType();
            cmbType.DisplayMemberPath= "Type";
            dateActivity.DisplayDateStart = DateTime.Today.AddYears(1);
            dateActivity.DisplayDateEnd = DateTime.Today.AddYears(40);
            manager = user;
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

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if(txtActivityName == null || cmbOperator == null || dateActivity == null || startTime==null || endTime == null || cmbCity == null || cmbType == null)
            {
                MessageBox.Show("fill all details", "NOT OKAY", MessageBoxButton.OK);
                return;
            }
            activity.ManagerName = manager.FirstName;
            activity.NumVolunnteers = 0;
            activity.Type = cmbType.SelectedValue as ActivityType;
            greenichService.InsertActivity(activity);
            MessageBox.Show("Success!", "", MessageBoxButton.OK);
        }
    }
}
