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

    public void Add(T model)
    {
       _model.Add(model);
       Save();
    }

    public async Task AddRangeAsync(IEnumerable<T> models)
    {
      await _model.AddRangeAsync(models);
      await _context.SaveChangesAsync();
    }

    public void AddRange(IEnumerable<T> models)
    {
       _model.AddRange(models);
       Save();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> query)
    => await _model.AnyAsync(query);

    public bool Any(Expression<Func<T, bool>> query)
    =>  _model.Any(query);

    public async Task DeleteAsync(long id)
    {
      T model =  _context.FindAsync<T>(id).Result;
      if(model != null) _model.Remove(model);
      await _context.SaveChangesAsync();
    }

    public void Delete(long id)
    {
      T model = _context.Find<T>(id);
      if (model != null) _model.Remove(model);
      Save();
    }

    public async Task DeleteRangeAsync(IEnumerable<T> models)
    {
      _model.RemoveRange(models);
      await _context.SaveChangesAsync();
    }

    public void DeleteRange(IEnumerable<T> models)
    {
      _model.RemoveRange(models);
      Save();
    }

    public async Task<long> GetCountAsync(Expression<Func<T, bool>> query = null)
    => await _model.CountAsync(query);

    public long GetCount(Expression<Func<T, bool>> query = null)
    =>  _model.Count(query);

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


    public List<object> GetList(Expression<Func<T, bool>> query = null,
      Func<T, object> selector = null,
      Func<T, object> orderBy = null,
      OrderType? orderType = null,
      List<string> includes = null,
      int? skip = null,
      int? take = null,
      bool? distinct = null)
    {
      dynamic result;
      var models = _model.AsQueryable().AsNoTrackingWithIdentityResolution();
      if (query != null) models = models.Where(query);
      if (includes != null && includes.Count() > 0) includes.ForEach(includeProperty => models.Include(includeProperty));
      if (skip != null) models = models.Skip((int)skip);
      if (take != null) models = models.Take((int)take);
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


    public List<T> GetList<T>(Expression<Func<T, bool>> query = null,
     Func<T, object> orderBy = null,
     OrderType? orderType = null,
     List<string> includes = null, int? skip = 0,
     int? take = null, bool? distinct = null)
      => (List<T>) GetList(query, orderBy, orderType, includes, skip, take, distinct);


    public async Task<T> FindAsync(long id)
    => await _model.FindAsync(id);

    public T Find(long id)
    =>  _model.Find(id);

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

    public object GetSingle(Expression<Func<T, bool>> query,
     Func<T, object> selector,
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

    public T GetSingle<T>(Expression<Func<T, bool>> query,
      List<string> includes = null)
    =>(T)GetSingle(query, includes);


    public async Task UpdateAsync(T model)
    {
      _model.Update(model);
      await _context.SaveChangesAsync();
    }
    public void Update(T model)
    {
      _model.Update(model);
      Save();
    }
    public async Task UpdateRangeAsync(IEnumerable<T> models)
    {
      _model.UpdateRange(models);
      await _context.SaveChangesAsync();
    }
    public  void UpdateRange(IEnumerable<T> models)
    {
      _model.UpdateRange(models);
      Save();
    }
    public async Task SaveAsync()
    => await _context.SaveChangesAsync();   
    public  void Save()
    => _context.SaveChanges();
    








  }
}
