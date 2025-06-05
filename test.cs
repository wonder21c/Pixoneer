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
        string back = " ";
        bool firstNum = true;
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
                cal();
                currentContent = content;
            }



            else if (content == "=")
            {
                cal();
                textbox.Text = result.ToString();
                input = result.ToString();
                currentContent = "";
                firstNum = true;

            }

            else if (content == "C")
            {
                input = "";
                result = 0;
                currentContent = "";
                firstNum = true;
                textbox.Text = "0";
            }

            else if (content == "Backspace") 
            {
                if (!string.IsNullOrEmpty(input))
                {
                    input = input.Substring(0, input.Length - 1);
                    textbox.Text = string.IsNullOrEmpty(input) ? "0" : input;
                }
            
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string num = (e.Key - Key.D0).ToString();
                buttonPressed(num);
            }

            else if(e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                {
                string num = (e.Key - Key.NumPad0).ToString();
                buttonPressed(num);
            }

            else if(e.Key == Key.Add || e.Key == Key.OemPlus && (Keyboard.Modifiers & ModifierKeys.Shift ) != 0)
            {
                buttonPressed("+");
            }

            else if(e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                buttonPressed("-");
            }
            else if(e.Key == Key.Multiply)
            {
                buttonPressed("*");
            }
            else if(e.Key == Key.Divide || e.Key == Key.Oem2)
            {
                buttonPressed("/");
            }

            else if (e.Key == Key.Enter)
            {
                buttonPressed("=");
            }

            else if (e.Key == Key.Back)
            {
                buttonPressed("Backspace");
            }

            else if (e.Key == Key.Escape)
            {
                buttonPressed("C");
            }

            else if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                buttonPressed(".");
            }
        }
        
        private void buttonPressed(string content)
        {
            Button button1 = new Button();
            button1.Content = content;
            button_Click(button1, null);

        }
        

        private void cal()
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

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {

        }
    } 

}    
