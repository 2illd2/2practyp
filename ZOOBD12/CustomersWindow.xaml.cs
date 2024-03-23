using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZOOBD12
{
    public partial class CustomersWindow : Window
    {
        private zoomagEntities3 context = new zoomagEntities3();
        public CustomersWindow()
        {
            InitializeComponent();
            CustomerGrd.ItemsSource = context.Customers.ToList();
            CustomerGrd.SelectionChanged += CustomerGrd_SelectionChanged;
        }
            private void CustomerGrd_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (CustomerGrd.SelectedItem != null)
                {
                Customers selectedCustomers = (Customers)CustomerGrd.SelectedItem;
                TextBoxCustomerName.Text = selectedCustomers.Name;
                TextBoxCustomerEmail.Text = selectedCustomers.Email;
                TextBoxCustomerPhone.Text = selectedCustomers.Phone.ToString();

                }
            }
            private void AddButton_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrWhiteSpace(TextBoxCustomerName.Text) || string.IsNullOrWhiteSpace(TextBoxCustomerEmail.Text) ||
                    string.IsNullOrWhiteSpace(TextBoxCustomerPhone.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все текстовые поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            Customers newCustomers = new Customers()
                {
                    Name = TextBoxCustomerName.Text,
                    Email = TextBoxCustomerEmail.Text,
                    Phone= Convert.ToString(TextBoxCustomerPhone.Text)
                };
                context.Customers.Add(newCustomers);
                context.SaveChanges();
                CustomerGrd.ItemsSource = context.Customers.ToList();
                UpdateAnimalsCustomer();
            }
            private void UpdateButton_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrWhiteSpace(TextBoxCustomerName.Text) || string.IsNullOrWhiteSpace(TextBoxCustomerEmail.Text) ||
                    string.IsNullOrWhiteSpace(TextBoxCustomerPhone.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все текстовые поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            Customers selectedCustomers = (Customers)CustomerGrd.SelectedItem;

                if (selectedCustomers != null)
                {
                selectedCustomers.Name = TextBoxCustomerName.Text;
                selectedCustomers.Email = TextBoxCustomerEmail.Text;
                selectedCustomers.Phone = TextBoxCustomerPhone.Text;

                    context.SaveChanges();

                CustomerGrd.ItemsSource = context.Customers.ToList();

                    UpdateAnimalsCustomer();
                }
            }
            private void DeleteButton_Click(object sender, RoutedEventArgs e)
            {
            Customers selectedCustomers = (Customers)CustomerGrd.SelectedItem;

                if (selectedCustomers != null)
                {
                    RemoveAnimalFromCustomer(selectedCustomers.ID_Customer);

                    context.Customers.Remove(selectedCustomers);
                    context.SaveChanges();

                CustomerGrd.ItemsSource = context.Customers.ToList();
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
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
