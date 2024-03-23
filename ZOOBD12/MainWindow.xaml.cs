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
using static ZOOBD12.zoomagDataSet;

namespace ZOOBD12
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Animals_Click(object sender, RoutedEventArgs e)
        {
            AnimalWindow window1 = new AnimalWindow();
            window1.ShowDialog();
            if (window1.DialogResult == false)
            {
                window1.Close();
            }
        }
        private void Customers_Click(object sender, RoutedEventArgs e)
        {
            CustomersWindow window2 = new CustomersWindow();
            window2.ShowDialog();
            if (window2.DialogResult == false)
            {
                window2.Close();
            }
        }
        private void AnimalsCustomers_Click(object sender, RoutedEventArgs e)
        {
            AnimalsCustomersWindow window2 = new AnimalsCustomersWindow();
            window2.ShowDialog();
            if (window2.DialogResult == false)
            {
                window2.Close();
            }
        }
    }
}
