using CleanerScheduleManager.Models;
using CleanerScheduleManager.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using CleanerSkillLevel = CleanerScheduleManager.Models.Enums.CleanerSkillLevel;
using TaskStatus = CleanerScheduleManager.Models.Enums.TaskStatus;

namespace CleanerScheduleManager.Tests
{
    public class JsonDataServiceTests
    {
        private readonly JsonDataService _service = new();

        [Fact]
        public async Task LoadAsync_ReturnsEmptyList_WhenFileMissing()
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var result = await _service.LoadAsync<Cleaner>(path);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SaveAsync_And_LoadAsync_PersistCleaners()
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                var items = new[]
                {
                    new Cleaner { Id = 1, Name = "Alice", SkillLevel = CleanerSkillLevel.Beginner, IsAvailable = true },
                    new Cleaner { Id = 2, Name = "Bob", SkillLevel = CleanerSkillLevel.Experienced, IsAvailable = false }
                };
                await _service.SaveAsync(items, path);

                var loaded = await _service.LoadAsync<Cleaner>(path);
                Assert.Collection(loaded,
                    c =>
                    {
                        Assert.Equal(1, c.Id);
                        Assert.Equal("Alice", c.Name);
                        Assert.True(c.IsAvailable);
                        Assert.Equal(CleanerSkillLevel.Beginner, c.SkillLevel);
                    },
                    c =>
                    {
                        Assert.Equal(2, c.Id);
                        Assert.Equal("Bob", c.Name);
                        Assert.False(c.IsAvailable);
                        Assert.Equal(CleanerSkillLevel.Experienced, c.SkillLevel);
                    });
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Fact]
        public async Task SaveAsync_And_LoadAsync_PersistTasksWithRelatedModels()
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                var tasks = new[]
                {
                    new CleaningTask
                    {
                        TaskId = 10,
                        Status = TaskStatus.InProgress,
                        ScheduledDate = new DateTime(2025, 1, 1),
                        Duration = TimeSpan.FromHours(1),
                        Client = new Client { Id = 5, Name = "Client" },
                        Cleaner = new Cleaner { Id = 7, Name = "Cleaner", SkillLevel = CleanerSkillLevel.Intermediate }
                    }
                };

                await _service.SaveAsync(tasks, path);
                var loaded = await _service.LoadAsync<CleaningTask>(path);

                var task = Assert.Single(loaded);
                Assert.Equal(10, task.TaskId);
                Assert.Equal(TaskStatus.InProgress, task.Status);
                Assert.Equal(new DateTime(2025, 1, 1), task.ScheduledDate);
                Assert.Equal(TimeSpan.FromHours(1), task.Duration);
                Assert.NotNull(task.Client);
                Assert.Equal(5, task.Client!.Id);
                Assert.NotNull(task.Cleaner);
                Assert.Equal(CleanerSkillLevel.Intermediate, task.Cleaner!.SkillLevel);
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
}