using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfCalc
{
    public partial class MainWindow : Window
    {
        private readonly Data _data;

        public MainWindow()
        {
            InitializeComponent();
            _data = new Data();
            MainGrid.DataContext = _data;
        }
    }

    public class Data : INotifyPropertyChanged
    {
        private string _num1 = "", _num2 = "";
        private ComboBoxItem _op;

        public ComboBoxItem Op
        {
            get => _op;
            set
            {
                _op = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Result"));
            }
        }

        public string Num1
        {
            get => _num1;
            set
            {
                _num1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Result"));
            }
        }

        public string Num2
        {
            get => _num2;
            set
            {
                _num2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Result"));
            }
        }

        public string Result
        {
            get
            {
                if (!double.TryParse(Num1, out var num1)
                    || !double.TryParse(Num2, out var num2))
                    return "Wrong Input";
                if (_op == null)
                    return "Please Select Operator";
                return _op.Content.ToString() switch
                {
                    "+" => (num1 + num2).ToString("F6"),
                    "-" => (num1 - num2).ToString("F6"),
                    "*" => (num1 * num2).ToString("F6"),
                    "/" => num2 != 0 ? (num1 / num2).ToString("F6") : "Divided by zero!",
                    _ => ""
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}