using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class InventoryRepository : GenericRepository<DrugStorage>, IInventoryRepository
    {
        public InventoryRepository(SchoolMedicalDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<DrugStorage>> GetDrugsByNameAsync(string medicationName)
        {
            return await _context.DrugStorages
                .Include(d => d.Nurse)
                .Where(d => d.MedicationName.Contains(medicationName))
                .ToListAsync();
        }

        public async Task<IEnumerable<DrugStorage>> GetExpiredDrugsAsync()
        {
            var currentDate = DateTime.Now;
            return await _context.DrugStorages
                .Include(d => d.Nurse)
                .Where(d => d.ExpirationDate < currentDate)
                .OrderBy(d => d.ExpirationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DrugStorage>> GetDrugsExpiringInDaysAsync(int days)
        {
            var targetDate = DateTime.Now.AddDays(days);
            return await _context.DrugStorages
                .Include(d => d.Nurse)
                .Where(d => d.ExpirationDate <= targetDate && d.ExpirationDate >= DateTime.Now)
                .OrderBy(d => d.ExpirationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DrugStorage>> GetLowStockDrugsAsync(int threshold)
        {
            return await _context.DrugStorages
                .Include(d => d.Nurse)
                .Where(d => d.Quantity <= threshold)
                .OrderBy(d => d.Quantity)
                .ToListAsync();
        }

        public async Task<IEnumerable<DrugStorage>> GetDrugsByNurseAsync(int nurseId)
        {
            return await _context.DrugStorages
                .Include(d => d.Nurse)
                .Where(d => d.ManagedBy == nurseId)
                .OrderBy(d => d.MedicationName)
                .ToListAsync();
        }

        public async Task<DrugStorage> AddDrugAsync(DrugStorage drug)
        {
            drug.DateReceived = DateTime.Now;
            _context.DrugStorages.Add(drug);
            await _context.SaveChangesAsync();
            return drug;
        }

        public async Task<DrugStorage> UpdateDrugAsync(DrugStorage drug)
        {
            _context.DrugStorages.Update(drug);
            await _context.SaveChangesAsync();
            return drug;
        }

        public async Task<bool> DeleteDrugAsync(int id)
        {
            var drug = await _context.DrugStorages.FindAsync(id);
            if (drug == null) return false;

            _context.DrugStorages.Remove(drug);
            await _context.SaveChangesAsync();
            return true;
        }

        #region Stock Management

        public async Task<bool> UpdateStockQuantityAsync(int drugId, int newQuantity)
        {
            var drug = await _context.DrugStorages.FindAsync(drugId);
            if (drug == null) return false;

            drug.Quantity = newQuantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReduceStockAsync(int drugId, int quantity)
        {
            var drug = await _context.DrugStorages.FindAsync(drugId);
            if (drug == null || drug.Quantity < quantity) return false;

            drug.Quantity -= quantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddStockAsync(int drugId, int quantity)
        {
            var drug = await _context.DrugStorages.FindAsync(drugId);
            if (drug == null) return false;

            drug.Quantity += quantity;
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Medication Usage Tracking

        public async Task<IEnumerable<Medication>> GetMedicationUsageAsync(int drugId)
        {
            var drugName = await _context.DrugStorages
                .Where(d => d.Id == drugId)
                .Select(d => d.MedicationName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(drugName)) return new List<Medication>();

            return await _context.Medications
                .Include(m => m.Student)
                .Include(m => m.Nurse)
                .Where(m => m.MedicationName == drugName)
                .ToListAsync();
        }

        #endregion

        #region Inventory Reports

        public async Task<InventoryReport> GenerateInventoryReportAsync()
        {
            var currentDate = DateTime.Now;
            var expiringDate = currentDate.AddDays(30);
            var lowStockThreshold = 10; // This could be configurable

            var allDrugs = await _context.DrugStorages.Include(d => d.Nurse).ToListAsync();
            var expiredDrugs = allDrugs.Where(d => d.ExpirationDate < currentDate).ToList();
            var expiringDrugs = allDrugs.Where(d => d.ExpirationDate <= expiringDate && d.ExpirationDate >= currentDate).ToList();
            var lowStockDrugs = allDrugs.Where(d => d.Quantity <= lowStockThreshold).ToList();

            return new InventoryReport
            {
                TotalItems = allDrugs.Count,
                ExpiredItems = expiredDrugs.Count,
                ExpiringItems = expiringDrugs.Count,
                LowStockItems = lowStockDrugs.Count,
                TotalValue = 0, // Would need price information to calculate
                GeneratedAt = currentDate,
                ExpiredDrugs = expiredDrugs,
                ExpiringDrugs = expiringDrugs,
                LowStockDrugs = lowStockDrugs
            };
        }

        public async Task<IEnumerable<DrugStorage>> GetInventoryByLocationAsync(string location)
        {
            return await _context.DrugStorages
                .Include(d => d.Nurse)
                .Where(d => d.StorageLocation.Contains(location))
                .OrderBy(d => d.MedicationName)
                .ToListAsync();
        }

        public async Task<decimal> CalculateTotalInventoryValueAsync()
        {
            // This would require price information in the database
            // For now, returning 0 as placeholder
            return 0;
        }

        #endregion

        #region Notifications and Alerts

        public async Task<IEnumerable<InventoryAlert>> GetInventoryAlertsAsync()
        {
            var alerts = new List<InventoryAlert>();
            var currentDate = DateTime.Now;
            var expiringDate = currentDate.AddDays(30);
            var lowStockThreshold = 10;

            // Get expired drugs
            var expiredDrugs = await _context.DrugStorages
                .Include(d => d.Nurse)
                .Where(d => d.ExpirationDate < currentDate)
                .ToListAsync();

            foreach (var drug in expiredDrugs)
            {
                alerts.Add(new InventoryAlert
                {
                    AlertType = "EXPIRED",
                    Message = $"{drug.MedicationName} has expired on {drug.ExpirationDate:yyyy-MM-dd}",
                    DrugId = drug.Id,
                    CreatedAt = currentDate,
                    Drug = drug,
                    IsRead = false
                });
            }

            // Get expiring drugs
            var expiringDrugs = await _context.DrugStorages
                .Include(d => d.Nurse)
                .Where(d => d.ExpirationDate <= expiringDate && d.ExpirationDate >= currentDate)
                .ToListAsync();

            foreach (var drug in expiringDrugs)
            {
                alerts.Add(new InventoryAlert
                {
                    AlertType = "EXPIRING",
                    Message = $"{drug.MedicationName} will expire on {drug.ExpirationDate:yyyy-MM-dd}",
                    DrugId = drug.Id,
                    CreatedAt = currentDate,
                    Drug = drug,
                    IsRead = false
                });
            }

            // Get low stock drugs
            var lowStockDrugs = await _context.DrugStorages
                .Include(d => d.Nurse)
                .Where(d => d.Quantity <= lowStockThreshold)
                .ToListAsync();

            foreach (var drug in lowStockDrugs)
            {
                alerts.Add(new InventoryAlert
                {
                    AlertType = "LOW_STOCK",
                    Message = $"{drug.MedicationName} is running low (Quantity: {drug.Quantity})",
                    DrugId = drug.Id,
                    CreatedAt = currentDate,
                    Drug = drug,
                    IsRead = false
                });
            }

            return alerts.OrderByDescending(a => a.CreatedAt).ToList();
        }

        public async Task<bool> MarkAlertAsReadAsync(int alertId)
        {
            // This would require an alerts table to track read status
            // For now, returning true as placeholder
            return true;
        }

        #endregion
    }
}

