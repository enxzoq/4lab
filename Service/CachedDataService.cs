using Microsoft.Extensions.Caching.Memory;
using Lab4.Data;
using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Service
{
    public class CachedDataService
    {
        private readonly TelecomContext _context;
        private readonly IMemoryCache _cache;
        private const int CacheDurationSeconds = 2 * 23 + 240;
        private const int RowCount = 20; // Ограничение на количество записей

        public CachedDataService(TelecomContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        // Универсальный метод для получения кэшированных данных
        private IEnumerable<T> GetOrSetCache<T>(string cacheKey, Func<IEnumerable<T>> dataRetrievalFunc)
        {
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<T> cachedData))
            {
                cachedData = dataRetrievalFunc();
                _cache.Set(cacheKey, cachedData, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(CacheDurationSeconds)
                });
            }
            return cachedData;
        }

        // Метод для получения 20 записей из таблицы Subscribers
        public IEnumerable<Subscriber> GetSubscribers()
        {
            return GetOrSetCache("Subscribers", () => _context.Subscribers
                .Take(RowCount)
                .ToList());
        }

        // Метод для получения 20 записей из таблицы TariffPlans
        public IEnumerable<TariffPlan> GetTariffPlans()
        {
            return GetOrSetCache("TariffPlans", () => _context.TariffPlans
                .Take(RowCount)
                .ToList());
        }

        // Метод для получения 20 записей из таблицы ServiceContracts
        public IEnumerable<ServiceContract> GetServiceContracts()
        {
            return GetOrSetCache("ServiceContracts", () => _context.ServiceContracts
                .Include(s => s.Employee)
                .Include(s => s.Subscriber)
                .Include(s => s.TariffPlanNameNavigation)
                .Take(RowCount)
                .ToList());
        }

        // Метод для получения 20 записей из таблицы Employees
        public IEnumerable<Employee> GetEmployees()
        {
            return GetOrSetCache("Employees", () => _context.Employees
                .Take(RowCount)
                .ToList());
        }

        // Метод для получения 20 записей из таблицы ServiceStatistics
        public IEnumerable<ServiceStatistic> GetServiceStatistics()
        {
            return GetOrSetCache("ServiceStatistics", () => _context.ServiceStatistics
                .Include(s => s.Contract)
                    .ThenInclude(c => c.Subscriber)
                .Include(s => s.Contract)
                    .ThenInclude(c => c.Employee)
                .Take(RowCount)
                .ToList());
        }
    }
}
