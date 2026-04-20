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
using System.Text.Json.Nodes;
using System.Windows.Threading;
namespace Grathic_app_2._0;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    
    public MainWindow()
    {
        InitializeComponent();
        CritClick = new Random();
        autoClickTimer = new DispatcherTimer();
        autoClickTimer.Interval = TimeSpan.FromSeconds(2);
        autoClickTimer.Tick += AutoClickTimer_click;
        this.DataContext = this;
    }
    //Shop parameters
    public const int OpenedShopScoreCount = 10;
    public int PowerClickLevel = 0;
    public int AutoClickLevel = 0;
    public int CriticalClickLevel = 0;
    public float CriticalChance = 0;
    private Random CritClick;
    private DispatcherTimer autoClickTimer;
    public int autoClickPower = 0;

    //global param
    private int clicksCount = 890;
    public int ClicksCount
    {
        get {return clicksCount;}
        set {
                clicksCount = value;
                OnPropertyChanged(); 
            }
    }
    public int clickPower = 1;
    public const int WinScore = 1000;
    private void mainButton_click(object sender, RoutedEventArgs e)
    { 
        double CritNum = CritClick.NextDouble();
        int CritClickBonus = 1;
        if (CritNum <= CriticalChance)
            CritClickBonus = 5;
        ClicksCount += clickPower * CritClickBonus;
        if (ClicksCount >= OpenedShopScoreCount)
        {
            ShopButton.IsEnabled = true;
        }
        if (ClicksCount >= WinScore)
        {
            MessageBox.Show("Game over");
            MainButton.IsEnabled = false;
        }
    }
    private void ShopButton_click(object sender, RoutedEventArgs e)
    {
        ShopWindow shopWindow = new ShopWindow(this, ClicksCount, PowerClickLevel, AutoClickLevel, CriticalClickLevel);
        shopWindow.ScoreChanged += OnScoreChanged;
        shopWindow.ClickPowerUpgrade += PowerUpgrade;
        shopWindow.ClickAutoUpgrade += AutoUpgrade;
        shopWindow.ClickCriticalUpgrade += CriticalUpgrade;
        shopWindow.ClickRestartButton += Restarting;
        shopWindow.ShowDialog();
    }
    private void OnScoreChanged(int newScore)
    {
        ClicksCount = newScore;
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }   
    public void PowerUpgrade()
    {
        clickPower *= 2;
        PowerClickLevel++;
    }
    public void AutoUpgrade(int level)
    {
        AutoClickLevel++;
        if (level == 0)
        {
            autoClickPower = 3;
            autoClickTimer.Start();
        }
        else if (level == 1)
            autoClickPower = 7;
        else
            autoClickPower = 10;
    }
    public void CriticalUpgrade(int level)
    {
        if (level == 0)
            CriticalChance = 0.005f;
        else
            CriticalChance += 0.0025f;
        CriticalClickLevel++;
    }
    public void Restarting()
    {
        clickPower = 1;
        PowerClickLevel = 0;
        AutoClickLevel = 0;
        CriticalClickLevel = 0;
        CriticalChance = 0;
        autoClickPower = 0;
        autoClickTimer.Stop();
    }
    private void AutoClickTimer_click(object sender, EventArgs e)
    {
        ClicksCount += autoClickPower;
    }
}