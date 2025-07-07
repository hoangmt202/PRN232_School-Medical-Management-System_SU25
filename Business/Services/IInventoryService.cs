using BusinessLogic.DTOs.DrugStorage;
using BusinessLogic.DTOs.Medication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<DrugStorageDto>> GetAllDrugsAsync();
        Task<DrugStorageDto> GetDrugByIdAsync(int id);
        Task<IEnumerable<DrugStorageDto>> SearchDrugsAsync(string searchTerm);
        Task<DrugStorageDto> AddDrugAsync(CreateDrugStorageDto drugDto);
        Task<DrugStorageDto> UpdateDrugAsync(int id, UpdateDrugStorageDto drugDto);
        Task<bool> DeleteDrugAsync(int id);

        // Stock Management
        Task<bool> UpdateStockAsync(UpdateStockDto updateStockDto);
        Task<bool> DispenseMedicationAsync(DispenseMedicationDto dispenseDto);
        Task<bool> ReceiveStockAsync(ReceiveStockDto receiveStockDto);

        // Inventory Monitoring
        Task<IEnumerable<DrugStorageDto>> GetExpiredDrugsAsync();
        Task<IEnumerable<DrugStorageDto>> GetExpiringDrugsAsync(int daysAhead = 30);
        Task<IEnumerable<DrugStorageDto>> GetLowStockDrugsAsync(int threshold = 10);
        Task<IEnumerable<DrugStorageDto>> GetDrugsByLocationAsync(string location);
        Task<IEnumerable<DrugStorageDto>> GetDrugsByNurseAsync(int nurseId);

        // Reports and Analytics
        Task<InventoryReportDto> GenerateInventoryReportAsync();
        Task<IEnumerable<StockMovementDto>> GetStockMovementHistoryAsync(int drugId);

        // Alerts and Notifications
        Task<List<InventoryAlertDto>> GetInventoryAlertsAsync();
        Task<bool> MarkAlertAsReadAsync(int alertId);
        Task<int> GetPendingAlertsCountAsync();

        // Validation and Business Logic
        Task<bool> IsDrugAvailableForDispenseAsync(int drugId, int quantity);
        Task<IEnumerable<string>> GetStorageLocationsAsync();
        Task<IEnumerable<string>> GetMedicationNamesAsync();
    }
}
