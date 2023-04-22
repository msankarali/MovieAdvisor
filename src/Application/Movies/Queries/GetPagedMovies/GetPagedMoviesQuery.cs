using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Movies.Queries.GetPagedMovies;

public class GetPagedMoviesQuery : IRequest<PagedList<MovieDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public class GetPagedMoviesQueryHandler : IRequestHandler<GetPagedMoviesQuery, PagedList<MovieDto>>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetPagedMoviesQueryHandler(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PagedList<MovieDto>> Handle(GetPagedMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _dbContext.Set<Movie>().AsNoTracking()
                .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
                .PagedListAsync<MovieDto>(
                    pageNumber: request.PageNumber,
                    pageSize: request.PageSize
                );

            return movies;
        }
    }
}
