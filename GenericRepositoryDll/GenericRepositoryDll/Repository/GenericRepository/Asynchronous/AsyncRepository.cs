using GenericReositoryDll.Enumrations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericRepositoryDll.Repository.GenericRepository;

  public partial class Repository<T> : IRepository<T> where T : class
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
      await SaveAsync();
    }
    
    public async Task AddRangeAsync(IEnumerable<T> models)
    {
      await _model.AddRangeAsync(models);
      await SaveAsync();
    }
    
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> query)
    => await _model.AnyAsync(query);
    
    public async Task DeleteAsync(long id)
    {
      T model =  _context.FindAsync<T>(id).Result;
      if(model != null) _model.Remove(model);
      await SaveAsync();
    }
    
    public async Task DeleteRangeAsync(IEnumerable<T> models)
    {
      _model.RemoveRange(models);
      await SaveAsync();
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
    public async Task<List<object>> GetListAsync(Expression<Func<T, bool>> query = null,
      Func<T,object> selector = null,
      Func<T, object> orderBy = null,
      OrderType? orderType = null,
      List<string> includes = null,
      int? skip = null,
      int? take = null,
      bool? distinct = null)
    {
      dynamic result;
      var models =  _model.AsQueryable().AsNoTrackingWithIdentityResolution();
      if (query != null) models = models.Where(query);
      if (includes != null && includes.Count() > 0) includes.ForEach(includeProperty => models.Include(includeProperty)); 
      if(skip != null) models = models.Skip((int)skip);
      if(take != null) models = models.Take((int)take);
      if (orderBy != null && orderType == OrderType.Asc) models = models.OrderBy(orderBy).AsQueryable();
      if (orderBy != null && orderType == OrderType.Desc) models = models.OrderByDescending(orderBy).AsQueryable();
      if (orderBy != null && orderType == null) models = models.OrderBy(orderBy).AsQueryable();
      if (distinct != null) models.Distinct();
      if (selector != null) result = models.Select(selector);
      result = models;

      return result.Tolist();

    }


    public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> query = null,
     Func<T, object> orderBy = null,
     OrderType? orderType = null,
     List<string> includes = null, int? skip = 0,
     int? take = null, bool? distinct = null)
    {
      List<T> result = GetListAsync(query, orderBy, orderType, includes, skip , take , distinct).Result;
      return result;
    }

    public async Task<T> FindAsync(long id)
    => await _model.FindAsync(id);

    public async Task<object> GetSingleAsync(Expression<Func<T, bool>> query,
      Func<T,object> selector,
      List<string> includes = null)
    {

      dynamic model = _model.AsNoTrackingWithIdentityResolution().AsQueryable();
      if (includes != null && includes.Count() > 0)
        includes.ForEach(includeProperty => model.Include(includeProperty));
      if (query != null) model = model.Where(query);
      if (selector != null) model = model.Select(selector).FirstOrDefaultAsync().Result;     
      return model;
      
    }

    public async Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> query,
      List<string> includes = null)
    => (T)GetSingleAsync(query, includes).Result;

    public async Task UpdateAsync(T model)
    {
      _model.Update(model);
      await SaveAsync();
    }
    
    public async Task UpdateRangeAsync(IEnumerable<T> models)
    {
      _model.UpdateRange(models);
      await SaveAsync();
    }
    
    public async Task SaveAsync()
    => await _context.SaveChangesAsync();   
    
    








  }

