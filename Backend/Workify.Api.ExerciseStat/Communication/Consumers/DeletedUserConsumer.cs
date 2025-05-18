using MassTransit;
using Workify.Api.ExerciseStat.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.ExerciseStat.Communication.Consumers
{
    public class DeletedUserConsumer(IStatService statService) : IConsumer<DeletedUserContract>
    {
        private readonly IStatService _statService = statService;

        public async Task Consume(ConsumeContext<DeletedUserContract> context)
        {
            // TODO: delete user stats
        }
    }
}
