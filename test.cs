using System.Windows;
using System.Windows.Controls;
using System;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        string input = "";
        double result = 0;
        string currentOperator = "";
        bool isFirstInput = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string content = btn.Content.ToString();

            if (char.IsDigit(content, 0) || content == ".")
            {
                input += content;
                textbox.Text = input;
            }
            else if (content == "+" || content == "-" || content == "*" || content == "/")
            {
                ApplyPreviousOperation();
                currentOperator = content;
            }
            else if (content == "=")
            {
                ApplyPreviousOperation();
                textbox.Text = result.ToString();
                input = result.ToString(); // 결과를 다음 입력으로 사용
                currentOperator = "";
                isFirstInput = true; // 다음 계산을 위해 초기화
            }
            else if (content == "C")
            {
                input = "";
                result = 0;
                currentOperator = "";
                isFirstInput = true;
                textbox.Text = "0";
            }
            else if (content == "←")
            {
                if (!string.IsNullOrEmpty(input))
                {
                input = input.Substring(0, input.Length - 1);
                textbox.Text = string.IsNullOrEmpty(input) ? "0" : input;
                }
            }

        }

        private void ApplyPreviousOperation()
        {
            if (!string.IsNullOrEmpty(input))
            {
                double number;
                if (double.TryParse(input, out number))
                {
                    if (isFirstInput)
                    {
                        result = number;
                        isFirstInput = false;
                    }
                    else
                    {
                        switch (currentOperator)
                        {
                            case "+":
                                result += number;
                                break;
                            case "-":
                                result -= number;
                                break;
                            case "*":
                                result *= number;
                                break;
                            case "/":
                                result /= number;
                                break;
                        }
                    }
                }
                input = "";
                textbox.Text = result.ToString();
            }
        }
    }
}
