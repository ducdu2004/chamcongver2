using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class DailyWorkersPage : Page
    {
        private readonly ApiService _apiService;

        public DailyWorkersPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadData();
        }

        private async void LoadData()
        {
            var workers = await _apiService.GetAsync<List<DailyWorkerModel>>("/api/employees/daily-workers");
            if (workers != null)
            {
                dgDailyWorkers.ItemsSource = workers;
            }
        }

        private void btnViewAttendance_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.CommandParameter != null)
            {
                int employeeId = (int)button.CommandParameter;
                var attendanceWindow = new AttendanceSheetWindow(employeeId);
                attendanceWindow.Show();
            }
        }
    }
}
