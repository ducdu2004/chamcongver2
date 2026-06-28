using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace chamcong.WpfAdmin.Views
{
    public partial class DistributorsPage : Page
    {
        private readonly ApiService _apiService;

        public DistributorsPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadDataAsync();
        }

        private async void LoadDataAsync(string query = "")
        {
            try
            {
                var endpoint = "/api/distributors";
                if (!string.IsNullOrEmpty(query))
                {
                    endpoint += $"?query={Uri.EscapeDataString(query)}";
                }

                var list = await _apiService.GetAsync<List<DistributorModel>>(endpoint);
                dgDistributors.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadDataAsync(txtSearch.Text);
        }

        private void btnAddDistributor_Click(object sender, RoutedEventArgs e)
        {
            var window = new CreateDistributorWindow();
            if (window.ShowDialog() == true)
            {
                LoadDataAsync();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is DistributorModel item)
            {
                var window = new CreateDistributorWindow(item);
                if (window.ShowDialog() == true)
                {
                    LoadDataAsync();
                }
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is DistributorModel item)
            {
                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa '{item.Name}'?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var req = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"{ApiService.BaseUrl}/api/distributors/{item.Id}");
                        if (!string.IsNullOrEmpty(ApiService.Token))
                        {
                            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ApiService.Token);
                        }

                        using var client = new System.Net.Http.HttpClient();
                        var res = await client.SendAsync(req);
                        if (res.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadDataAsync();
                        }
                        else
                        {
                            var err = await res.Content.ReadAsStringAsync();
                            MessageBox.Show($"Lỗi xóa: {err}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
