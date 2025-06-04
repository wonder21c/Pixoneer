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
        bool plusButton = false;
        bool minusButton = false;
        bool mulButton = false;
        bool divButton = false;

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
            
            else if (content == "+")
            {

                if (double.TryParse(input, out num))
                {
                    result += num;
                    input = "";
                    if (minusButton || mulButton || divButton)
                    {
                        minusButton = false;
                        mulButton = false;
                        divButton = false;
                    }
                    plusButton = true;
                }
            }

            else if (content == "-")
            {
                if (double.TryParse(input, out num))
                {
                    if (result == 0)
                    {
                        result = num - result;
                        input = "";
                        if (plusButton || mulButton || divButton)
                        {
                            plusButton = false;
                            mulButton = false;
                            divButton = false;
                        }
                        minusButton = true;
                    }
                    else
                        result -= num;
                    input = "";
                    if (plusButton || mulButton || divButton)
                    {
                        plusButton = false;
                        mulButton = false;
                        divButton = false;
                    }
                    minusButton = true;
                }
            }

            else if (content == "*")
            {
                if (double.TryParse(input, out num))
                {
                    if (result <= 0)
                    {
                        result = 1;
                    }

                    result *= num;
                    input = "";
                    if (plusButton || minusButton || divButton)
                    {
                        plusButton = false;
                        minusButton = false;
                        divButton = false;
                    }
                    mulButton = true;
                }
            }

            else if (content == "/")
            {
                if (double.TryParse(input, out num))
                {
                    if (result == 0)
                    {
                        result = num / 1;
                        input = "";
                        if (plusButton || mulButton || minusButton)
                        {
                            plusButton = false;
                            mulButton = false;
                            minusButton = false;
                        }
                        divButton = true;
                    }
                    else
                    {
                        result /= num;
                        input = "";
                        if (plusButton || mulButton || minusButton)
                        {
                            plusButton = false;
                            mulButton = false;
                            minusButton = false;
                        }
                        divButton = true;
                    }
                }
            }


            else if (content == "=")
            {
                if (plusButton && double.TryParse(input, out num))
                {

                    double finalResult = result + num;
                    textbox.Text = finalResult.ToString();
                    //input = finalResult.ToString();
                    plusButton = false;
                }

                else if (minusButton && double.TryParse(input, out num))
                {
                    double finalResult = result - num;
                    textbox.Text = finalResult.ToString();
                    // input = finalResult.ToString();
                    minusButton = false;
                }

                else if (mulButton && double.TryParse(input, out num))
                {
                    double finalResult = result * num;
                    textbox.Text = finalResult.ToString();
                    //input = finalResult.ToString();
                    mulButton = false;
                }

                else if (divButton && double.TryParse(input, out num))
                {
                    double finalResult = result / num;
                    textbox.Text = finalResult.ToString();
                    //input = finalResult.ToString();
                    divButton = false;

                }
            }

            else if (content == "C")
            {
                input = "";
                result = 0;
                plusButton = false;
                minusButton = false;
                mulButton = false;
                divButton = false;
                textbox.Text = "0";
            }


        }


    }


}
