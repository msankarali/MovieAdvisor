using System.Globalization;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Movies.Queries.GetPagedMovies;

public class MovieDto : IMapFrom<Movie>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string AverageScore { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Movie, MovieDto>()
            .ForMember(
                dest => dest.AverageScore,
                memOpt => memOpt.MapFrom(src => !src.Ratings.Any() ? "Not rated" : src.Ratings.Average(r => r.Score).ToString("F1", new NumberFormatInfo { NumberDecimalSeparator = "." })));
    }
}
