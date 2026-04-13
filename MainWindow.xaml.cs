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
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Grathic_app_2._0;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = this;
    }
    /*private void Button_click(object sender, RoutedEventArgs e)
    {
        string text = valueTextBox.Text;
        if (string.IsNullOrWhiteSpace(text))
            MessageBox.Show("Введите что нибудь");
        double value = double.Parse(text);
        if (Operations.SelectedItem == null)
        {
            MessageBox.Show("Operation not choosen");
            return;
        }
        int selectedOperation = Operations.SelectedIndex;
        double result = 0;
        if (selectedOperation == 0)
        {
            result = value * value;
        }
        else if (selectedOperation == 1)
            result = value * value * value;
        else if (selectedOperation == 2)
        {
            if (value < 0)
            {
                CalculatedValue.Text = "NaN";
                return;
            }
            result = Math.Sqrt(value);     
        }
        CalculatedValue.Text = result.ToString();
        ComboBoxItem selected = Operations.SelectedItem as ComboBoxItem;
        string operation = selected.Content.ToString();
        
        HistoryList.Items.Add(text + " " + operation + " = " + result);
    }
    private void ClearButton_click(object sender, RoutedEventArgs e)
    {
        valueTextBox.Clear();
        CalculatedValue.Clear();
    }
    private void DeleteItem_click(object sender, RoutedEventArgs e)
    {
        if (HistoryList.SelectedItem == null)
        {
            MessageBox.Show("Item not choosen");
            return;
        }   
        HistoryList.Items.Remove(HistoryList.SelectedItem);
    }
    private void ClearListButton_click(object sender, RoutedEventArgs e)
    {
        HistoryList.Items.Clear();
    }
    private void OperationChanged(object sender, RoutedEventArgs e)
    {
    }*/
    private int clicksCount = 0;
    public int ClicksCount
    {
        get {return clicksCount;}
        set {
                clicksCount = value;
                OnPropertyChanged(); 
            }
    }
    public int clickPower = 1;
    public const int OpenedShopScoreCount = 10;
    public const int WinScore = 30;
    private void mainButton_click(object sender, RoutedEventArgs e)
    {
        ClicksCount += clickPower;
        if (ClicksCount == OpenedShopScoreCount)
        {
            ShopButton.IsEnabled = true;
        }
        if (ClicksCount == WinScore)
        {
            MessageBox.Show("Game over");
            MainButton.IsEnabled = false;
        }

    }
    private void ShopButton_click(object sender, RoutedEventArgs e)
    {
        ShopWindow shopWindow = new ShopWindow();
        shopWindow.ShowDialog();
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }   
}