using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class ProductsPage : Page
    {
        private readonly ApiService _apiService;

        public ProductsPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadProducts();
        }

        public async void LoadProducts()
        {
            await LoadDistributorsAsync();
            SearchProducts();
        }

        private async System.Threading.Tasks.Task LoadDistributorsAsync()
        {
            var distributors = await _apiService.GetAsync<List<DistributorModel>>("/api/distributors");
            if (distributors != null)
            {
                var list = new List<DistributorModel> { new DistributorModel { Id = 0, Name = "-- Tất cả Nhà phân phối --" } };
                list.AddRange(distributors);
                cbSearchDistributor.ItemsSource = list;
                cbSearchDistributor.SelectedIndex = 0;
            }
        }

        private async void SearchProducts()
        {
            var query = txtSearchProduct.Text.Trim();
            var endpoint = "/api/products";
            
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(query))
            {
                queryParams.Add($"query={System.Uri.EscapeDataString(query)}");
            }
            if (cbSearchDistributor.SelectedValue is int distributorId && distributorId > 0)
            {
                queryParams.Add($"distributorId={distributorId}");
            }

            if (queryParams.Count > 0)
            {
                endpoint += "?" + string.Join("&", queryParams);
            }

            var products = await _apiService.GetAsync<List<ProductModel>>(endpoint);
            if (products != null)
            {
                dgProducts.ItemsSource = products;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchProducts();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var window = new CreateProductWindow(this);
            window.ShowDialog();
        }
    }
}
