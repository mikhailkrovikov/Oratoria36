using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence.Services
{
    public class SettingService : ISettingsService
    {
        private readonly AppDBContext _dbContext;
        private readonly ILogger<SettingService> _logger;

        public SettingService(AppDBContext dbContext, ILogger<SettingService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IReadOnlyList<DeviceSettingEntity> Load()
        {
            return _dbContext.DeviceSettings
                .AsNoTracking()
                .ToList();
        }

        public void Upsert(DeviceSettingEntity entity)
        {
            try
            {
                var existing = _dbContext.DeviceSettings.Find(entity.DeviceKey, entity.Name);
                if (existing is null)
                {
                    _dbContext.DeviceSettings.Add(entity);
                }
                else
                {
                    existing.Value = entity.Value;
                    existing.Min = entity.Min;
                    existing.Max = entity.Max;
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
            }
        }
    }
}
