using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.QueryableExtensions;
using EFCore.BulkExtensions;
using Stock_CMS.Entity;
using Microsoft.EntityFrameworkCore;
using log4net;
using Stock_CMS.Models;
using System.Collections;
using Stock_CMS.Common;
using Stock_CMS.Entity;

namespace Stock_CMS.Common
{
	public class EfRepository<T, TModel> : IRepository<T, TModel>, IAsyncRepository<T, TModel>
        where T : class
        where TModel : class
    {
        private readonly DmCmsContext _dbContext;
        private readonly IMapper _mapper;
        private readonly int _bulkInsertorUpdateLimit = 1000;
        private static readonly ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private readonly ILogger<EfRepository<T, TModel>>  _logger;


        public EfRepository(DmCmsContext dbContext, IMapper mapper)
        {
           
            _dbContext = dbContext;
            _mapper = mapper;
        }

        protected async Task<IEnumerable<TModel>> GetMany(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var ent = _dbContext.Set<T>().Where(predicate);
                ent = AllIncludes().Aggregate(ent, (current, include) => current.Include(include));
                var entities = await ent.ToArrayAsync().ConfigureAwait(false);
                return entities.Any() ? _mapper.Map<T[], TModel[]>(entities) : Enumerable.Empty<TModel>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Enumerable.Empty<TModel>();
            }
        }
        protected async Task<TModel> GetOne(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var query = _dbContext.Set<T>().Where(predicate);
                query = AllIncludes().Aggregate(query, (current, include) => current.Include(include));
                var entity = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                return entity is not null ? _mapper.Map<T, TModel>(entity) : Activator.CreateInstance<TModel>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Activator.CreateInstance<TModel>();
            }
        }
        protected async Task<IEnumerable<TModel>> AddEntitiesBulk(IEnumerable<TModel> models)
        {
            try
            {
                var entities = _mapper.Map<IEnumerable<TModel>, T[]>(models);
                await _dbContext.BulkInsertAsync<T>(entities, new BulkConfig()
                {
                    BatchSize = _bulkInsertorUpdateLimit,
                    PreserveInsertOrder = true,
                    SetOutputIdentity = true
                });
                await _dbContext.BulkSaveChangesAsync();
                return _mapper.Map<T[], IEnumerable<TModel>>(entities);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Enumerable.Empty<TModel>();
            }
        }
        protected async Task<IEnumerable<TModel>> UpdateEntities(IEnumerable<TModel> models)
        {
            try
            {
                var entities = _mapper.Map<IEnumerable<TModel>, T[]>(models);
                _dbContext.BulkUpdate<T>(entities, new BulkConfig() { BatchSize = _bulkInsertorUpdateLimit });
                await _dbContext.BulkSaveChangesAsync();
                return _mapper.Map<T[], IEnumerable<TModel>>(entities);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Enumerable.Empty<TModel>();
            }
        }

		protected async Task<IEnumerable<TModel>> UpdateEntitiesArray(IEnumerable<TModel> models, params string[] columnsToUpdate)
		{
			try
			{
				var entities = _mapper.Map<IEnumerable<TModel>, T[]>(models);
				var bulkConfig = new BulkConfig
				{
					BatchSize = _bulkInsertorUpdateLimit,
					PropertiesToInclude = columnsToUpdate.ToList() 
				};
				_dbContext.BulkUpdate(entities, bulkConfig);
				await _dbContext.BulkSaveChangesAsync();
				return _mapper.Map<T[], IEnumerable<TModel>>(entities);
			}
			catch (Exception ex)
			{
                _logger.Error(ex.Message);
                //_logger.LogError(ex, "Error updating entities Array");
                return Enumerable.Empty<TModel>();
			}
		}

		protected async Task<IEnumerable<TModel>> DeleteEntities(Func<T, bool> predicate)
        {
            try
            {
                var entities = _dbContext.Set<T>().Where(predicate).ToArray();

                if (entities.Any())
                {
                    _dbContext.Set<T>().RemoveRange(entities);
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                    return _mapper.Map<T[], IEnumerable<TModel>>(entities);
                }

                return Enumerable.Empty<TModel>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Enumerable.Empty<TModel>();
            }
        }

        protected async Task<IEnumerable<TModel>> AddEntities(IEnumerable<TModel> models)
        {
            try
            {
                var entities = _mapper.Map<IEnumerable<TModel>, IEnumerable<T>>(models);

                await _dbContext.AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<IEnumerable<T>, IEnumerable<TModel>>(entities);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Enumerable.Empty<TModel>();
            }
        }


        protected async Task<IEnumerable<TModel>> AddEntitiesNotMap(IEnumerable<TModel> models)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TModel, T>()
                       .ForAllMembers(opts =>
                           opts.Condition((src, dest, srcMember, destMember, context) =>
                               typeof(TModel).GetProperty(opts.DestinationMember.Name)?
                                   .GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0));

                    cfg.CreateMap<T, TModel>();
                });

                var mapper = new Mapper(config);
                var entities = mapper.Map<IEnumerable<T>>(models);

                await _dbContext.AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<IEnumerable<T>, IEnumerable<TModel>>(entities);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                return Enumerable.Empty<TModel>();
            }
        }
        protected async Task<IEnumerable<TModel>> AddOrUpdateEntities(IEnumerable<TModel> models)
        {
            try
            {
                var entities = _mapper.Map<IEnumerable<T>>(models);

                foreach (var entity in entities)
                {
                    var entityEntry = _dbContext.Entry(entity);
                    var keyProperty = entityEntry.Metadata.FindPrimaryKey()?.Properties.FirstOrDefault();

                    if (keyProperty == null)
                        throw new InvalidOperationException("Entity must have a primary key.");

                    var entityId = entityEntry.Property(keyProperty.Name).CurrentValue;
                    var existing = await _dbContext.Set<T>().FindAsync(entityId);

                    if (existing != null)
                    {
                        // Update scalar properties
                        _dbContext.Entry(existing).CurrentValues.SetValues(entity);

                        // Handle navigations
                        await AttachNavigationPropertiesAsync(entity, existing);
                    }
                    else
                    {
                        // Attach any existing foreign objects in this new entity
                        await AttachNavigationPropertiesAsync(entity, null);

                        await _dbContext.AddAsync(entity);
                    }
                }

                await _dbContext.SaveChangesAsync();

                return _mapper.Map<IEnumerable<T>, IEnumerable<TModel>>(entities);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Enumerable.Empty<TModel>();
            }
        }
        protected async Task<PagedResult<TModel>> GetManyPaged(
Expression<Func<T, bool>> predicate,
PaginationSpecification pagination = null)
        {
            try
            {
                var query = _dbContext.Set<T>().Where(predicate);
                query = AllIncludes().Aggregate(query, (current, include) => current.Include(include));

                int totalRecords = await query.CountAsync().ConfigureAwait(false);
                int pageNumber = 1;
                int pageSize = totalRecords;

                if (pagination != null)
                {
                    if (pagination.TotalNeeded)
                        pagination.TotalRecords = totalRecords;

                    query = query.Skip(pagination.NumberToSkip).Take(pagination.NumberToTake);
                    pageNumber = pagination.NumberToSkip / pagination.NumberToTake + 1;
                    pageSize = pagination.NumberToTake;
                }

                var entities = await query.ToArrayAsync().ConfigureAwait(false);
                var models = entities.Any() ? _mapper.Map<T[], TModel[]>(entities) : Array.Empty<TModel>();

                return new PagedResult<TModel>(models, totalRecords, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new PagedResult<TModel>(Enumerable.Empty<TModel>(), 0, 1, 0);
            }
        }

        public async Task<IEnumerable<TModel>> ListWithExpressionAsync<TKey>(IOrderSpecification<T, TKey> orderSpec, Expression<Func<T, bool>> predicate)
        {
            //var queryableResultWithIncludes = spec.Includes
            //    .Aggregate(context.Set<T>().AsQueryable(),
            //    (current, include) => current.Include(include));

            //var secondaryResult = spec.IncludeStrings
            //    .Aggregate(queryableResultWithIncludes,
            //        (current, include) => current.Include(include));

            //var result = secondaryResult.Where(spec.Criteria);

            var result = _dbContext.Set<T>().Where(predicate);

            if (orderSpec.SortDescending)
                result = result.OrderByDescending(orderSpec.SortBy).Take(1);
            else
                result = result.OrderBy(orderSpec.SortBy).Take(1);

            //pageSpec.TotalRecords = result.Count();

            //result = result
            //    .Skip(pageSpec.NumberToSkip)
            //    .Take(pageSpec.NumberToTake);

            var data = await result.ToArrayAsync().ConfigureAwait(false);

            return data.Any() ? _mapper.Map<T[], TModel[]>(data) : Enumerable.Empty<TModel>();
        }

        //Helper Function
        private async Task AttachNavigationPropertiesAsync(T entity, T existingEntity)
        {
            var navProps = _dbContext.Entry(entity).Navigations.ToList();

            foreach (var nav in navProps)
            {
                var navValue = nav.CurrentValue;

                if (navValue == null) continue;

                if (nav.Metadata.IsCollection)
                {
                    var elementType = nav.Metadata.ClrType.GenericTypeArguments.FirstOrDefault();
                    if (elementType == null) continue;

                    var typedList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

                    foreach (var item in (IEnumerable<object>)navValue)
                    {
                        var itemEntry = _dbContext.Entry(item);
                        var keyProperty = itemEntry.Metadata.FindPrimaryKey()?.Properties.FirstOrDefault();
                        var itemKey = keyProperty?.PropertyInfo?.GetValue(item);

                        var tracked = itemKey != null
                            ? await _dbContext.FindAsync(elementType, itemKey)
                            : null;

                        typedList.Add(tracked ?? item);
                    }

                    if (existingEntity != null)
                    {
                        _dbContext.Entry(existingEntity).Navigation(nav.Metadata.Name).CurrentValue = typedList;
                    }
                    else
                    {
                        nav.CurrentValue = typedList;
                    }
                }

                else
                {
                    // Handle reference
                    var refEntry = _dbContext.Entry(navValue);
                    var refKey = refEntry.Metadata.FindPrimaryKey()?.Properties.FirstOrDefault()?.PropertyInfo?.GetValue(navValue);

                    var existingRef = refKey != null
                        ? await _dbContext.FindAsync(navValue.GetType(), refKey)
                        : null;

                    if (existingEntity != null)
                    {
                        _dbContext.Entry(existingEntity).Navigation(nav.Metadata.Name).CurrentValue = existingRef ?? navValue;
                    }
                    else
                    {
                        nav.CurrentValue = existingRef ?? navValue;
                    }
                }
            }
        }


        //OLD

        private IEnumerable<string> AllIncludes()
        {
            var map = _mapper.ConfigurationProvider.Internal().FindTypeMapFor(typeof(T), typeof(TModel));
            if (map == null || map.PropertyMaps == null)
                return Array.Empty<string>();
            return map.PropertyMaps.Where(v => v.CustomMapExpression != null).Any(v => v.CustomMapExpression.ToString().Count(v => (v == '.')) > 1)
            ? map.PropertyMaps.Where(v => v.CustomMapExpression != null).Select(v => string.Join(".", v.CustomMapExpression.ToString().Split('.').SkipLast(1).Skip(1))).Where(v => !string.IsNullOrEmpty(v)).Distinct()
            : Array.Empty<string>();
        }
        public IEnumerable<TModel> List<TKey>(ISpecification<T> spec, PaginationSpecification pageSpec, DbContext context)
        {
            var queryableResultWithIncludes = spec.Includes
               .Aggregate(context.Set<T>().AsQueryable(),
               (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            var result = secondaryResult.Where(spec.Criteria);

            pageSpec.TotalRecords = result.Count();

            result = result.Skip(pageSpec.NumberToSkip).Take(pageSpec.NumberToTake);

            return _mapper.Map<T[], TModel[]>(result.ToArray());
        }

        public async Task<IEnumerable<TModel>> ListAsync<TKey>(ISpecification<T> spec, PaginationSpecification pageSpec, DbContext context)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(context.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            var result = secondaryResult.Where(spec.Criteria);

            if (pageSpec.TotalNeeded)
            {
                pageSpec.TotalRecords = result.Count();
            }

            result = result.Skip(pageSpec.NumberToSkip).Take(pageSpec.NumberToTake);

            var paginatedResult = await result.ToListAsync();

            return _mapper.Map<T[], TModel[]>(paginatedResult.ToArray());
        }

        public async Task<IEnumerable<TModel>> ListAsync(ISpecification<T> spec, PaginationSpecification pageSpec, DbContext context, IMapper mapper)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(context.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            var result = secondaryResult.Where(spec.Criteria);
            pageSpec.TotalRecords = pageSpec.TotalNeeded ? result.Count() : 0;

            return await result.Skip(pageSpec.NumberToSkip).Take(pageSpec.NumberToTake).ProjectTo<TModel>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<T>> ListAsync<TKey>(ISpecification<T> spec, IOrderSpecification<T, TKey> orderSpec, PaginationSpecification pageSpec, DbContext context)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(context.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            var result = secondaryResult.Where(spec.Criteria);

            if (orderSpec.SortDescending)
                result = result.OrderByDescending(orderSpec.SortBy);
            else
                result = result.OrderBy(orderSpec.SortBy);

            pageSpec.TotalRecords = result.Count();

            result = result
                .Skip(pageSpec.NumberToSkip)
                .Take(pageSpec.NumberToTake);

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<TModel>> ListAsync(ISpecification<T> spec, DbContext context)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(context.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            var result = await secondaryResult.Where(spec.Criteria).ToListAsync();
            return _mapper.Map<T[], TModel[]>(result.ToArray());
        }
        public async Task<IEnumerable<T>> ListAsyncT<T>(
ISpecification<T> spec,
PaginationSpecification pageSpec,
DbContext context,
IMapper mapper,
List<FilterRule> filters,
Expression<Func<T, bool>> searchExpression,
string sortColumn,
bool sortDescending
) where T : class
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(context.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // Search
            if (searchExpression != null)
            {
                secondaryResult = secondaryResult.Where(searchExpression);
            }

            // Dynamic filters
            var filterExpression = FilterHelper.BuildFilterExpression<T>(filters);
            if (filterExpression != null)
            {
                secondaryResult = secondaryResult.Where(filterExpression);
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortColumn))
            {
                var property = typeof(T).GetProperty(sortColumn);
                if (property != null)
                {
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var propertyAccess = Expression.Property(parameter, property);
                    var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                    var methodName = sortDescending ? "OrderByDescending" : "OrderBy";
                    var resultExpression = Expression.Call(typeof(Queryable), methodName,
                        new Type[] { typeof(T), property.PropertyType },
                        secondaryResult.Expression,
                        Expression.Quote(orderByExpression));

                    secondaryResult = secondaryResult.Provider.CreateQuery<T>(resultExpression);
                }
            }

            // Apply spec criteria
            var result = secondaryResult.Where(spec.Criteria);

            // Total records
            pageSpec.TotalRecords = pageSpec.TotalNeeded ? result.Count() : 0;

            // Pagination
            return await result.Skip(pageSpec.NumberToSkip)
                               .Take(pageSpec.NumberToTake)
                               .ToListAsync();
        }

        public async Task<IEnumerable<T>> ListAsyncRaw(ISpecification<T> spec, DbContext context)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(context.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            if (spec.Criteria == null)
            {
                return await secondaryResult.ToListAsync();
            }
            else
            {
                return await secondaryResult.Where(spec.Criteria).ToListAsync();
            }
        }

        protected async Task<IReadOnlyList<TModel>> GetAllEntities()
        {
            var entities = await _dbContext.Set<T>().ToArrayAsync().ConfigureAwait(false);
            return _mapper.Map<T[], TModel[]>(entities);
        }
        protected async Task<IEnumerable<TModel>> AddEntitieswithArr(IEnumerable<TModel> models, List<string> arr)
        {
            var entities = _mapper.Map<IEnumerable<TModel>, T[]>(models);
            await _dbContext.BulkInsertAsync<T>(entities, new BulkConfig() { BatchSize = _bulkInsertorUpdateLimit, PreserveInsertOrder = true, UpdateByProperties = arr });
            await _dbContext.BulkSaveChangesAsync();
            return _mapper.Map<T[], IEnumerable<TModel>>(entities);
        }
        protected async Task<IEnumerable<TModel>> UpdateEntitieswithArr(IEnumerable<TModel> models, List<string> arr)
        {
            var entities = _mapper.Map<IEnumerable<TModel>, T[]>(models);
            _dbContext.BulkUpdate<T>(entities, new BulkConfig() { BatchSize = _bulkInsertorUpdateLimit, PreserveInsertOrder = true, UpdateByProperties = arr });
            await _dbContext.BulkSaveChangesAsync();
            return _mapper.Map<T[], IEnumerable<TModel>>(entities);
        }
        protected async Task<IEnumerable<TModel>> AddEntitiesWithLimit(IEnumerable<TModel> models)
        {
            //var allEntities = _mapper.Map<IEnumerable<TModel>, T[]>(models);
            
            return await AddEntities(models);

            //int sk            pipes == 0;
            //int count = models.Count() / _bulkInsertorUpdateLimit;
            //for (int i = 0; i <= count; i++)
            //{
            //    var model = models.Skip(skip).Take(_bulkInsertorUpdateLimit);
            //    if (model.Any())
            //    {
            //        var entities = _mapper.Map<IEnumerable<TModel>, T[]>(model);
            //        await _dbContext.Set<T>().AddRangeAsync(entities).ConfigureAwait(false);
            //        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            //        skip += _bulkInsertorUpdateLimit;
            //    }
            //}Suraj_Yadav_00;
            

            //return _mapper.Map<T[], IEnumerable<TModel>>(allEntities);
        }
        protected async Task<IEnumerable<TModel>> UpdateEntitiesWithLimit(IEnumerable<TModel> models)
        {
            return await UpdateEntities(models);

            //var allEntities = _mapper.Map<IEnumerable<TModel>, T[]>(models);
            //int skip = 0;
            //int count = models.Count() / _bulkInsertorUpdateLimit;
            //for (int i = 0; i <= count; i++)
            //{
            //    var model = models.Skip(skip).Take(_bulkInsertorUpdateLimit);
            //    if (model.Any())
            //    {
            //        var entities = _mapper.Map<IEnumerable<TModel>, T[]>(model);
            //        _dbContext.Set<T>().UpdateRange(entities);
            //        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            //        skip += _bulkInsertorUpdateLimit;
            //    }
            //}

            //return _mapper.Map<T[], IEnumerable<TModel>>(allEntities);
        }
       
        protected async Task<IEnumerable<TModel>> BulkInsertOrUpdate(IEnumerable<TModel> models)
        {
            var entities = _mapper.Map<IEnumerable<TModel>, T[]>(models);
            if (entities.Any())
            {
                await _dbContext.BulkInsertOrUpdateAsync<T>(entities, new BulkConfig() { SetOutputIdentity = true, BatchSize = _bulkInsertorUpdateLimit, PreserveInsertOrder = true, WithHoldlock = false });

                
            }

            try
            {
                await _dbContext.BulkSaveChangesAsync();
                return _mapper.Map<T[], IEnumerable<TModel>>(entities);
            }
            catch (Exception)
            {
                return Array.Empty<TModel>();
            }
        }
        protected async Task<IEnumerable<TModel>> AddEntities(
    IEnumerable<TModel> models,
    Expression<Func<TModel, object>>[] modelKeySelectors,
    Expression<Func<T, object>>[] entityKeySelectors)
        {
            if (modelKeySelectors.Length != entityKeySelectors.Length)
                throw new ArgumentException("Model key selectors and entity key selectors must have the same length.");

            var entities = _mapper.Map<IEnumerable<TModel>, List<T>>(models);

            var modelKeys = models
                .Select(model => modelKeySelectors.Select(ks => ks.Compile()(model)).ToArray())
                .ToList();

            var parameter = Expression.Parameter(typeof(T), "e");
            Expression predicate = Expression.Constant(false);

            foreach (var modelKey in modelKeys)
            {
                Expression currentPredicate = Expression.Constant(true);

                for (int i = 0; i < entityKeySelectors.Length; i++)
                {
                    var entityKeySelector = entityKeySelectors[i];
                    var modelKeyValue = Expression.Constant(modelKey[i]);

                    var entityKey = Expression.Invoke(entityKeySelector, parameter);
                    var entityKeyConverted = Expression.Convert(entityKey, modelKey[i].GetType());
                    var equalExpression = Expression.Equal(entityKeyConverted, modelKeyValue);

                    currentPredicate = Expression.AndAlso(currentPredicate, equalExpression);
                }

                predicate = Expression.OrElse(predicate, currentPredicate);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);

            var existingEntities = await _dbContext.Set<T>()
                .Where(lambda)
                .ToListAsync();

            var newEntities = entities
                .Where(e => !existingEntities
                    .Select(existingEntity => entityKeySelectors.Select(ek => ek.Compile()(existingEntity)).ToArray())
                    .Any(existingKeys => existingKeys.SequenceEqual(entityKeySelectors.Select(ek => ek.Compile()(e)))))
                .ToList();

            var updatedEntities = entities
                .Where(e => existingEntities
                    .Select(existingEntity => entityKeySelectors.Select(ek => ek.Compile()(existingEntity)).ToArray())
                    .Any(existingKeys => existingKeys.SequenceEqual(entityKeySelectors.Select(ek => ek.Compile()(e)))))
                .ToList();

            if (newEntities.Any())
            {
                await _dbContext.BulkInsertAsync(newEntities, new BulkConfig
                {
                    BatchSize = _bulkInsertorUpdateLimit,
                    PreserveInsertOrder = true,
                    SetOutputIdentity = true
                });
            }

            await _dbContext.BulkSaveChangesAsync();
            var resultEntities = newEntities.Concat(updatedEntities).ToList();
            return _mapper.Map<IEnumerable<T>, IEnumerable<TModel>>(resultEntities);
        }

    }

}
