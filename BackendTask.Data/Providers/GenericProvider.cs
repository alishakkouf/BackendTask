using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BackendTask.Shared;
using Microsoft.EntityFrameworkCore;

namespace BackendTask.Data.Providers
{
    internal class GenericProvider<TEntity> where TEntity : class, IAuditedEntity
    {
        protected readonly BackendTaskDbContext _dbContext;
        protected readonly IMapper _mapper;

        public GenericProvider()
        {

        }

        public GenericProvider(BackendTaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public GenericProvider(BackendTaskDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// IQueryable of entities where IsDeleted != true
        /// </summary>
        protected virtual IQueryable<TEntity> ActiveDbSet => _dbContext.Set<TEntity>().Where(x => x.IsDeleted != true);

        protected async Task SoftDeleteListAsync(List<TEntity> list)
        {
            if (list.Any())
            {
                list.ForEach(x => x.IsDeleted = true);

                await _dbContext.SaveChangesAsync();
            }
        }

        protected async Task SoftDeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedResultDto<TResult>> ApplyPagingAsync<TResult, TSource, TPagedRequest>(IQueryable<TSource> source, TPagedRequest request)
            where TResult : class
            where TSource : class
            where TPagedRequest : PagedAndSortedResultRequestDto
        {
            var itemsCount = await source.CountAsync();

            var pageNumber = request?.PageIndex ?? 1;
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            var pageSize = request?.PageSize ?? Constants.DefaultPageSize;
            pageSize = pageSize <= 0 ? Constants.DefaultPageSize : pageSize;

            // apply pagination
            source = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var resultList = await source.ToListAsync();

            var result = new PagedResultDto<TResult>()
            {
                TotalCount = itemsCount,
                PagesSize = pageSize,
                Items = _mapper.Map<List<TResult>>(resultList),
            };

            return result;
        }
    }
}
