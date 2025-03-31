using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClubManagementSystem.Controllers.Worker
{
    public class StatusUpdatingWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public StatusUpdatingWorker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Perform your background task
                Console.WriteLine($"Worker running at {DateTime.Now}");

                using (var scopeServices = _serviceScopeFactory.CreateScope())
                {
                    //get services
                    var clubService = scopeServices.ServiceProvider.GetRequiredService<IClubService>();
                    var eventService = scopeServices.ServiceProvider.GetRequiredService<IEventService>();

                    //update event
                    var clubs = await clubService.GetAllClubsAsync();
                    foreach (var club in clubs)
                    {
                        var ongoing = await eventService.GetOnGoingEvent(club.ClubId);
                        if(ongoing != null)
                        {
                            ongoing.Status = "On Going";
                            await eventService.UpdateEventAsync(ongoing);
                        }
                    }
                }

                // Wait for 5 minutes before running again
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
