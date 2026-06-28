using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using chamcong.WpfAdmin.Models;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class AttendanceSheetWindow : Window
    {
        private readonly int _employeeId;
        private readonly ApiService _apiService;
        private bool _isLoaded = false;

        public AttendanceSheetWindow(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
            _apiService = new ApiService();

            // Populate Months & Years
            for (int i = 1; i <= 12; i++) cboMonth.Items.Add(i);
            for (int i = 2024; i <= 2030; i++) cboYear.Items.Add(i);

            cboMonth.SelectedItem = DateTime.Now.Month;
            cboYear.SelectedItem = DateTime.Now.Year;
            
            _isLoaded = true;
            LoadCalendar();
        }

        private void cboDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoaded) LoadCalendar();
        }

        private async void LoadCalendar()
        {
            int month = (int)cboMonth.SelectedItem;
            int year = (int)cboYear.SelectedItem;

            var data = await _apiService.GetAsync<List<AttendanceSheetDayModel>>($"/api/employees/{_employeeId}/attendance-sheet?month={month}&year={year}");
            
            if (data == null) return;

            // To make it look like a real calendar, we need to add empty cells before the 1st of the month
            var firstDayOfMonth = new DateTime(year, month, 1);
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            
            // Adjust Sunday (0) to 7 for Monday-starting calendar if needed, but standard WPF UniformGrid
            // We'll map Monday=1, Sunday=7. Standard DayOfWeek: Sun=0, Mon=1...
            // Let's assume the columns are: Mon, Tue, Wed, Thu, Fri, Sat, Sun
            int offset = startDayOfWeek == 0 ? 6 : startDayOfWeek - 1;

            var displayItems = new List<dynamic>();

            for (int i = 0; i < offset; i++)
            {
                displayItems.Add(new { DayText = "", CheckInText = "", CheckOutText = "", OvertimeText = "", IsWeekend = false });
            }

            foreach (var item in data)
            {
                displayItems.Add(item);
            }

            icCalendar.ItemsSource = displayItems;
        }
    }
}
