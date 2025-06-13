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
        }

        ObservableCollection<Person> listPeoples = new ObservableCollection<Person>(); //동적 데이터 컬렉션
        string filePath = "D:\\손정우\\AddressBook\\data.csv";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.InfoList.SelectedIndex = 0;
            dgPersonInfo.ItemsSource = listPeoples; //data grid에 people 컬렉션을 바인딩
            LoadDataFromFile();
        }

  
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
                    var person = Person.FromCSV(line);
                    if (person != null)
                        listPeoples.Add(person);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("csv파일이 열려있어 데이터를 읽어올 수 없습니다. 프로그램을 종료합니다.: " + ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(); //예외 발생시 프로그램 종료

            }
        }

        private void SaveDataToFile()
        {
            try
            {
                var lines = listPeoples.Where(p => p != null).Select(p => p.ToCSV()); //각 Person 객체를 문자열로 변환
                File.WriteAllLines(filePath, lines, Encoding.UTF8);
            }
            catch (Exception ex)
            { 
                MessageBox.Show("csv파일이 열려있어 저장할 수 없습니다.: " + ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsFileLocked(filePath))
            {
                MessageBox.Show("csv파일이 열려있어 추가할 수 없습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addWindow = new AddPerson();
            if (addWindow.ShowDialog() == true ) // 모달 
            {
                listPeoples.Add(addWindow.NewPerson); //list에 추가
                SaveDataToFile();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (IsFileLocked(filePath))
            {
                MessageBox.Show("csv파일이 열려있어 수정할 수 없습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dgPersonInfo.SelectedItem is Person selectedPerson) //selectedPerson이 Person 타입인지 확인 후 할당
            {
                var editWindow = new AddPerson(selectedPerson);
                if (editWindow.ShowDialog() == true)
                {
                    selectedPerson.name = editWindow.NewPerson.name;
                    selectedPerson.team = editWindow.NewPerson.team;
                    selectedPerson.grade = editWindow.NewPerson.grade;
                    selectedPerson.phoneNum = editWindow.NewPerson.phoneNum;
                    selectedPerson.email = editWindow.NewPerson.email;

                    dgPersonInfo.Items.Refresh();
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
            if (dgPersonInfo.SelectedItem is Person selectedPerson)
            {
                if (MessageBox.Show("정말 삭제하시겠습니까?", "삭제", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    listPeoples.Remove(selectedPerson);
                    SaveDataToFile();

                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = edtSearchText.Text.ToLower();
            var selectedFilter = (InfoList.SelectedItem as ComboBoxItem).Content;

            if (string.IsNullOrEmpty(searchText))
            {
                dgPersonInfo.ItemsSource = listPeoples;
            }
            else
            {
                edtSearchText.IsReadOnly = false;
                List<Person> list = new List<Person>();
                switch(selectedFilter)
                {
                    case "이름":
                        list = listPeoples.Where(o=>o.name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) > -1).ToList();
                        break;
                    case "소속":
                        list = listPeoples.Where(o => o.team.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) > -1).ToList();
                        break;
                    case "직급":
                        list = listPeoples.Where(o => o.grade.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) > -1).ToList();
                        break;
                }       
                dgPersonInfo.ItemsSource = list;
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
                edtSearchText.IsReadOnly = true;
                edtSearchText.Text = string.Empty;
                dgPersonInfo.ItemsSource = listPeoples;
                edtSearchText.Background = Brushes.Gray;
            }
            else
            {
                edtSearchText.IsReadOnly = false;
                edtSearchText.Background = Brushes.White;               
            }
        }       
    }
}
