using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfApp9
{
    public partial class EFW : Window
    {
        zoomagEntities context = new zoomagEntities();
        public ObservableCollection<AllDataWithoutIDs> AllDataWithoutIDs { get; set; }
        private List<AllDataWithoutIDs> initialData; // Переменная для хранения изначальных данных

        public EFW()
        {
            InitializeComponent();
            initialData = context.AllDataWithoutIDs.ToList(); // Сохраняем изначальные данные
            AllDataWithoutIDs = new ObservableCollection<AllDataWithoutIDs>(initialData); 
            AllDataWithoutIDsDgr.ItemsSource = AllDataWithoutIDs;
            AllDataWithoutIDsCbx.ItemsSource = context.AllDataWithoutIDs
                        .Where(item => !string.IsNullOrEmpty(item.AnimalSpecies))
                        .Select(item => item.AnimalSpecies)
                        .Distinct()
                        .ToList();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text;
            var query = context.AllDataWithoutIDs.Where(item => item.AnimalName.Contains(searchTerm))
                .ToList();
            AllDataWithoutIDs.Clear();
            foreach (var item in query)
            {
                AllDataWithoutIDs.Add(item);
            }
        }

        private void AllDataWithoutIDsCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedSpecies = AllDataWithoutIDsCbx.SelectedItem as string;

            if (selectedSpecies != null)
            {
                
                AllDataWithoutIDsDgr.ItemsSource = context.AllDataWithoutIDs.Where(item => item.AnimalSpecies == selectedSpecies).ToList();
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
            AllDataWithoutIDsCbx.SelectedIndex = -1;
            AllDataWithoutIDs.Clear();
            foreach (var item in initialData)
            {
                AllDataWithoutIDs.Add(item);
            }
            AllDataWithoutIDsDgr.ItemsSource = AllDataWithoutIDs;
        }
    }
}
