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

namespace chamcong.WpfAdmin.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private EmployeesPage _employeesPage;
    private DailyWorkersPage _dailyWorkersPage;
    private BatchesPage _batchesPage;
    private IssuesPage _issuesPage;

    public MainWindow()
    {
        InitializeComponent();
        _employeesPage = new EmployeesPage();
        MainFrame.Navigate(_employeesPage); // Default page
    }

    private void btnEmployees_Click(object sender, RoutedEventArgs e)
    {
        _employeesPage ??= new EmployeesPage();
        MainFrame.Navigate(_employeesPage);
    }

    private void btnDistributors_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new DistributorsPage());
    }

    private void btnDailyWorkers_Click(object sender, RoutedEventArgs e)
    {
        _dailyWorkersPage ??= new DailyWorkersPage();
        MainFrame.Navigate(_dailyWorkersPage);
    }

    private void btnPieceRateWorkers_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new PieceRateWorkersPage());
    }

    private void btnBatches_Click(object sender, RoutedEventArgs e)
    {
        _batchesPage ??= new BatchesPage();
        MainFrame.Navigate(_batchesPage);
    }

    private void btnProductCategories_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ProductCategoriesPage());
    }

    private void btnProducts_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ProductsPage());
    }

    private void btnIssues_Click(object sender, RoutedEventArgs e)
    {
        _issuesPage ??= new IssuesPage();
        MainFrame.Navigate(_issuesPage);
    }

    private void btnLogout_Click(object sender, RoutedEventArgs e)
    {
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        this.Close();
    }
}