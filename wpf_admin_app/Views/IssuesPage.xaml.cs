using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class IssuesPage : Page
    {
        private readonly ApiService _apiService;

        public IssuesPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadData();
        }

        private async void LoadData()
        {
            var issues = await _apiService.GetAsync<List<IssueReportModel>>("/api/issuereports/pending");
            if (issues != null)
            {
                dgIssues.ItemsSource = issues;
            }
        }

        private async void btnResolve_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.CommandParameter != null)
            {
                int issueId = (int)button.CommandParameter;
                
                var result = MessageBox.Show("Xác nhận đã xử lý xong khiếu nại này (Sửa công/Đền bù)?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = await _apiService.PutAsync($"/api/issuereports/{issueId}/resolve", new { }); 
                    if (success)
                    {
                        MessageBox.Show("Đã xử lý xong!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData(); // Reload
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi xử lý.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
