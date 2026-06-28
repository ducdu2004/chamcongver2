using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class BatchesPage : Page
    {
        private readonly ApiService _apiService;

        public BatchesPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadData();
        }

        public async void LoadData()
        {
            var batches = await _apiService.GetAsync<List<BatchModel>>("/api/batches");
            if (batches != null)
            {
                dgRootBatches.ItemsSource = batches.Where(b => b.ParentBatchId == null).ToList();
                dgSubBatches.ItemsSource = batches.Where(b => b.ParentBatchId != null).ToList();
            }
        }

        private void dgBatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid currentGrid && currentGrid.SelectedItem != null)
            {
                if (currentGrid == dgRootBatches)
                {
                    dgSubBatches.SelectedItem = null;
                }
                else if (currentGrid == dgSubBatches)
                {
                    dgRootBatches.SelectedItem = null;
                }
            }
        }

        private void btnViewEmployees_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.CommandParameter != null)
            {
                int batchId = (int)button.CommandParameter;
                var batchEmployeesWindow = new BatchEmployeesWindow(batchId);
                batchEmployeesWindow.Show();
            }
        }

        private void btnAddBatch_Click(object sender, RoutedEventArgs e)
        {
            var window = new CreateBatchWindow(this);
            window.ShowDialog();
        }

        private void btnCreateSubBatch_Click(object sender, RoutedEventArgs e)
        {
            var selectedBatch = dgRootBatches.SelectedItem as BatchModel ?? dgSubBatches.SelectedItem as BatchModel;
            if (selectedBatch == null)
            {
                MessageBox.Show("Vui lòng chọn một Lô hàng gốc từ danh sách.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var window = new CreateSubBatchWindow(this, selectedBatch.Id, selectedBatch.ProductName, selectedBatch.Quantity);
            window.ShowDialog();
        }

        private void btnGenerateBundles_Click(object sender, RoutedEventArgs e)
        {
            var selectedBatch = dgRootBatches.SelectedItem as BatchModel ?? dgSubBatches.SelectedItem as BatchModel;
            if (selectedBatch == null)
            {
                MessageBox.Show("Vui lòng chọn một Lô hàng từ danh sách.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var window = new GenerateBundlesWindow(selectedBatch.Id, selectedBatch.ProductName);
            window.ShowDialog();
        }
    }
}
