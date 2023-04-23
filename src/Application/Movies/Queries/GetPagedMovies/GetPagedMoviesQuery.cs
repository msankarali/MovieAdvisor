using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Models.Cache;
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
        private readonly ICacheService _cacheService;

        public GetPagedMoviesQueryHandler(IDbContext dbContext, IMapper mapper, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<PagedList<MovieDto>> Handle(GetPagedMoviesQuery request, CancellationToken cancellationToken)
        {
            string cachePattern = $"{CachePatterns.Movies.PagedMovies.Pattern}|pageNumber:{request.PageNumber}|pageSize:{request.PageSize}";
            // _cacheService.RemoveByPattern(CachePatterns.Movies.PagedMovies.Pattern);

            return await _cacheService.GetOrAddAsync<PagedList<MovieDto>>(
                key: cachePattern,
                action: async () =>
                    await _dbContext.Set<Movie>().AsNoTracking()
                        .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
                        .PagedListAsync<MovieDto>(
                            pageNumber: request.PageNumber,
                            pageSize: request.PageSize
                        ));
        }
    }
}
