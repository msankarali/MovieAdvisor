using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Movies.Queries.GetMovieDetailsById;

public class GetMovieDetailsByIdQuery : IRequest<MovieDetailsDto>
{
    public int MovieId { get; init; }

    public class GetMovieDetailsByIdQueryHandler : IRequestHandler<GetMovieDetailsByIdQuery, MovieDetailsDto>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetMovieDetailsByIdQueryHandler(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<MovieDetailsDto> Handle(GetMovieDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var movieDetails = await _dbContext.Set<Movie>().AsNoTracking()
                .Include(m => m.Ratings)
                .Where(m => m.Id == request.MovieId)
                .ProjectTo<MovieDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (movieDetails is null) throw new NotFoundException("Movie not found");

            return movieDetails;
        }
    }
}
