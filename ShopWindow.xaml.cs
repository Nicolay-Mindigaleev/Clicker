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
using System.Windows.Threading;
namespace Grathic_app_2._0;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ShopWindow : Window, INotifyPropertyChanged
{
    public event Action<int> ScoreChanged;
    public event Action<int> ClickPowerUpgrade;
    public event Action<int> ClickAutoUpgrade;
    public event Action<int> ClickCriticalUpgrade;
    public event Action ClickRestartButton;
    public ShopWindow(MainWindow mainWin, int currScore, int powClickLvl, int autoClickLvl, int critClickLvl)
    {
        InitializeComponent();
        this.DataContext = this;
        mainWindow = mainWin;
        PlayerScore = currScore;
        powerClickLevel = powClickLvl;
        powerClickPrice *= (int)Math.Pow(4, powerClickLevel);
        autoClickLevel = autoClickLvl;
        autoClickPrice *= (int)Math.Pow(4, autoClickLevel);
        criticalClickLevel = critClickLvl;
        criticalClickPrice *= (int)Math.Pow(4, criticalClickLevel);
        CheckData();
        UpdateTimer = new DispatcherTimer();
        UpdateTimer.Interval = TimeSpan.FromMilliseconds(100);
        UpdateTimer.Tick += UpdateScore;
        UpdateTimer.Start(); 
    }
    private int playerScore;
    public int PlayerScore
    {
        get {return playerScore;}
        set {
                playerScore = value;
                OnPropertyChanged(); 
            }
    }
    private DispatcherTimer UpdateTimer;
    private MainWindow mainWindow;
    //levels
    private int powerClickLevel;
    private int autoClickLevel;
    private int criticalClickLevel;
    //prices
    private int powerClickPrice = 15;
    public int PowerClickPrice
    {
        get {return powerClickPrice;}
        set {
                powerClickPrice = value;
                OnPropertyChanged();
            }
    }
    private int autoClickPrice = 15;
    public int AutoClickPrice
    {
        get {return autoClickPrice;}
        set {
                autoClickPrice = value;
                OnPropertyChanged();
            }
    }
    private int criticalClickPrice = 15;
    public int CriticalClickPrice
    {
        get {return criticalClickPrice;}
        set {
                criticalClickPrice = value;
                OnPropertyChanged();
            }
    }
    private int restartPrice = 900;
    private void CheckData()
    {
        if (playerScore >= powerClickPrice)
            PowerClickButton.IsEnabled = true;
        else
            PowerClickButton.IsEnabled = false;

        if (playerScore >= autoClickPrice)
            AutoClickButton.IsEnabled = true;
        else
            AutoClickButton.IsEnabled = false;

        if (playerScore >= criticalClickPrice)
            CriticalClickButton.IsEnabled = true;
        else
            CriticalClickButton.IsEnabled = false;
        if (playerScore >= restartPrice)
            RestartButton.IsEnabled = true;
        else   
            RestartButton.IsEnabled = false;
        if (powerClickLevel == 1)
        {
            PowClLv1.Background = new SolidColorBrush(Colors.LightGreen);
        }
        else if (powerClickLevel == 2)
        {
            PowClLv1.Background = new SolidColorBrush(Colors.Yellow);
            PowClLv2.Background = new SolidColorBrush(Colors.Yellow);
        }
        else if (powerClickLevel == 3)
        {
            PowClLv1.Background = new SolidColorBrush(Colors.Red);
            PowClLv2.Background = new SolidColorBrush(Colors.Red);
            PowClLv3.Background = new SolidColorBrush(Colors.Red);
            PowerClickButton.Content = "MAX";
            PowerClickButton.IsEnabled = false;
        }

        if (autoClickLevel == 1)
        {
            AutoClLv1.Background = new SolidColorBrush(Colors.LightGreen);
        }
        else if (autoClickLevel == 2)
        {
            AutoClLv1.Background = new SolidColorBrush(Colors.Yellow);
            AutoClLv2.Background = new SolidColorBrush(Colors.Yellow);
        }
        else if (autoClickLevel == 3)
        {
            AutoClLv1.Background = new SolidColorBrush(Colors.Red);
            AutoClLv2.Background = new SolidColorBrush(Colors.Red);
            AutoClLv3.Background = new SolidColorBrush(Colors.Red);
            AutoClickButton.Content = "MAX";
            AutoClickButton.IsEnabled = false;
        }

        if (criticalClickLevel == 1)
        {
            CritClLv1.Background = new SolidColorBrush(Colors.LightGreen);
        }
        else if (criticalClickLevel == 2)
        {
            CritClLv1.Background = new SolidColorBrush(Colors.Yellow);
            CritClLv2.Background = new SolidColorBrush(Colors.Yellow);
        }
        else if (criticalClickLevel == 3)
        {
            CritClLv1.Background = new SolidColorBrush(Colors.Red);
            CritClLv2.Background = new SolidColorBrush(Colors.Red);
            CritClLv3.Background = new SolidColorBrush(Colors.Red);
            CriticalClickButton.Content = "MAX";
            CriticalClickButton.IsEnabled = false;
        }
    }
    private void CloseWindow_click(object sender, RoutedEventArgs e)
    {
        UpdateTimer.Stop();
        this.Close();
    }
    private void UpgradePower_click(object sender, RoutedEventArgs e)
    {
        PlayerScore -= powerClickPrice;
        ScoreChanged?.Invoke(playerScore);
        ClickPowerUpgrade?.Invoke(powerClickLevel);
        powerClickLevel++;
        PowerClickPrice *= 4;
        CheckData();
    }
    private void UpgradeAuto_click(object sender, RoutedEventArgs e)
    {
        PlayerScore -= autoClickPrice;
        ScoreChanged?.Invoke(playerScore);
        ClickAutoUpgrade?.Invoke(autoClickLevel);
        autoClickLevel++;
        AutoClickPrice *= 4;
        CheckData();
    }
    private void UpgradeCritical_click(object sender, RoutedEventArgs e)
    {
        PlayerScore -= criticalClickPrice;
        ScoreChanged?.Invoke(playerScore);
        ClickCriticalUpgrade?.Invoke(criticalClickLevel);
        criticalClickLevel++;
        CriticalClickPrice *= 4;
        CheckData();
    }
    private void Restart_click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult AUSure = MessageBox.Show("Are you sure?","SURE?", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (AUSure == MessageBoxResult.Yes)
        {
            PlayerScore -= restartPrice;
            ScoreChanged?.Invoke(playerScore);
            ClickRestartButton?.Invoke();
            this.Close();         
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }   
    private void UpdateScore(object sender, EventArgs e)
    {
        PlayerScore = mainWindow.ClicksCount;
        CheckData();
    }
}