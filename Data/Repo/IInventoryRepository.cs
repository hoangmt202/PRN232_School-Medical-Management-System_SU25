using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public interface IInventoryRepository : IGenericRepository<DrugStorage>
    {
        Task<IEnumerable<DrugStorage>> GetDrugsByNameAsync(string medicationName);
        Task<IEnumerable<DrugStorage>> GetExpiredDrugsAsync();
        Task<IEnumerable<DrugStorage>> GetDrugsExpiringInDaysAsync(int days);
        Task<IEnumerable<DrugStorage>> GetLowStockDrugsAsync(int threshold);
        Task<IEnumerable<DrugStorage>> GetDrugsByNurseAsync(int nurseId);
        Task<DrugStorage> AddDrugAsync(DrugStorage drug);
        Task<DrugStorage> UpdateDrugAsync(DrugStorage drug);
        Task<bool> DeleteDrugAsync(int id);

        // Stock Management
        Task<bool> UpdateStockQuantityAsync(int drugId, int newQuantity);
        Task<bool> ReduceStockAsync(int drugId, int quantity);
        Task<bool> AddStockAsync(int drugId, int quantity);

        // Medication Usage Tracking
        Task<IEnumerable<Medication>> GetMedicationUsageAsync(int drugId);

        // Inventory Reports
        Task<InventoryReport> GenerateInventoryReportAsync();
        Task<IEnumerable<DrugStorage>> GetInventoryByLocationAsync(string location);
        Task<decimal> CalculateTotalInventoryValueAsync();

        // Notifications and Alerts
        Task<IEnumerable<InventoryAlert>> GetInventoryAlertsAsync();
        Task<bool> MarkAlertAsReadAsync(int alertId);
    }
}
