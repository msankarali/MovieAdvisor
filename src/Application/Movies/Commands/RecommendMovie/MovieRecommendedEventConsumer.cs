using System.Diagnostics;
using System.Text.Json;
using Application.Common.Interfaces;
using Application.Common.Models.Mail;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Movies.Commands.RecommendMovie;

public class MovieRecommendedEventConsumer : IConsumer<MovieRecommendedEvent>
{
    private readonly ILogger<MovieRecommendedEventConsumer> _logger;
    private readonly IEmailService _emailService;

    public MovieRecommendedEventConsumer(ILogger<MovieRecommendedEventConsumer> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public Task Consume(ConsumeContext<MovieRecommendedEvent> context)
    {
        _logger.LogInformation(JsonSerializer.Serialize(context.Message));

        return _emailService.SendAsync(new EmailMessage
        {
            Subject = "Movie Advisor - Movie recommendation from a friend..",
            Body = $$"""
                   Heyyyy!!!
                   Your friend recommended u wathin' <strong>{{context.Message.MovieTitle}}</strong>!
                   Check <a href="http://movieadvisor.com/movies/{{context.Message.MovieId}}">this</a> out, NOW!
                   """,
            To = new string[] { context.Message.Email },
            IsBodyHtml = true
        });
    }
}
