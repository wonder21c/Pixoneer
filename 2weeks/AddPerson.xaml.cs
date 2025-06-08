using System;
using System.Windows;
using System.Windows.Controls;
using AddressBook.Model;

namespace AddressBook
{
    public partial class AddPerson : Window
    {
        public Person NewPerson { get; private set; }

        public AddPerson()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewPerson = new Person
            {
                name = NameBox.Text,
                team = TeamBox.Text,
                grade = GradeBox.Text,
                phoneNum = PhoneBox.Text,
                email = EmailBox.Text
            };
            DialogResult = true;
            Close();
        }
    }
}
