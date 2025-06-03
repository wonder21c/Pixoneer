```
using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        string input = "";            // 현재 입력 중인 숫자
        double firstNumber = 0;       // 첫 번째 숫자 저장
        bool isPlusPressed = false;   // + 버튼 눌렸는지 여부

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string content = btn.Content.ToString();

            // 숫자 입력 처리
            if (char.IsDigit(content, 0))
            {
                input += content;
                textbox.Text = input;
            }
            // + 버튼 클릭
            else if (content == "+")
            {
                if (double.TryParse(input, out firstNumber))
                {
                    input = "";
                    isPlusPressed = true;
                }
            }
            // = 버튼 클릭
            else if (content == "=")
            {
                if (isPlusPressed && double.TryParse(input, out double secondNumber))
                {
                    double result = firstNumber + secondNumber;
                    textbox.Text = result.ToString();
                    input = result.ToString();
                    isPlusPressed = false;
                }
            }
            // C 버튼 클릭
            else if (content == "C")
            {
                input = "";
                firstNumber = 0;
                isPlusPressed = false;
                textbox.Text = "";
            }
        }
    }
}
```
