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
    public partial class AnimalsCustomersWindow : Window
    {
        private zoomagEntities3 context = new zoomagEntities3();

        public AnimalsCustomersWindow()
        {
            InitializeComponent();
            // Загружаем данные из базы данных при открытии окна
            RefreshDataGrid();
        }

        // Метод для загрузки данных в DataGrid
        private void RefreshDataGrid()
        {
            // Загружаем данные из таблицы AllDataWithoutIDs
            var allData = context.ALLZOO.ToList();
            AnimalsCustomersGrd.ItemsSource = allData;
        }

        // Метод для добавления новой записи
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, заполнены ли все необходимые поля
            if (string.IsNullOrWhiteSpace(TextBoxName.Text) ||
                string.IsNullOrWhiteSpace(TextBoxSpecies.Text) ||
                string.IsNullOrWhiteSpace(TextBoxAge.Text) ||
                string.IsNullOrWhiteSpace(TextBoxPrice.Text) ||
                string.IsNullOrWhiteSpace(TextBoxCustomerName.Text) ||
                string.IsNullOrWhiteSpace(TextBoxCustomerEmail.Text) ||
                string.IsNullOrWhiteSpace(TextBoxCustomerPhone.Text) ||
                DatePickerOrderDate.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все текстовые поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Преобразуем текст из текстовых полей в нужные типы данных
            if (!int.TryParse(TextBoxAge.Text, out int animalAge) ||
                !decimal.TryParse(TextBoxPrice.Text, out decimal animalPrice) ||
                !int.TryParse(TextBoxCustomerPhone.Text, out int customerPhone) ||
                !int.TryParse(TextBoxAge.Text, out int orderQuantity))
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newALLZOO = new ALLZOO
            {
                OrderQuantity = orderQuantity,
                OrderDate = DatePickerOrderDate.SelectedDate.Value,
                // Здесь предполагается, что ID_Customer и ID_Animal будут установлены автоматически при вставке новых записей
                AnimalName = TextBoxName.Text,
                AnimalSpecies = TextBoxSpecies.Text,
                AnimalAge = animalAge,
                AnimalPrice = animalPrice,
                CustomerName = TextBoxCustomerName.Text,
                CustomerEmail = TextBoxCustomerEmail.Text,
                CustomerPhone = TextBoxCustomerPhone.Text
            };

            // Добавляем новые записи в контекст данных
            context.ALLZOO.Add(newALLZOO);

            // Сохраняем изменения в базе данных
            context.SaveChanges();

            // Обновляем данные в DataGrid
            RefreshDataGrid();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxName.Text) || string.IsNullOrWhiteSpace(TextBoxSpecies.Text) ||
                string.IsNullOrWhiteSpace(TextBoxAge.Text) || string.IsNullOrWhiteSpace(TextBoxPrice.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все текстовые поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Получаем выбранный объект из DataGrid
            ALLZOO selectedData = (ALLZOO)AnimalsCustomersGrd.SelectedItem;

            if (selectedData != null)
            {
                // Обновляем свойства объекта, используя данные из текстовых полей
                selectedData.AnimalName = TextBoxName.Text;
                selectedData.AnimalSpecies = TextBoxSpecies.Text;
                selectedData.AnimalAge = Convert.ToInt32(TextBoxAge.Text);
                selectedData.AnimalPrice = Convert.ToInt32(TextBoxPrice.Text);

                context.SaveChanges();

                AnimalsCustomersGrd.ItemsSource = context.ALLZOO.ToList();

                UpdateAnimalsCustomer();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный объект из DataGrid
            ALLZOO selectedData = (ALLZOO)AnimalsCustomersGrd.SelectedItem;

            if (selectedData != null)
            {
                // Удаляем выбранный объект из контекста данных
                context.ALLZOO.Remove(selectedData);
                context.SaveChanges();

                AnimalsCustomersGrd.ItemsSource = context.ALLZOO.ToList();
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
            // Очищаем старые связи
            var allOrders = context.Orders.ToList();
            foreach (var order in allOrders)
            {
                context.Orders.Remove(order);
            }

            // Добавляем новые связи
            var allData = context.ALLZOO.ToList();
            foreach (var data in allData)
            {
                // Проверяем, существует ли клиент в базе данных
                var customer = context.Customers.FirstOrDefault(c => c.Name == data.CustomerName && c.Email == data.CustomerEmail && c.Phone == data.CustomerPhone);
                if (customer == null)
                {
                    // Если клиента нет, создаем нового
                    customer = new Customers
                    {
                        Name = data.CustomerName,
                        Email = data.CustomerEmail,
                        Phone = data.CustomerPhone
                    };
                    context.Customers.Add(customer);
                    context.SaveChanges();
                }

                // Проверяем, существует ли животное в базе данных
                var animal = context.Animals.FirstOrDefault(a => a.Name == data.AnimalName && a.Species == data.AnimalSpecies && a.Age == data.AnimalAge && a.Price == data.AnimalPrice);
                if (animal == null)
                {
                    // Если животного нет, создаем нового
                    animal = new Animals
                    {
                        Name = data.AnimalName,
                        Species = data.AnimalSpecies,
                        Age = data.AnimalAge,
                        Price = data.AnimalPrice
                    };
                    context.Animals.Add(animal);
                    context.SaveChanges();
                }

                // Создаем связь между клиентом и животным
                context.Orders.Add(new Orders
                {
                    Customer_ID = customer.ID_Customer,
                    Animal_ID = animal.ID_Animal,
                    Quantity = data.OrderQuantity,
                    OrderDate = data.OrderDate
                });
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
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
