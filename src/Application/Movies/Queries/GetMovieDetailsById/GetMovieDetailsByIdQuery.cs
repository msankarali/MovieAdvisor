using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Models.Cache;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Movies.Queries.GetMovieDetailsById;

public class GetMovieDetailsByIdQuery : IRequest<DataResult<MovieDetailsDto>>
{
    public int MovieId { get; init; }

    public class GetMovieDetailsByIdQueryHandler : IRequestHandler<GetMovieDetailsByIdQuery, DataResult<MovieDetailsDto>>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetMovieDetailsByIdQueryHandler(IDbContext dbContext, IMapper mapper, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<DataResult<MovieDetailsDto>> Handle(GetMovieDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            if (_cacheService.TryGet<MovieDetailsDto>(
                key: CachePatterns.Movies.MovieDetails.GetMovieDetailsById(request.MovieId),
                out var cachedMovie)) return DataResult<MovieDetailsDto>.Success(cachedMovie);
            else
            {
                var movieDetails = await _dbContext.Set<Movie>().AsNoTracking()
                    .Include(m => m.Ratings)
                    .Where(m => m.Id == request.MovieId)
                    .ProjectTo<MovieDetailsDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (movieDetails is null) throw new NotFoundException("Movie not found!");

                await _cacheService.SetValueAsync<MovieDetailsDto>(
                    key: CachePatterns.Movies.MovieDetails.GetMovieDetailsById(request.MovieId),
                    value: movieDetails,
                    duration: TimeSpan.FromDays(7));

                return DataResult<MovieDetailsDto>.Success(movieDetails);
            }
        }
    }
}
