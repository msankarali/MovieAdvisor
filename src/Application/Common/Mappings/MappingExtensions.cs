using System.Linq.Expressions;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PagedList<TDestination>> PagedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class => PagedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();

    public static IMappingExpression<TSource, TDestination> MapIf<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map,
        Expression<Func<TDestination, object>> selector,
        Func<TSource, bool> mapIfCondition,
        Expression<Func<TSource, object>> mapping)
    {
        map.ForMember(selector, c =>
        {
            c.MapFrom(mapping);
            c.PreCondition(mapIfCondition);
        });
        return map;
    }
}
