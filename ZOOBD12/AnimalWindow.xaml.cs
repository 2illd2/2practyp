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

namespace ZOOBD12
{
    public partial class AnimalWindow : Window
    {
        private zoomagEntities3 context = new zoomagEntities3();
        public AnimalWindow()
        {
            InitializeComponent();
            AnimalGrd.ItemsSource = context.Animals.ToList();
            AnimalGrd.SelectionChanged += AnimalGrd_SelectionChanged;
        }
        private void AnimalGrd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AnimalGrd.SelectedItem != null)
            {
                Animals selectedAnimal = (Animals)AnimalGrd.SelectedItem;
                TextBoxName.Text = selectedAnimal.Name;
                TextBoxSpecies.Text = selectedAnimal.Species;
                TextBoxAge.Text = selectedAnimal.Age.ToString();
                TextBoxPrice.Text = selectedAnimal.Price.ToString();
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxName.Text) || string.IsNullOrWhiteSpace(TextBoxSpecies.Text) ||
                string.IsNullOrWhiteSpace(TextBoxAge.Text) || string.IsNullOrWhiteSpace(TextBoxPrice.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все текстовые поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Animals newAnimals = new Animals()
            {
                Name = TextBoxName.Text,
                Species = TextBoxSpecies.Text,
                Age = Convert.ToInt32(TextBoxAge.Text),
                Price = Convert.ToInt32(TextBoxPrice.Text)
            };
            context.Animals.Add(newAnimals);
            context.SaveChanges();
            AnimalGrd.ItemsSource = context.Animals.ToList();
            UpdateAnimalsCustomer();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxName.Text) || string.IsNullOrWhiteSpace(TextBoxSpecies.Text) ||
                string.IsNullOrWhiteSpace(TextBoxAge.Text) || string.IsNullOrWhiteSpace(TextBoxPrice.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все текстовые поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Animals selectedAnimal = (Animals)AnimalGrd.SelectedItem;

            if (selectedAnimal != null)
            {
                selectedAnimal.Name = TextBoxName.Text;
                selectedAnimal.Species = TextBoxSpecies.Text;
                selectedAnimal.Age = Convert.ToInt32(TextBoxAge.Text);
                selectedAnimal.Price = Convert.ToInt32(TextBoxPrice.Text);
                context.SaveChanges();
                AnimalGrd.ItemsSource = context.Animals.ToList();
                UpdateAnimalsCustomer();
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Animals selectedAnimal = (Animals)AnimalGrd.SelectedItem;
            if (selectedAnimal != null)
            {
                RemoveAnimalFromCustomer(selectedAnimal.ID_Animal);
                context.Animals.Remove(selectedAnimal);
                context.SaveChanges();
                AnimalGrd.ItemsSource = context.Animals.ToList();
            }
        }
        private void RemoveAnimalFromCustomer(int animalId)
        {
            var ordersToRemove = context.Orders.Where(o => o.Animal_ID == animalId).ToList();
            foreach (var order in ordersToRemove)
            {
                context.Orders.Remove(order);
            }
            context.SaveChanges();
        }
        private void UpdateAnimalsCustomer()
        {
            foreach (var animalCustomer in context.Orders.ToList())
            {
                if (!context.Animals.Any(a => a.ID_Animal == animalCustomer.Animal_ID) ||
                    !context.Customers.Any(c => c.ID_Customer == animalCustomer.Customer_ID))
                {
                    context.Orders.Remove(animalCustomer);
                }
            }
            foreach (var customer in context.Customers)
            {
                var customerAnimals = context.Orders
                    .Where(ac => ac.Customer_ID == customer.ID_Customer)
                    .Select(ac => ac.Animal_ID)
                    .ToList();
                foreach (var animalId in customerAnimals)
                {
                    if (!context.Orders.Any(ac => ac.Customer_ID == customer.ID_Customer && ac.Animal_ID == animalId))
                    {
                        context.Orders.Add(new Orders()
                        {
                            Customer_ID = customer.ID_Customer,
                            Animal_ID = animalId
                        });
                    }
                }
            }
            context.SaveChanges();
        }
        private void TextBoxAge_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]+$"))
            {
                e.Handled = true;
            }
        }
        private void TextBoxName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                int maxLength = 50;
                if (textBox.Text.Length >= maxLength)
                {
                    e.Handled = true;
                    return;
                }
                foreach (char c in e.Text)
                {
                    if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
        }

        private void TextBoxSpecies_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                int maxLength = 100;
                if (textBox.Text.Length >= maxLength)
                {
                    e.Handled = true;
                }
            }
        }
        private void TextBoxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                int maxLength = 100;
                if (textBox.Text.Length >= maxLength)
                {
                    e.Handled = true;
                }
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
