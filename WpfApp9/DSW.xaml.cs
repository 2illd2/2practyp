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
using WpfApp9.zoomagDataSetTableAdapters;

namespace WpfApp9
{
    public partial class DSW : Window
    {
        private zoomagDataSet dataset = new zoomagDataSet();
        public ObservableCollection<zoomagDataSet.AllDataWithoutIDsRow> AllDataWithoutIDs { get; set; }
        AllDataWithoutIDsTableAdapter AllDataWithout = new AllDataWithoutIDsTableAdapter();
        public DSW()
        {
            InitializeComponent();
            LoadData();
            LoadSpeciesComboBox();
        }
        private void LoadData()
        {
            try
            {
                AllDataWithout.Fill(dataset.AllDataWithoutIDs);
                AllDataWithoutIDs = new ObservableCollection<zoomagDataSet.AllDataWithoutIDsRow>(dataset.AllDataWithoutIDs);
                AllDataWithoutIDsDgr.ItemsSource = AllDataWithoutIDs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }
        private void LoadSpeciesComboBox()
        {
            var speciesList = dataset.AllDataWithoutIDs.Select(item => item.AnimalSpecies).Distinct().ToList();
            AllDataWithoutIDsCbx.ItemsSource = speciesList;
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text.Trim();

            var query = dataset.AllDataWithoutIDs
                               .Where(item => item.AnimalName.Contains(searchTerm))
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
                var query = dataset.AllDataWithoutIDs
                                   .Where(item => item.AnimalSpecies == selectedSpecies)
                                   .ToList();
                AllDataWithoutIDs.Clear();
                foreach (var item in query)
                {
                    AllDataWithoutIDs.Add(item);
                }
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            AllDataWithout.Fill(dataset.AllDataWithoutIDs);
            AllDataWithoutIDs.Clear();
            foreach (var item in dataset.AllDataWithoutIDs)
            {
                AllDataWithoutIDs.Add(item);
            }
        }
    }
}
