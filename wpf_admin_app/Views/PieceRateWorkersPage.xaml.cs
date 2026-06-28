using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace chamcong.WpfAdmin.Views
{
    public partial class PieceRateWorkersPage : Page
    {
        private readonly ApiService _apiService;
        private List<EmployeeSummaryModel> _allEmployees = new();
        private List<ProductModel> _allProducts = new();
        private List<GarmentPartModel> _allGarmentParts = new(); // Assuming a model for GarmentPart

        public PieceRateWorkersPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbGarmentParts.SelectionChanged += CbGarmentParts_SelectionChanged;
            await LoadEmployeesAsync();
            await LoadProductsAsync();
            await LoadGarmentPartsAsync();
        }

        private async Task LoadEmployeesAsync()
        {
            try
            {
                var employees = await _apiService.GetAsync<List<EmployeeSummaryModel>>("/api/employees");
                if (employees != null)
                {
                    _allEmployees = employees;
                    dgEmployees.ItemsSource = _allEmployees;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách nhân viên: {ex.Message}");
            }
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var products = await _apiService.GetAsync<List<ProductModel>>("/api/products");
                if (products != null)
                {
                    _allProducts = products;
                    cbProducts.ItemsSource = _allProducts;
                }
            }
            catch (Exception)
            {
                // Ignore
            }
        }

        private async Task LoadGarmentPartsAsync()
        {
            try
            {
                var parts = await _apiService.GetAsync<List<GarmentPartModel>>("/api/garmentparts");
                if (parts != null)
                {
                    _allGarmentParts = parts;
                }
            }
            catch (Exception ex)
            {
                // Ignore or log
            }
        }

        private void btnSearchEmployee_Click(object sender, RoutedEventArgs e)
        {
            var keyword = txtSearchEmployee.Text.ToLower();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                dgEmployees.ItemsSource = _allEmployees;
            }
            else
            {
                dgEmployees.ItemsSource = _allEmployees.Where(emp => 
                    emp.FullName.ToLower().Contains(keyword) || 
                    (emp.Phone != null && emp.Phone.Contains(keyword)) ||
                    emp.Id.ToString() == keyword).ToList();
            }
        }

        private async void dgEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEmployees.SelectedItem is EmployeeSummaryModel selectedEmp)
            {
                txtSelectedEmployee.Text = $"Đang chọn: {selectedEmp.FullName} (Mã: {selectedEmp.Id})";
                await LoadProductionLogsAsync(selectedEmp.Id);
            }
            else
            {
                txtSelectedEmployee.Text = "Vui lòng chọn nhân viên bên trái...";
                dgProductionLogs.ItemsSource = null;
            }
        }

        private async void cbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProducts.SelectedItem is ProductModel selectedProduct)
            {
                if (selectedProduct.Components != null && selectedProduct.Components.Count > 0)
                {
                    // Filter based on configured components
                    var configuredPartIds = selectedProduct.Components.Select(c => c.GarmentPartId).ToList();
                    cbGarmentParts.ItemsSource = _allGarmentParts.Where(p => configuredPartIds.Contains(p.Id)).ToList();
                }
                else if (selectedProduct.ProductCategoryId.HasValue)
                {
                    // Fallback to category parts if no specific components configured
                    cbGarmentParts.ItemsSource = _allGarmentParts.Where(p => p.ProductCategoryId == selectedProduct.ProductCategoryId.Value).ToList();
                }
                else
                {
                    cbGarmentParts.ItemsSource = _allGarmentParts;
                }
            }
        }

        private void CbGarmentParts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbGarmentParts.SelectedItem is GarmentPartModel selectedPart)
            {
                txtUnitPrice.Text = selectedPart.DefaultUnitPrice.ToString("0.##");
            }
        }

        private async Task LoadProductionLogsAsync(int employeeId)
        {
            try
            {
                var logs = await _apiService.GetAsync<List<ManualProductionDto>>($"/api/production/manual/{employeeId}?date={DateTime.Now:yyyy-MM-dd}");
                if (logs != null)
                {
                    dgProductionLogs.ItemsSource = logs;
                }
            }
            catch (Exception ex)
            {
                // Ignore
            }
        }

        private async void btnSaveProduction_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployees.SelectedItem is not EmployeeSummaryModel selectedEmp)
            {
                MessageBox.Show("Vui lòng chọn nhân viên trước!");
                return;
            }

            if (cbProducts.SelectedValue is not int productId)
            {
                MessageBox.Show("Vui lòng chọn Sản phẩm!");
                return;
            }

            if (cbGarmentParts.SelectedValue is not int partId)
            {
                MessageBox.Show("Vui lòng chọn Bộ phận!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSizeOrTable.Text))
            {
                MessageBox.Show("Vui lòng nhập Size hoặc Bàn!");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên lớn hơn 0!");
                return;
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) || unitPrice < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ!");
                return;
            }

            var dto = new ManualProductionCreateDto
            {
                EmployeeId = selectedEmp.Id,
                ProductId = productId,
                GarmentPartId = partId,
                SizeOrTable = txtSizeOrTable.Text.Trim(),
                Quantity = quantity,
                UnitPrice = unitPrice
            };

            try
            {
                var success = await _apiService.PostAsync("/api/production/manual", dto);
                if (success)
                {
                    MessageBox.Show("Ghi nhận thành công!");
                    txtQuantity.Clear();
                    // txtSizeOrTable.Clear(); // maybe keep it if they are typing multiple sizes
                    await LoadProductionLogsAsync(selectedEmp.Id);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi lưu dữ liệu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
