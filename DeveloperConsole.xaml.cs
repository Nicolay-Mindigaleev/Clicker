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
using Xceed.Wpf.Toolkit;
using WinMessageBox = System.Windows.MessageBox;
namespace Grathic_app_2._0;
public partial class DeveloperConsole : Window, INotifyPropertyChanged
{
    public event Action<int> PowerChanged;
    public event Action<int> AutoChanged;
    public event Action<int> CriticalChanged;
    public DeveloperConsole(MainWindow mainWin, int currScore, int powClickLvl, int autoClickLvl, int critClickLvl, int winScore)
    {
        InitializeComponent();
        this.DataContext = this;
        mainWindow = mainWin;
        playerScore = currScore;
        powerClickLevel = powClickLvl;
        autoClickLevel = autoClickLvl;
        criticalClickLevel = critClickLvl;
        WinScore = winScore;
    }
    private int playerScore;
    public int PlayerScore
    {
        get {return playerScore;}
        set
        {
            playerScore = value;
            OnPropertyChanged();
            mainWindow.ClicksCount = value;
        }
    }
    private int powerClickLevel;
    private int autoClickLevel;
    private int criticalClickLevel;
    private int WinScore;
    private MainWindow mainWindow;
    public void PowerLevel_changed(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        powerClickLevel = (int)((IntegerUpDown)sender).Value;
        PowerChanged?.Invoke(powerClickLevel);
    }
    public void AutoLevel_changed(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        autoClickLevel = (int)((IntegerUpDown)sender).Value;
        AutoChanged?.Invoke(autoClickLevel - 1);
    }
    public void CriticalLevel_changed(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        criticalClickLevel = (int)((IntegerUpDown)sender).Value; 
        CriticalChanged?.Invoke(criticalClickLevel);
    }
    public void SetCLicksButton_click(object sender, RoutedEventArgs e)
    {
        PlayerScore += (int)NewClicksValue.Value;
        mainWindow.ClicksCount += (int)NewClicksValue.Value;
    }
    public void SetWinnerScore_click(object sender, RoutedEventArgs e)
    {
        mainWindow.WinScore = (int)NewWinnerScore.Value;
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        
    }   
}