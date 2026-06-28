using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;
using System;
using System.Windows;

namespace chamcong.WpfAdmin.Views
{
    public partial class CreateDistributorWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly DistributorModel? _editingDistributor;

        public CreateDistributorWindow(DistributorModel? distributor = null)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _editingDistributor = distributor;

            if (_editingDistributor != null)
            {
                Title = "Cập nhật Nhà phân phối";
                txtName.Text = _editingDistributor.Name;
                txtPhone.Text = _editingDistributor.Phone;
                txtEmail.Text = _editingDistributor.Email;
                txtAddress.Text = _editingDistributor.Address;
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà phân phối.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dto = new
            {
                Name = txtName.Text.Trim(),
                Phone = txtPhone.Text?.Trim() ?? "",
                Email = txtEmail.Text?.Trim() ?? "",
                Address = txtAddress.Text?.Trim() ?? ""
            };

            try
            {
                bool success;
                if (_editingDistributor == null)
                {
                    success = await _apiService.PostAsync("/api/distributors", dto);
                }
                else
                {
                    success = await _apiService.PutAsync($"/api/distributors/{_editingDistributor.Id}", dto);
                }

                if (success)
                {
                    MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Lỗi khi lưu dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
