using System;
using System.Windows;
using System.Windows.Controls;
using AddressBook;

namespace AddressBook
{
    public partial class AddPerson : Window
    {
        public Person NewPerson { get;  set; }

        public AddPerson()
        {
            InitializeComponent();
        }

        public AddPerson(Person selectedPerson)
        {
            InitializeComponent();
            if (selectedPerson != null)
            {
                NameBox.Text = selectedPerson.name;
                TeamBox.Text = selectedPerson.team;
                GradeBox.Text = selectedPerson.grade;
                PhoneBox.Text = selectedPerson.phoneNum;
                EmailBox.Text = selectedPerson.email;
            }
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
