using AutoMapper;
using BusinessLogic.DTOs.DrugStorage;
using BusinessLogic.DTOs.Medication;
using BusinessObject.Entity;
using DataAccess;
using DataAccess.Repo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(
            IUnitOfWorks unitOfWorks,
            IMapper mapper,
            ILogger<InventoryService> logger)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
            _logger = logger;
        }

        #region Drug Management

        public async Task<IEnumerable<DrugStorageDto>> GetAllDrugsAsync()
        {
            try
            {
                var drugs = await _unitOfWorks.InventoryRepository.GetAllAsync("Nurse");
                var drugDtos = _mapper.Map<IEnumerable<DrugStorageDto>>(drugs);

                // Enrich with calculated properties
                return drugs.Select(d =>
                {
                    var dto = drugDtos.First(x => x.Id == d.Id); 
                    dto.ManagedByName = d.Nurse?.FullName ?? "Unknown";
                    return EnrichDrugDto(dto); 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all drugs");
                throw;
            }
        }

        public async Task<DrugStorageDto> GetDrugByIdAsync(int id)
        {
            try
            {
                var drug = await _unitOfWorks.InventoryRepository.GetByIdAsync(id);
                if (drug == null) return null;

                var drugDto = _mapper.Map<DrugStorageDto>(drug);
                return EnrichDrugDto(drugDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving drug with ID {DrugId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<DrugStorageDto>> SearchDrugsAsync(string searchTerm)
        {
            try
            {
                var drugs = await _unitOfWorks.InventoryRepository.GetDrugsByNameAsync(searchTerm);
                var drugDtos = _mapper.Map<IEnumerable<DrugStorageDto>>(drugs);

                return drugDtos.Select(dto => EnrichDrugDto(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching drugs with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<DrugStorageDto> AddDrugAsync(CreateDrugStorageDto drugDto)
        {
            try
            {
                var drug = _mapper.Map<DrugStorage>(drugDto);
                var addedDrug = await _unitOfWorks.InventoryRepository.AddDrugAsync(drug);

                _logger.LogInformation("Drug {MedicationName} added to inventory", drugDto.MedicationName);

                var resultDto = _mapper.Map<DrugStorageDto>(addedDrug);
                return EnrichDrugDto(resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding drug {MedicationName}", drugDto.MedicationName);
                throw;
            }
        }

        public async Task<DrugStorageDto> UpdateDrugAsync(int id, UpdateDrugStorageDto drugDto)
        {
            try
            {
                var existingDrug = await _unitOfWorks.InventoryRepository.GetByIdAsync(id);
                if (existingDrug == null)
                {
                    throw new ArgumentException($"Drug with ID {id} not found");
                }

                // Update only provided fields
                _mapper.Map(drugDto, existingDrug);

                var updatedDrug = await _unitOfWorks.InventoryRepository.UpdateDrugAsync(existingDrug);

                _logger.LogInformation("Drug {MedicationName} updated", updatedDrug.MedicationName);

                var resultDto = _mapper.Map<DrugStorageDto>(updatedDrug);
                return EnrichDrugDto(resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating drug with ID {DrugId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteDrugAsync(int id)
        {
            try
            {
                var drug = await _unitOfWorks.InventoryRepository.GetByIdAsync(id);
                if (drug == null) return false;

                var result = await _unitOfWorks.InventoryRepository.DeleteDrugAsync(id);

                if (result)
                {
                    _logger.LogInformation("Drug {MedicationName} deleted from inventory", drug.MedicationName);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting drug with ID {DrugId}", id);
                throw;
            }
        }

        #endregion

        #region Stock Management

        public async Task<bool> UpdateStockAsync(UpdateStockDto updateStockDto)
        {
            try
            {
                var drug = await _unitOfWorks.InventoryRepository.GetByIdAsync(updateStockDto.DrugId);
                if (drug == null)
                {
                    throw new ArgumentException($"Drug with ID {updateStockDto.DrugId} not found");
                }

                var result = await _unitOfWorks.InventoryRepository.UpdateStockQuantityAsync(
                    updateStockDto.DrugId,
                    updateStockDto.NewQuantity);

                if (result)
                {
                    _logger.LogInformation("Stock updated for {MedicationName}: {OldQuantity} -> {NewQuantity}. Reason: {Reason}",
                        drug.MedicationName, drug.Quantity, updateStockDto.NewQuantity, updateStockDto.Reason);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock for drug ID {DrugId}", updateStockDto.DrugId);
                throw;
            }
        }

        public async Task<bool> DispenseMedicationAsync(DispenseMedicationDto dispenseDto)
        {
            try
            {
                // Check if enough stock is available
                var isAvailable = await IsDrugAvailableForDispenseAsync(dispenseDto.DrugId, dispenseDto.Quantity);
                if (!isAvailable)
                {
                    throw new InvalidOperationException("Insufficient stock for dispensing");
                }

                // Reduce stock
                var stockReduced = await _unitOfWorks.InventoryRepository.ReduceStockAsync(dispenseDto.DrugId, dispenseDto.Quantity);
                if (!stockReduced)
                {
                    throw new InvalidOperationException("Failed to reduce stock");
                }

                // Record medication administration
                var medication = new Medication
                {
                    StudentId = dispenseDto.StudentId,
                    MedicationName = await GetMedicationNameAsync(dispenseDto.DrugId),
                    Dosage = dispenseDto.Dosage,
                    Frequency = dispenseDto.Frequency,
                    GivenBy = dispenseDto.AdministeredBy
                };

                // Note: You would need to add a method to save medication records
                // await _medicationRepository.AddMedicationAsync(medication);

                _logger.LogInformation("Medication dispensed: {MedicationName} ({Quantity}) to student ID {StudentId}",
                    medication.MedicationName, dispenseDto.Quantity, dispenseDto.StudentId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error dispensing medication for drug ID {DrugId}", dispenseDto.DrugId);
                throw;
            }
        }

        public async Task<bool> ReceiveStockAsync(ReceiveStockDto receiveStockDto)
        {
            try
            {
                var drug = await _unitOfWorks.InventoryRepository.GetByIdAsync(receiveStockDto.DrugId);
                if (drug == null)
                {
                    throw new ArgumentException($"Drug with ID {receiveStockDto.DrugId} not found");
                }

                // Update expiration date if provided
                if (receiveStockDto.ExpirationDate.HasValue && receiveStockDto.ExpirationDate > drug.ExpirationDate)
                {
                    drug.ExpirationDate = receiveStockDto.ExpirationDate.Value;
                    await _unitOfWorks.InventoryRepository.UpdateDrugAsync(drug);
                }

                // Add to stock
                var result = await _unitOfWorks.InventoryRepository.AddStockAsync(receiveStockDto.DrugId, receiveStockDto.Quantity);

                if (result)
                {
                    _logger.LogInformation("Stock received for {MedicationName}: +{Quantity}. Batch: {BatchNumber}",
                        drug.MedicationName, receiveStockDto.Quantity, receiveStockDto.BatchNumber);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error receiving stock for drug ID {DrugId}", receiveStockDto.DrugId);
                throw;
            }
        }

        #endregion

        #region Inventory Monitoring

        public async Task<IEnumerable<DrugStorageDto>> GetExpiredDrugsAsync()
        {
            try
            {
                var expiredDrugs = await _unitOfWorks.InventoryRepository.GetExpiredDrugsAsync();
                var drugDtos = _mapper.Map<IEnumerable<DrugStorageDto>>(expiredDrugs);
                return drugDtos.Select(dto => EnrichDrugDto(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expired drugs");
                throw;
            }
        }

        public async Task<IEnumerable<DrugStorageDto>> GetExpiringDrugsAsync(int daysAhead = 30)
        {
            try
            {
                var expiringDrugs = await _unitOfWorks.InventoryRepository.GetDrugsExpiringInDaysAsync(daysAhead);
                var drugDtos = _mapper.Map<IEnumerable<DrugStorageDto>>(expiringDrugs);
                return drugDtos.Select(dto => EnrichDrugDto(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expiring drugs");
                throw;
            }
        }

        public async Task<IEnumerable<DrugStorageDto>> GetLowStockDrugsAsync(int threshold = 10)
        {
            try
            {
                var lowStockDrugs = await _unitOfWorks.InventoryRepository.GetLowStockDrugsAsync(threshold);
                var drugDtos = _mapper.Map<IEnumerable<DrugStorageDto>>(lowStockDrugs);
                return drugDtos.Select(dto => EnrichDrugDto(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving low stock drugs");
                throw;
            }
        }

        public async Task<IEnumerable<DrugStorageDto>> GetDrugsByLocationAsync(string location)
        {
            try
            {
                var drugs = await _unitOfWorks.InventoryRepository.GetInventoryByLocationAsync(location);
                var drugDtos = _mapper.Map<IEnumerable<DrugStorageDto>>(drugs);
                return drugDtos.Select(dto => EnrichDrugDto(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving drugs by location {Location}", location);
                throw;
            }
        }

        public async Task<IEnumerable<DrugStorageDto>> GetDrugsByNurseAsync(int nurseId)
        {
            try
            {
                var drugs = await _unitOfWorks.InventoryRepository.GetDrugsByNurseAsync(nurseId);
                var drugDtos = _mapper.Map<IEnumerable<DrugStorageDto>>(drugs);
                return drugDtos.Select(dto => EnrichDrugDto(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving drugs by nurse ID {NurseId}", nurseId);
                throw;
            }
        }

        #endregion

        #region Reports and Analytics

        public async Task<InventoryReportDto> GenerateInventoryReportAsync()
        {
            try
            {
                var report = await _unitOfWorks.InventoryRepository.GenerateInventoryReportAsync();
                var reportDto = _mapper.Map<InventoryReportDto>(report);

                // Enrich with additional analytics
                var allDrugs = await _unitOfWorks.InventoryRepository.GetAllAsync();
                reportDto.LocationSummary = allDrugs
                    .GroupBy(d => d.StorageLocation ?? "Unknown")
                    .ToDictionary(g => g.Key, g => g.Count());

                reportDto.DosageFormSummary = allDrugs
                    .GroupBy(d => d.DosageForm ?? "Unknown")
                    .ToDictionary(g => g.Key, g => g.Count());

                return reportDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating inventory report");
                throw;
            }
        }

        public async Task<IEnumerable<StockMovementDto>> GetStockMovementHistoryAsync(int drugId)
        {
            try
            {
                // This would require a stock movement history table
                // For now, returning empty list as placeholder
                return new List<StockMovementDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stock movement history for drug ID {DrugId}", drugId);
                throw;
            }
        }

        #endregion

        #region Alerts and Notifications

        public async Task<List<InventoryAlertDto>> GetInventoryAlertsAsync()
        {
            try
            {
                var alerts = await _unitOfWorks.InventoryRepository.GetInventoryAlertsAsync();
                return _mapper.Map<List<InventoryAlertDto>>(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inventory alerts");
                throw;
            }
        }

        public async Task<bool> MarkAlertAsReadAsync(int alertId)
        {
            try
            {
                return await _unitOfWorks.InventoryRepository.MarkAlertAsReadAsync(alertId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking alert as read {AlertId}", alertId);
                throw;
            }
        }

        public async Task<int> GetPendingAlertsCountAsync()
        {
            try
            {
                var alerts = await _unitOfWorks.InventoryRepository.GetInventoryAlertsAsync();
                return alerts.Count(a => !a.IsRead);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending alerts count");
                throw;
            }
        }

        #endregion

        #region Validation and Business Logic

        
        public async Task<bool> IsDrugAvailableForDispenseAsync(int drugId, int quantity)
        {
            try
            {
                var drug = await _unitOfWorks.InventoryRepository.GetByIdAsync(drugId);
                if (drug == null) return false;

                return drug.Quantity >= quantity && !drug.IsExpired;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking drug availability for drug ID {DrugId}", drugId);
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetStorageLocationsAsync()
        {
            try
            {
                var drugs = await _unitOfWorks.InventoryRepository.GetAllAsync();
                return drugs.Select(d => d.StorageLocation)
                           .Where(l => !string.IsNullOrEmpty(l))
                           .Distinct()
                           .OrderBy(l => l);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving storage locations");
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetMedicationNamesAsync()
        {
            try
            {
                var drugs = await _unitOfWorks.InventoryRepository.GetAllAsync();
                return drugs.Select(d => d.MedicationName)
                           .Distinct()
                           .OrderBy(n => n);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medication names");
                throw;
            }
        }

        #endregion

        #region Private Helper Methods

        private DrugStorageDto EnrichDrugDto(DrugStorageDto dto)
        {
            dto.IsExpired = dto.ExpirationDate < DateTime.Now;
            dto.IsExpiringSoon = dto.ExpirationDate <= DateTime.Now.AddDays(30);
            dto.DaysUntilExpiration = (dto.ExpirationDate - DateTime.Now).Days;
            dto.IsLowStock = dto.Quantity <= 10; // This could be configurable

            // Set status based on conditions
            if (dto.IsExpired)
                dto.Status = "Expired";
            else if (dto.IsLowStock)
                dto.Status = "Low Stock";
            else if (dto.IsExpiringSoon)
                dto.Status = "Expiring Soon";
            else
                dto.Status = "Active";

            return dto;
        }

        private async Task<string> GetMedicationNameAsync(int drugId)
        {
            var drug = await _unitOfWorks.InventoryRepository.GetByIdAsync(drugId);
            return drug?.MedicationName ?? "Unknown";
        }

        #endregion
    }
}
