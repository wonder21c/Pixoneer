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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var textBoxes = new[] { NameBox, TeamBox, GradeBox, PhoneBox, EmailBox };

            if (textBoxes.All(tb => string.IsNullOrWhiteSpace(tb.Text)))
            {
                MessageBox.Show("모든 항목이 비어있으면 추가할 수 없습니다.", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewPerson = new Person
            {
                name = NameBox.Text,
                team = TeamBox.Text,
                grade = GradeBox.Text,
                phoneNum = PhoneBox.Text,
                email = EmailBox.Text
            };
            DialogResult = true; //대화상자 수락
            Close();
        }

        private void CancleButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
