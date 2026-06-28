using System.Collections.Generic;
using System.Windows;
using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class BatchEmployeesWindow : Window
    {
        private readonly int _batchId;
        private readonly ApiService _apiService;

        public BatchEmployeesWindow(int batchId)
        {
            InitializeComponent();
            _batchId = batchId;
            _apiService = new ApiService();
            LoadData();
        }

        private async void LoadData()
        {
            var employees = await _apiService.GetAsync<List<BatchEmployeeModel>>($"/api/batches/{_batchId}/employees");
            if (employees != null)
            {
                dgBatchEmployees.ItemsSource = employees;
            }
        }
    }
}
