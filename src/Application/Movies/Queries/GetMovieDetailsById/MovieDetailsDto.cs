using System.Globalization;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Movies.Queries.GetMovieDetailsById;

public class MovieDetailsDto : IMapFrom<Movie>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string AverageScore { get; set; }
    public List<MovieDetailsRatingItemDto> Ratings { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Movie, MovieDetailsDto>()
            .ForMember(dest => dest.AverageScore, opt => opt.MapFrom(src => !src.Ratings.Any() ? "Not rated" : src.Ratings.Average(r => r.Score).ToString("F1", new NumberFormatInfo { NumberDecimalSeparator = "." })));
        // .ForMember(
        //     dest => dest.AverageScore,
        //     memOpt => memOpt.MapFrom(src => src.Ratings.Average(r => r.Score)));
    }
}

public class MovieDetailsRatingItemDto : IMapFrom<Rating>
{
    public string UserFullName { get; set; }
    public int Score { get; set; }
    public string? Comment { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Rating, MovieDetailsRatingItemDto>()
            .ForMember(
                dest => dest.UserFullName,
                memOpt => memOpt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));
    }
}