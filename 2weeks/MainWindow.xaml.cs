using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AddressBook.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AddressBook
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            /*list = new List<Data>
            {
                new Data
                {
                    name = "손정우",
                    team = "개발3부2팀",
                    grade = "연구원",
                    phoneNum = "010-7566-3616",
                    email = "jeongwoo.son@pixoneer.co.kr"
                }
            };*/

            PersonInfo.ItemsSource = people;
            InfoList.ItemsSource = Info;
        }

        ObservableCollection<Person> people = new ObservableCollection<Person>();
        string filePath = "data.txt";

        public List<string> Info { get; set; } = new List<string>()
        {
            "이름",
            "소속",
            "직위",
            "연락처",
            "이메일"
        };

        public class Data
        {
            public string name { get; set; }
            public string team { get; set; }
            public string grade { get; set; }
            public string phoneNum { get; set; }
            public string email { get; set; }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PersonInfo.SelectedItem is Data selected)
            {
                MessageBox.Show($"선택된 사람: {selected.name} / {selected.email}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddPerson();
            if (addWindow.ShowDialog() == true)
            {
               people.Add(addWindow.NewPerson);
               SaveDataToFile();
            }
        }

        private void SaveDataToFile()
        {
            var lines = people.Select(p => p.ToString());
            File.WriteAllLines(filePath, lines);
        }

        private void LoadDataFromFile()
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var person = Person.FromString(line);
                    if (person != null)
                        people.Add(person);
                }
            }
        }


    }
}
