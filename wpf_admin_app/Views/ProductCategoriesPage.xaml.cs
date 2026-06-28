using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class ProductCategoriesPage : Page
    {
        private readonly ApiService _apiService;
        private ReferenceModel? _editingCategory = null;
        private GarmentPartModel? _editingPart = null;

        public ProductCategoriesPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadCategories();
        }

        private async void LoadCategories()
        {
            var categories = await _apiService.GetAsync<List<ReferenceModel>>("/api/productcategories");
            if (categories != null)
            {
                dgCategories.ItemsSource = categories;
            }
        }

        private async void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text)) return;

            if (_editingCategory == null)
            {
                bool success = await _apiService.PostAsync("/api/productcategories", new { Name = txtCategoryName.Text });
                if (success)
                {
                    txtCategoryName.Clear();
                    LoadCategories();
                }
            }
            else
            {
                bool success = await _apiService.PutAsync($"/api/productcategories/{_editingCategory.Id}", new { Name = txtCategoryName.Text });
                if (success)
                {
                    txtCategoryName.Clear();
                    _editingCategory = null;
                    btnAddCategory.Content = "Thêm mới";
                    LoadCategories();
                }
            }
        }

        private void dgCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCategories.SelectedItem is ReferenceModel category)
            {
                grpParts.Header = $"Bộ phận của '{category.Name}'";
                grpParts.IsEnabled = true;
                LoadParts(category.Id);
            }
            else
            {
                grpParts.IsEnabled = false;
                dgParts.ItemsSource = null;
            }
        }

        private async void LoadParts(int categoryId)
        {
            var parts = await _apiService.GetAsync<List<GarmentPartModel>>($"/api/garmentparts/category/{categoryId}");
            if (parts != null)
            {
                dgParts.ItemsSource = parts;
            }
        }

        private async void btnAddPart_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPartName.Text)) return;
            decimal unitPrice = 0;
            decimal.TryParse(txtPartUnitPrice.Text, out unitPrice);

            if (dgCategories.SelectedItem is ReferenceModel category)
            {
                var requestData = new
                {
                    Name = txtPartName.Text,
                    ProductCategoryId = category.Id,
                    DefaultUnitPrice = unitPrice
                };

                if (_editingPart == null)
                {
                    bool success = await _apiService.PostAsync("/api/garmentparts", requestData);
                    if (success)
                    {
                        txtPartName.Clear();
                        txtPartUnitPrice.Text = "0";
                        LoadParts(category.Id);
                    }
                }
                else
                {
                    bool success = await _apiService.PutAsync($"/api/garmentparts/{_editingPart.Id}", requestData);
                    if (success)
                    {
                        txtPartName.Clear();
                        txtPartUnitPrice.Text = "0";
                        _editingPart = null;
                        btnAddPart.Content = "Thêm Bộ phận";
                        LoadParts(category.Id);
                    }
                }
            }
        }

        private async void btnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ReferenceModel category)
            {
                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa danh mục '{category.Name}' không?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool success = await _apiService.DeleteAsync($"/api/productcategories/{category.Id}");
                    if (success)
                    {
                        LoadCategories();
                        grpParts.IsEnabled = false;
                        dgParts.ItemsSource = null;
                    }
                }
            }
        }

        private async void btnDeletePart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is GarmentPartModel part)
            {
                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa bộ phận '{part.Name}' không?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool success = await _apiService.DeleteAsync($"/api/garmentparts/{part.Id}");
                    if (success && dgCategories.SelectedItem is ReferenceModel category)
                    {
                        LoadParts(category.Id);
                    }
                }
            }
        }
        private void btnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ReferenceModel category)
            {
                _editingCategory = category;
                txtCategoryName.Text = category.Name;
                btnAddCategory.Content = "Cập nhật";
            }
        }

        private void btnEditPart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is GarmentPartModel part)
            {
                _editingPart = part;
                txtPartName.Text = part.Name;
                txtPartUnitPrice.Text = part.DefaultUnitPrice.ToString("0.##");
                btnAddPart.Content = "Cập nhật";
            }
        }
    }
}
