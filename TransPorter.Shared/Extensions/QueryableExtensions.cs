﻿using Microsoft.EntityFrameworkCore;
using TransPorter.Shared.Enums;
using TransPorter.Shared.Interfaces;
using TransPorter.Shared.SpecificationsBase;
using TransPorter.Shared.Wrapper;

namespace TransPorter.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class
    {
        if (source == null) throw new();
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        int count = await source.CountAsync();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
    }

    public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class, IEntity
    {
        var queryableResultWithIncludes = spec.Includes
            .Aggregate(query,
                (current, include) => current.Include(include));

        var secondaryResult = spec.IncludeStrings
            .Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));

        var queryableResultWithWhere = secondaryResult.Where(spec.Criteria);

        if (spec.OrderBy != null && spec.SortDirection != SortDirection.None)
        {
            queryableResultWithWhere =
                spec.SortDirection == SortDirection.Ascending ?
                queryableResultWithWhere.OrderBy(spec.OrderBy) :
                queryableResultWithWhere.OrderByDescending(spec.OrderBy);
        }

        return queryableResultWithWhere;
    }
}

