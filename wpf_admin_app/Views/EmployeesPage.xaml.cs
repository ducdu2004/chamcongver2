using System.Collections.Generic;
using System.Windows.Controls;
using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class EmployeesPage : Page
    {
        private readonly ApiService _apiService;

        public EmployeesPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadData();
        }

        private async void LoadData()
        {
            var employees = await _apiService.GetAsync<List<EmployeeSummaryModel>>("/api/employees");
            if (employees != null)
            {
                dgEmployees.ItemsSource = employees;
            }
        }
    }
}
