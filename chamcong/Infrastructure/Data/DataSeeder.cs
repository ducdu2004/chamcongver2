using chamcong.Domain.Entities;
using chamcong.Infrastructure.Data;

namespace chamcong.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static void SeedData(AppDbContext context)
        {
            if (!context.Workshops.Any())
            {
                var w1 = new Workshop { Name = "Xưởng Nhà Chính", IsInternal = true, Address = "Hà Nội" };
                var w2 = new Workshop { Name = "Thầu phụ A", IsInternal = false, Address = "Hà Nam" };
                var w3 = new Workshop { Name = "Thầu phụ B", IsInternal = false, Address = "Nam Định" };
                context.Workshops.AddRange(w1, w2, w3);
                context.SaveChanges();

                var p1 = new Product { ProductCode = "SHIRT-01", ProductName = "Áo Sơ mi Nam", DefaultUnitPrice = 20000 };
                var p2 = new Product { ProductCode = "PANT-01", ProductName = "Quần âu Nam", DefaultUnitPrice = 25000 };
                var p3 = new Product { ProductCode = "TSHIRT-01", ProductName = "Áo thun", DefaultUnitPrice = 10000 };
                var p4 = new Product { ProductCode = "JACKET-01", ProductName = "Áo khoác", DefaultUnitPrice = 50000 };
                var p5 = new Product { ProductCode = "SHIRT-02", ProductName = "Áo Sơ mi Nữ", DefaultUnitPrice = 22000 };
                context.Products.AddRange(p1, p2, p3, p4, p5);
                context.SaveChanges();

                var e1 = new Employee { FullName = "Nguyễn Văn A (Công nhật)", PayType = 1, WorkshopId = w1.Id, FingerprintId = "FINGER_001" };
                var e2 = new Employee { FullName = "Trần Thị B (Khoán)", PayType = 2, WorkshopId = w1.Id, FingerprintId = "FINGER_002" };
                var e3 = new Employee { FullName = "Lê Văn C (Thầu phụ)", PayType = 2, WorkshopId = w2.Id, FingerprintId = "FINGER_003" };
                for (int i = 4; i <= 10; i++)
                {
                    context.Employees.Add(new Employee { FullName = $"Thợ {i}", PayType = 2, WorkshopId = w1.Id, FingerprintId = $"FINGER_00{i}" });
                }
                context.Employees.AddRange(e1, e2, e3);
                context.SaveChanges();

                var batch1 = new Batch { ProductId = p1.Id, Quantity = 1000, UnitPrice = 20000, Status = 0, AssignedWorkshopId = w1.Id };
                var batch2 = new Batch { ProductId = p2.Id, Quantity = 500, UnitPrice = 25000, Status = 0, AssignedWorkshopId = w1.Id };
                context.Batches.AddRange(batch1, batch2);
                context.SaveChanges();

                // Create a sub batch
                var subBatch1 = new Batch { ProductId = p1.Id, ParentBatchId = batch1.Id, Quantity = 200, UnitPrice = 18000, Status = 0, AssignedWorkshopId = w2.Id };
                context.Batches.Add(subBatch1);
                context.SaveChanges();

                // Create Bundles
                for (int i = 1; i <= 30; i++)
                {
                    context.Bundles.Add(new Bundle { BatchId = batch1.Id, BundleNumber = i, StepName = "May Cổ Áo", StepPrice = 1000, Quantity = 10, Status = 0 });
                }
                context.SaveChanges();

                // Create Accounts
                if (!context.Accounts.Any())
                {
                    var adminAccount = new Account
                    {
                        Username = "admin",
                        PasswordHash = HashPassword("123456"),
                        Role = "Admin"
                    };

                    var employeeAccount = new Account
                    {
                        Username = "nvA",
                        PasswordHash = HashPassword("123456"),
                        Role = "Employee",
                        EmployeeId = e1.Id
                    };
                    
                    var thauPhuAccount = new Account
                    {
                        Username = "thauphuA",
                        PasswordHash = HashPassword("123456"),
                        Role = "Partner",
                        EmployeeId = e3.Id
                    };

                    context.Accounts.AddRange(adminAccount, employeeAccount, thauPhuAccount);
                    context.SaveChanges();
                }
            }
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
