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
        int num1 = 0;
        bool plusButton = false;
        public MainWindow()
        {
            InitializeComponent();
        }
    private void button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string content = btn.Content.ToString();

            
            if (char.IsDigit(content, 0))
            {
                input += content;
                textbox.Text = input;
            }

            else if (content == "+")
            {
                if (int.TryParse(input, out num1))
                {
                    input = "";
                    plusButton = true;
                }
            }

            else if (content == "=")
            {
                if (plusButton && int.TryParse(input, out int num2))
                {
                    int result = num1 + num2;
                    textbox.Text = result.ToString();
                    input = result.ToString();
                    plusButton = false;
                }
            }

            else if (content == "C")
            {
                input = "";
                num1 = 0;
                plusButton = false;
                textbox.Text = "0";
            }
            ;
        }

       
    }


}
