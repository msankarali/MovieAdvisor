using MassTransit;

namespace Application.Movies.Commands.RecommendMovie;

public class MovieRecommendedEventConsumer : IConsumer<MovieRecommendedEvent>
{
    public Task Consume(ConsumeContext<MovieRecommendedEvent> context)
    {
        

        return Task.CompletedTask;
    }
}