using AddressBook;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace AddressBook
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            PersonInfo.ItemsSource = people; //data grid에 people 컬렉션을 바인딩
            InfoList.ItemsSource = SearchList; //combobox에 SearchList를 바인딩
            LoadDataFromFile();
        }



        ObservableCollection<Person> people = new ObservableCollection<Person>(); //동적 데이터 컬렉션
        string filePath = "C:\\Users\\pixo\\Desktop\\손정우\\AddressBook\\data.csv";

 

        public List<string> SearchList { get; set; } = new List<string>()
        {
            "전체",
            "이름",
            "소속",
            "직급"
        };

        private bool IsFileLocked(string path)
        {
            try
            {
                using (FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                
                return false;
            }
            catch (IOException)
            {
                return true; 
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsFileLocked(filePath))
            {
                MessageBox.Show("csv파일이 열려있어 추가할 수 없습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addWindow = new AddPerson();
            if (addWindow.ShowDialog() == true) // 모달 
            {
                people.Add(addWindow.NewPerson); //list에 추가
                SaveDataToFile();
            }
        }

        private void SaveDataToFile()
        {
            try
            {
                var lines = people.Select(p => p.ToCsV()); //각 Person 객체를 문자열로 변환
                File.WriteAllLines(filePath, lines, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("csv파일이 열려있어 저장할 수 없습니다.: " + ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDataFromFile()
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("데이터 파일이 존재하지 않아 생성하였습니다. " + filePath);
                return;
            }
            try
            {
                var lines = File.ReadAllLines(filePath, Encoding.UTF8);
                foreach (var line in lines)
                {
                    var person = Person.FromCsv(line);
                    if (person != null)
                        people.Add(person);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("csv파일이 열려있어 데이터를 읽어올 수 없습니다. 프로그램을 종료합니다.: " + ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(); //예외 발생시 프로그램 종료

            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (IsFileLocked(filePath))
            {
                MessageBox.Show("csv파일이 열려있어 수정할 수 없습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PersonInfo.SelectedItem is Person selectedPerson) //selectedPerson이 Person 타입인지 확인 후 할당
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
            if (IsFileLocked(filePath))
            {
                MessageBox.Show("csv파일이 열려있어 삭제할 수 없습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PersonInfo.SelectedItem is Person selectedPerson)
            {
                if (MessageBox.Show("정말 삭제하시겠습니까?", "삭제", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    people.Remove(selectedPerson);
                    SaveDataToFile();

                }
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
                SearchTextBox.IsReadOnly = false;

                var filteredPeople = people.Where(p =>
                    (selectedFilter == "이름" && p.name != null && p.name.ToLower().Contains(searchText)) ||
                    (selectedFilter == "소속" && p.team != null && p.team.ToLower().Contains(searchText)) ||
                    (selectedFilter == "직급" && p.grade != null && p.grade.ToLower().Contains(searchText))
                ).ToList();
                PersonInfo.ItemsSource = filteredPeople;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SearchButton_Click(null, null);
        }
        private void InfoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InfoList.SelectedIndex == 0)
            {
                SearchTextBox.IsReadOnly = true;
                SearchTextBox.Text = string.Empty;
                PersonInfo.ItemsSource = people;
                SearchTextBox.Background = Brushes.Gray;
            }
            else
            {
                SearchTextBox.IsReadOnly = false;
                SearchTextBox.Background = Brushes.White;
                //PersonInfo.ItemsSource = people;
            }

        }
    }
}
