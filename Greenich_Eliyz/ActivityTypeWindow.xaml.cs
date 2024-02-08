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
using Greenich_Eliyz.GreenichService;

namespace Greenich_Eliyz
{
    /// <summary>
    /// Interaction logic for ActivityTypeWindow.xaml
    /// </summary>
    public partial class ActivityTypeWindow : Window
    {
        ServiceProjectClient greenichService;
        ActivityTypeList activityTypes;
        bool add = true;

        public ActivityTypeWindow()
        {
            InitializeComponent();
            greenichService = new ServiceProjectClient();
            activityTypes = greenichService.SelectAllActivityType();
            lbat.ItemsSource = activityTypes;
            lbat.DisplayMemberPath = "Type";

        }

        private void lbat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            add = false;
            ActivityType type = lbat.SelectedItem as ActivityType;
            TypeTxtBox.Text = type.Type;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (add)
            {
                if (activityTypes.Find(it => it.Type.ToLower().Equals(TypeTxtBox.Text.Trim().ToLower())) != null)
                {
                    MessageBox.Show("", "already exist", MessageBoxButton.OK);
                    return;
                }
                ActivityType newType = new ActivityType { Type = TypeTxtBox.Text };
                greenichService.InsertActivityType(newType);
                MessageBox.Show("", "Done", MessageBoxButton.OK);
                lbat.ItemsSource = activityTypes=greenichService.SelectAllActivityType();
                clearBtn_Click(null, null);
                return;
            }
            else
            {
                ActivityType activityType = lbat.SelectedItem as ActivityType;
                activityType.Type = TypeTxtBox.Text;
                greenichService.UpdateActivityType(activityType);
                MessageBox.Show("", "new item", MessageBoxButton.OK);
                return;
            }
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            lbat.IsEnabled = false;
            clearBtn.Visibility = Visibility.Visible;
            add = true;
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            lbat.IsEnabled = true;
            lbat.SelectedIndex = 0;

            clearBtn.Visibility = Visibility.Hidden;
        }
    }
}
