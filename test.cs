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
        string currentContent = "";
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

    } 
}    
