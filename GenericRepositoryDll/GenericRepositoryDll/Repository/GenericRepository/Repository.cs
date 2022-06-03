using GenericReositoryDll.Enumrations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericReositoryDll.Repository.GenericRepository
{
  public class Repository<T> : IRepository<T> where T : class
  {

    private readonly DbContext _context;
    private readonly DbSet<T> _model;
    public Repository(DbContext context)
    {
      this._context = context;
      _model = _context.Set<T>();
    }
    public async Task AddAsync(T model)
    {
      await _model.AddAsync(model);
      await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> models)
    {
      await _model.AddRangeAsync(models);
      await _context.SaveChangesAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> query)
    => await _model.AnyAsync(query);

    public async Task DeleteAsync(long id)
    {
      T model =  _context.FindAsync<T>(id).Result;
      if(model != null) _model.Remove(model);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<T> models)
    {
      _model.RemoveRange(models);
      await _context.SaveChangesAsync();
    }

    public async Task<long> GetCountAsync(Expression<Func<T, bool>> query = null)
    => await _model.CountAsync(query);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="query">Get where query</param>
    /// <param name="orderBy">Order by orderBy expression</param>
    /// <param name="orderByType">Desc | Asc </param>
    /// <param name="skip">skip</param>
    /// <param name="take">take</param>
    /// <returns></returns>
    public async Task<IQueryable<object>> GetListAsync(Expression<Func<T, bool>> query = null,
      Func<T,object> selector = null,
      Func<T, IOrderedQueryable<T>> orderBy = null,
      OrderByType? orderByType = null,
      List<string> includes = null,
      int? skip = null,
      int? take = null,
      bool? distinct = null)
    {

      var models =  _model.AsQueryable().AsNoTrackingWithIdentityResolution();
      if (query != null) models = models.Where(query); 
      if(includes != null && includes.Count()>0)
      {
        foreach (var includeProperty in includes)
        {
          models.Include(includeProperty);
        }
      }
      if (selector != null) models.Select(selector);
      if(skip != null) models.Skip((int)skip);
      if(take != null) models.Take((int)take);
      if (orderBy != null && orderByType == OrderByType.Asc) models = models.OrderBy(orderBy).AsQueryable();
      if (orderBy != null && orderByType == OrderByType.Desc) models = models.OrderByDescending(orderBy).AsQueryable();
      if (orderBy != null && orderByType == null) models = models.OrderBy(orderBy).AsQueryable();
      if (distinct != null) models.Distinct();

      return models;

    }


    public async Task<IQueryable<T>> GetListAsync(Expression<Func<T, bool>> query = null,
     Func<T, IOrderedQueryable<T>> orderBy = null,
     OrderByType? orderByType = null,
     List<string> includes = null, int? skip = 0,
     int? take = null, bool? distinct = null)
    {
      var result = (IQueryable<T>)GetListAsync(query, orderBy, orderByType, includes, skip).Result;
      return result;
    }

    public async Task<T> GetSingleAsync(long id)
    => await _model.FindAsync(id);

    public async Task<object> GetSingleAsync(Expression<Func<T, bool>> query,
      Func<T,object> selector,
      List<string> includes = null)
    {
      object result;

      dynamic model = _model.AsNoTrackingWithIdentityResolution().AsQueryable();
      if (includes != null && includes.Count() > 0)
      {
        foreach (var includeProperty in includes)
        {
          model.Include(includeProperty);
        }
      }

      if (query != null) model = model.Where(query);
      if (selector != null) model = model.Select(selector).FirstOrDefault();
      
      return model;
      
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> query,
      List<string> includes = null)
    {
      var result = (T)GetSingleAsync(query, includes).Result;
      return result;
    }

    public async Task UpdateAsync(T model)
    {
      _model.Update(model);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<T> models)
    {
      _model.UpdateRange(models);
      await _context.SaveChangesAsync();
    }

    public async Task SaveAsync()
    {
      await _context.SaveChangesAsync();
    }

   



   

  
  }
}