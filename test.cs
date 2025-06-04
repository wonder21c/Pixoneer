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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string input = "";
        double num = 0;
        double result = 0;
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
                if (!string.IsNullOrEmpty(input))
                {
                    double num;

                    if (double.TryParse(input, out num))
                    {
                        if (firstNum)
                        {
                            result = num;
                            firstNum = false;
                        }
                        else
                        {
                            switch (content)
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
                    //textbox.Text = result.ToString();
                }
            }


            else if (content == "=")
            {
                textbox.Text = result.ToString();
                //input = result.ToString();
                firstNum = true;

            }

            else if (content == "C")
            {
                input = "";
                result = 0;

                textbox.Text = "0";
            }





        }

    }
}
