using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AddressBook;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace AddressBook
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            PersonInfo.ItemsSource = people;
            InfoList.ItemsSource = SearchList;
            LoadDataFromFile();
        }

      

        ObservableCollection<Person> people = new ObservableCollection<Person>(); //동적 데이터 컬렉션
        string filePath = "C:\\Users\\pixo\\Desktop\\손정우\\AddressBook\\data.txt";

        public List<string> Info { get; set; } = new List<string>() //자동 프로퍼티
        {
            "이름",
            "소속",
            "직위",
            "연락처",
            "이메일"
        };

        public List<string> SearchList { get; set; } = new List<string>()
        {
            "전체",
            "이름",
            "소속",
            "직급"
        };


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddPerson();
            if (addWindow.ShowDialog() == true) // 모달 
            {
                people.Add(addWindow.NewPerson); //list에 추가
                SaveDataToFile();
            }
        }

        private void SaveDataToFile()
        {
            var lines = people.Select(p => p.ToString());
            File.WriteAllLines(filePath, lines);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (PersonInfo.SelectedItem is Person selectedPerson)
            {
               
                var editWindow = new AddPerson(selectedPerson);
                if (editWindow.ShowDialog() == true)
                {                   
                    selectedPerson.name = editWindow.NewPerson.name;
                    selectedPerson.team = editWindow.NewPerson.team;
                    selectedPerson.grade = editWindow.NewPerson.grade;
                    selectedPerson.phoneNum = editWindow.NewPerson.phoneNum;
                    selectedPerson.email = editWindow.NewPerson.email;

                    PersonInfo.Items.Refresh(); 
                    SaveDataToFile();           
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (PersonInfo.SelectedItem is Person selectedPerson)
            {
                if(MessageBox.Show("정말 삭제하시겠습니까?","삭제", MessageBoxButton.YesNo)==MessageBoxResult.Yes)
                {
                    people.Remove(selectedPerson);
                    SaveDataToFile();

                }
            }
        }


        private void LoadDataFromFile()
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("데이터 파일이 존재하지 않습니다: " + filePath);
                return;
            }
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var person = Person.FromString(line);
                if (person != null)
                    people.Add(person);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            var selectedFilter = InfoList.SelectedItem as string;

            if (string.IsNullOrEmpty(searchText) || selectedFilter == "전체")
            {
                PersonInfo.ItemsSource = people; 
            }
            else
            {
                var filteredPeople = people.Where(p =>
                    (selectedFilter == "이름" && p.name != null && p.name.ToLower().Contains(searchText)) ||
                    (selectedFilter == "소속" && p.team != null && p.team.ToLower().Contains(searchText)) ||
                    (selectedFilter == "직급" && p.grade != null && p.grade.ToLower().Contains(searchText))
                ).ToList();
                PersonInfo.ItemsSource = filteredPeople; 
            }
        }

    }
}
