using System.Collections.Generic;
using System.Windows;
using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class CreateSubBatchWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly BatchesPage _parent;
        private readonly int _parentBatchId;

        public CreateSubBatchWindow(BatchesPage parent, int parentBatchId, string productName, int availableQuantity)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _parent = parent;
            _parentBatchId = parentBatchId;

            lblBatchInfo.Text = $"Cắt từ Lô: #{parentBatchId} - {productName} (Tối đa: {availableQuantity})";
            LoadWorkshops();
        }

        private async void LoadWorkshops()
        {
            var workshops = await _apiService.GetAsync<List<ReferenceModel>>("/api/referencedata/workshops");
            if (workshops != null)
            {
                cbWorkshops.ItemsSource = workshops;
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbWorkshops.SelectedValue == null || string.IsNullOrWhiteSpace(txtQuantity.Text) || string.IsNullOrWhiteSpace(txtUnitPrice.Text))
            {
                MessageBox.Show("Vui lòng điền đủ thông tin.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || !decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice))
            {
                MessageBox.Show("Dữ liệu không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var requestData = new
            {
                ParentBatchId = _parentBatchId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                AssignedWorkshopId = (int)cbWorkshops.SelectedValue
            };

            bool success = await _apiService.PostAsync("/api/batches/sub-batch", requestData);
            if (success)
            {
                MessageBox.Show("Tạo lô con thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                _parent.LoadData();
                this.Close();
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra hoặc số lượng vượt quá cho phép.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
