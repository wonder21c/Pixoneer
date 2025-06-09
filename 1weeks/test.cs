using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        string input = "";
        double num = 0;
        double result = 0;
        string currentContent = "";
        bool firstNum = true;
        StringBuilder history = new StringBuilder();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalcFunc(string func)
        {
            if (char.IsDigit(func, 0) || func == ".")
            {
                input += func;
                history.Append(func);
                textbox.Text = input;
                HistoryText.Text = history.ToString();
            }
            else if (func == "+" || func == "-" || func == "*" || func == "/")
            {
                Symbols();
                currentContent = func;
                history.Append(func);
                HistoryText.Text = history.ToString();
            }
            else if (func == "=")
            {
                Symbols();
                textbox.Text = Math.Round(result, 3).ToString("#,##0.####");
                input = result.ToString();
                currentContent = "";
                history.Append("=" + Math.Round(result, 3).ToString("#,##0.####"));
                HistoryText.Text = history.ToString();
            }
            else if (func == "C")
            {
                input = "";
                result = 0;
                currentContent = "";
                firstNum = true;
                textbox.Text = "0";
                history.Clear();
                HistoryText.Text = "";
            }
            else if (func == "Backspace")
            {       
                if (!string.IsNullOrEmpty(textbox.Text) && textbox.Text != "0")
                {   
                    history.Clear();
                    HistoryText.Text = result.ToString();
                    
                }
                else if (!string.IsNullOrEmpty(input))
                {
                    input = input.Substring(0, input.Length - 1);
                    textbox.Text = string.IsNullOrEmpty(input) ? "0" : input;
                    if (history.Length > 0)
                        history.Remove(history.Length - 1, 1);
                    HistoryText.Text = history.ToString();
                }
                else if (!string.IsNullOrEmpty(currentContent))
                {
                    if (history.Length > 0)
                        history.Remove(history.Length - 1, 1);
                    currentContent = "";
                    HistoryText.Text = history.ToString();
                }
            }
        }
 
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string content = btn.Content.ToString();

            if (content != null && content != string.Empty)
            {
                CalcFunc(content);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string num = (e.Key - Key.D0).ToString();
                CalcFunc(num);
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                string num = (e.Key - Key.NumPad0).ToString();
                CalcFunc(num);
            }
            else if (e.Key == Key.Add || e.Key == Key.OemPlus && (Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                CalcFunc("+");
            }
            else if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                CalcFunc("-");
            }
            else if (e.Key == Key.Multiply)
            {
                CalcFunc("*");
            }
            else if (e.Key == Key.Divide || e.Key == Key.Oem2)
            {
                CalcFunc("/");
            }
            else if (e.Key == Key.Enter)
            {
                CalcFunc("=");
            }
            else if (e.Key == Key.Back)
            {
                CalcFunc("Backspace");
            }
            else if (e.Key == Key.Escape)
            {
                CalcFunc("C");
            }
            else if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                CalcFunc(".");
            }
        }


        private void Symbols()
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (double.TryParse(input, out num))
                {
                    if (firstNum)
                    {
                        result = num;
                        firstNum = false;
                    }
                    else
                    {
                        switch (currentContent)
                        {
                            case "+":
                                result += num; break;

                            case "-":
                                result -= num; break;

                            case "*":
                                result *= num; break;

                            case "/":
                                result /= num; break;
                        }
                    }
                }
                input = "";
                textbox.Text = result.ToString();
            }
        }
    }

}    
