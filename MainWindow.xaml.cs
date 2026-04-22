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
using System.Threading.Tasks;
using WinMessageBox = System.Windows.MessageBox;
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
        RandomEvent = new Random();
        autoClickTimer = new DispatcherTimer();
        autoClickTimer.Interval = TimeSpan.FromSeconds(2);
        autoClickTimer.Tick += AutoClickTimer_click;

        eventsArray = new Events[1];
        eventsArray[0] = ButtonTeleport;

        gameStartTime = DateTime.Now;
        comboClickTimer = new DispatcherTimer();
        comboClickTimer.Interval = TimeSpan.FromSeconds(2);
        comboClickTimer.Tick += ResetComboClick;
        comboDecayTimer = new DispatcherTimer();
        comboDecayTimer.Interval = TimeSpan.FromMilliseconds(30);
        comboDecayTimer.Tick += ResetComboClickRec;
        VisibleTime = new DispatcherTimer();
        VisibleTime.Interval = TimeSpan.FromSeconds(1);
        VisibleTime.Tick += HideComboText;

        this.DataContext = this;
    }
    //Shop parameters
    public const int OpenedShopScoreCount = 10;
    public int PowerClickLevel = 0;
    public int AutoClickLevel = 0;
    public int CriticalClickLevel = 0;
    public double CriticalChance = 0;
    private Random CritClick;
    private DispatcherTimer autoClickTimer;
    public int autoClickPower = 0;
    //Developer param
    private List<Key> keyCombinations = new List<Key>();
    private List<Key> KonamiCode = new List<Key>{Key.Up, Key.Up, Key.Down, Key.Down, Key.Left, Key.Right, Key.Left, Key.Right, Key.B, Key.A};
    private DateTime gameStartTime;
    //Random event
    private Random RandomEvent;
    private double RandEventChance = 0.00001f;
    private int eventsClicks = 0;
    private int Duration = 50;
    private Events currentEvent;
    private bool isEvent = false;
    private delegate void Events();
    Events[] eventsArray;
    //global param
    private int clicksCount = 0;
    public int ClicksCount
    {
        get {return clicksCount;}
        set {
                clicksCount = value;
                OnPropertyChanged(); 
            }
    }
    private int comboClick = 0;
    public int ComboClick
    {
        get {return comboClick;}
        set
        {
            comboClick = value;
            OnPropertyChanged(); 
        }
    }
    private DispatcherTimer comboClickTimer;
    private DispatcherTimer comboDecayTimer;
    private DispatcherTimer VisibleTime;
    private int targetCombo = 0; // целевое значение (будет уменьшаться до 0)
    public int clickPower = 1;
    public int WinScore = 1000;
    private void mainButton_click(object sender, RoutedEventArgs e)
    { 
        double CritNum = CritClick.NextDouble();
        int CritClickBonus = 1;
        if (CritNum <= CriticalChance)
        {
            CritClickBonus = 5;
            ShowCritPopup();
        }
        ClicksCount += clickPower * CritClickBonus;
        ComboClick++;
        if (ComboClick > 0)
        {
            ComboText.Visibility = Visibility.Visible;
        }
        comboDecayTimer.Stop();
        comboClickTimer.Stop();
        comboClickTimer.Start();
        
        if (ClicksCount >= OpenedShopScoreCount)
        {
            ShopButton.IsEnabled = true;
        }
        if (ClicksCount >= WinScore)
        {
            WinMessageBox.Show(
                "You won!\n\n" +
                "Secret code for DevConsole:\n" +
                "↑ ↑ ↓ ↓ ← → ← → B A\n\n" +
                "(Available only in first 10 seconds of new game)",
                "Congratulations!",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
            MainButton.IsEnabled = false;
            return;
        }
        if (isEvent)
        {
            eventsClicks++;
            currentEvent?.Invoke();
            return;
        }
        double RandEventNum = RandomEvent.NextDouble();
        if (RandEventNum < RandEventChance)
        {
            WinMessageBox.Show("New event", "event", MessageBoxButton.OK, MessageBoxImage.Warning);
            LaunchRandomEvent();
            RandEventChance = 0.00001;
        }
        else
        {
            RandEventChance += 0.000005;
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
    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Delete)
        {
            keyCombinations.Clear();
            return;
        }
        keyCombinations.Add(e.Key);
        if (keyCombinations.Count > KonamiCode.Count)
            keyCombinations.RemoveAt(0);
        if (keyCombinations.Count == KonamiCode.Count)
        {
            for (int i = 0; i < KonamiCode.Count; i++)
            {
                if (keyCombinations[i] != KonamiCode[i])
                {
                    keyCombinations.Clear();
                    return;
                }
            }
            DeveloperConsoleActivated();
            keyCombinations.Clear();
        }
    }
    private void DeveloperConsoleActivated()
    {
        TimeSpan diffTime = DateTime.Now - gameStartTime;
        if (diffTime.TotalSeconds > 10)
        {
            WinMessageBox.Show("Console access denied", "denied", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        DeveloperConsole developerConsole = new DeveloperConsole(this, clicksCount, PowerClickLevel, AutoClickLevel, CriticalClickLevel, WinScore);
        developerConsole.PowerChanged += PowerUpgrade;
        developerConsole.AutoChanged += AutoUpgrade;
        developerConsole.CriticalChanged += CriticalUpgrade;
        developerConsole.ShowDialog();        
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
    public void PowerUpgrade(int level)
    {
        clickPower = 2 * (level + 1);
        PowerClickLevel = level + 1;
    }
    public void AutoUpgrade(int level)
    {
        AutoClickLevel = level + 1;
        if (level == 0)
        {
            autoClickPower = 3;
            autoClickTimer.Start();
        }
        else if (level == 1)
            autoClickPower = 7;
        else if (level == 2)
            autoClickPower = 10;
        else
            autoClickPower = (level + 1) * 3;
    }
    public void CriticalUpgrade(int level)
    {
        if (level == 0)
            CriticalChance = 0.005f;
        else
            CriticalChance = 0.005f + level * 0.0025f;
        CriticalClickLevel = level + 1;
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
        ShowAutoClickPopup();
    }
    private async void ShowAutoClickPopup()
    {
        AutoClickPopup.Text = $"+{autoClickPower}";
        AutoClickPopup.Opacity = 1;
        await Task.Delay(500);
        AutoClickPopup.Opacity = 0;
    }
    private async void ShowCritPopup()
    {
        CritPopup.Opacity = 1;
        await Task.Delay(300);
        CritPopup.Opacity = 0;
    }
    private void ResetComboClick(object sender, EventArgs e)
    {
        comboDecayTimer.Start();
        comboClickTimer.Stop();
    }
    private void ResetComboClickRec(object sender, EventArgs e)
    {
        ComboClick--;
        if (ComboClick == 0)
        {
            VisibleTime.Start();
            comboDecayTimer.Stop();
        }       
    }
    private void HideComboText(object sender, EventArgs e)
    {
        ComboText.Visibility = Visibility.Hidden;
        VisibleTime.Stop();
    }
    private void LaunchRandomEvent()
    {
        eventsClicks = 0;
        isEvent = true;
        Random random = new Random();
        int randNum = random.Next(0, eventsArray.Length);
        eventsArray[randNum]?.Invoke();  
        currentEvent = eventsArray[randNum];      
    }
    private void ButtonTeleport()
    {
        Random randCoord = new Random();
        int leftMargin = randCoord.Next(10, 320);
        int UpMargin = randCoord.Next(10, 320);
        int rightMargin = randCoord.Next(10, 320);
        int downMargin = randCoord.Next(10, 320);
        MainButton.Margin = new Thickness(leftMargin, UpMargin, rightMargin, downMargin);
        if (eventsClicks >= Duration)
        {
            isEvent = false;
            MainButton.Margin = new Thickness(0, 0, 50, 0);
            currentEvent = null;
            WinMessageBox.Show("Event over", "end", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}