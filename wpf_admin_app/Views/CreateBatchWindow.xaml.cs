using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class CreateBatchWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly BatchesPage _parent;

        public CreateBatchWindow(BatchesPage parent)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _parent = parent;
            
            lblInfo.Text = $"Ngày nhập: {DateTime.Now:dd/MM/yyyy} | Người nhập: Admin";
            LoadDistributors();
        }

        private async void LoadDistributors()
        {
            var distributors = await _apiService.GetAsync<List<ReferenceModel>>("/api/referencedata/distributors");
            if (distributors != null)
            {
                cbDistributors.ItemsSource = distributors;
            }
        }

        private async void cbDistributors_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbDistributors.SelectedValue != null)
            {
                int distributorId = (int)cbDistributors.SelectedValue;
                await LoadProductsByDistributor(distributorId);
            }
        }

        private async System.Threading.Tasks.Task LoadProductsByDistributor(int distributorId)
        {
            var products = await _apiService.GetAsync<List<ProductModel>>("/api/products");
            if (products != null)
            {
                cbProducts.ItemsSource = products.Where(p => p.DistributorId == distributorId).ToList();
            }
        }



        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbProducts.SelectedValue == null || string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Vui lòng điền đủ thông tin.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int productId = (int)cbProducts.SelectedValue;
            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Số lượng không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var requestData = new
            {
                ProductId = productId,
                Quantity = quantity,
                AssignedWorkshopId = (int?)null,
                ReceiverName = "Admin"
            };

            bool success = await _apiService.PostAsync("/api/batches", requestData);
            if (success)
            {
                MessageBox.Show("Thêm lô hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                _parent.LoadData();
                this.Close();
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
