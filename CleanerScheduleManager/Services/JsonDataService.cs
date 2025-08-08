using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using CleanerScheduleManager.Services.Interfaces;

namespace CleanerScheduleManager.Services
{
    public class JsonDataService : IDataService
    {
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<List<T>> LoadAsync<T>(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return new List<T>();

                using var stream = File.OpenRead(path);
                var data = await JsonSerializer.DeserializeAsync<List<T>>(stream, _options);

                return data ?? new List<T>();
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to load data from '{path}'", ex);
            }
        }

        public async Task SaveAsync<T>(IEnumerable<T> items, string path)
        {
            try
            {
                using var stream = File.Create(path);
                await JsonSerializer.SerializeAsync(stream, items, _options);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to save data to '{path}'", ex);
            }
        }
    }
}