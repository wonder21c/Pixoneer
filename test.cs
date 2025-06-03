
using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        string input = "";           
        double firstNumber = 0;      
        bool isPlusPressed = false;   
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
           
            else if (content == "+")
            {
                if (double.TryParse(input, out firstNumber))
                {
                    input = "";
                    isPlusPressed = true;
                }
            }
            
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

