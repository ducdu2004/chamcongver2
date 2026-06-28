using System.Windows;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class GenerateBundlesWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly int _batchId;

        public GenerateBundlesWindow(int batchId, string productName)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _batchId = batchId;

            lblBatchInfo.Text = $"Lô hàng: #{batchId} - {productName}";
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumBundles.Text) || 
                string.IsNullOrWhiteSpace(txtStepName.Text) || 
                string.IsNullOrWhiteSpace(txtStepPrice.Text) || 
                string.IsNullOrWhiteSpace(txtQtyPerBundle.Text))
            {
                MessageBox.Show("Vui lòng điền đủ thông tin.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtNumBundles.Text, out int numBundles) || 
                !decimal.TryParse(txtStepPrice.Text, out decimal stepPrice) || 
                !int.TryParse(txtQtyPerBundle.Text, out int qtyPerBundle))
            {
                MessageBox.Show("Dữ liệu số không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var requestData = new
            {
                BatchId = _batchId,
                NumberOfBundles = numBundles,
                StepName = txtStepName.Text,
                StepPrice = stepPrice,
                QuantityPerBundle = qtyPerBundle
            };

            bool success = await _apiService.PostAsync("/api/bundles/generate", requestData);
            if (success)
            {
                MessageBox.Show("Phát sinh bó hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
