
using GenericReositoryDll.Enumrations;
using System.Linq.Expressions;

namespace GenericReositoryDll.Repository.GenericRepository
{
  
  public interface IRepository<T>  where T : class 
  {

    Task<T> FindAsync(long id);
    T Find(long id);
    Task<object> GetSingleAsync(Expression<Func<T, bool>> query,
      Func<T, object> selector ,
      List<string> includes = null);

    object GetSingle(Expression<Func<T, bool>> query,
      Func<T, object> selector,
      List<string> includes = null);

    Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> query,
      List<string> includes = null);

    T GetSingle<T>(Expression<Func<T, bool>> query,
      List<string> includes = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="query">Query </param>
    /// <param name="selector">Select </param>
    /// <param name="orderBy">Order By</param>
    /// <param name="orderByType">Asc| Desc</param>
    /// <param name="includes">include</param>
    /// <param name="skip">skip</param>
    /// <param name="take">take</param>
    /// <param name="distinct">isDistinct</param>
    /// <returns></returns>
    Task<List<object>> GetListAsync(Expression<Func<T, bool>> query = null,
                                Func<T, object> selector = null,
                                Func<T, object> orderBy = null,
                                OrderType? orderType = null,
                                List<string> includes = null,
                                int? skip = 0,
                                int? take = null,
                                bool? distinct = null);

    List<object> GetList(Expression<Func<T, bool>> query = null,
                                Func<T, object> selector = null,
                                Func<T, object> orderBy = null,
                                OrderType? orderType = null,
                                List<string> includes = null,
                                int? skip = 0,
                                int? take = null,
                                bool? distinct = null);

    Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> query = null,
                                Func<T, object> orderBy = null,
                                OrderType? orderType = null,
                                List<string> includes = null,
                                int? skip = 0,
                                int? take = null,
                                bool? distinct = null);

    List<T> GetList<T>(Expression<Func<T, bool>> query = null,
                                Func<T, object> orderBy = null,
                                OrderType? orderType = null,
                                List<string> includes = null,
                                int? skip = 0,
                                int? take = null,
                                bool? distinct = null);

    Task<long> GetCountAsync(Expression<Func<T, bool>> query = null);
    long GetCount(Expression<Func<T, bool>> query = null);
    Task AddAsync(T model);
    void Add(T model);
    Task AddRangeAsync(IEnumerable<T> models);
    void AddRange(IEnumerable<T> models);
    Task UpdateAsync(T model);
    void Update(T model);
    Task UpdateRangeAsync(IEnumerable<T> models);
    void UpdateRange(IEnumerable<T> models);
    Task DeleteAsync(long id);
    void Delete(long id);
    Task DeleteRangeAsync(IEnumerable<T> models);
    void DeleteRange(IEnumerable<T> models);
    Task<bool> AnyAsync(Expression<Func<T, bool>> query);
    bool Any(Expression<Func<T, bool>> query);
    Task SaveAsync();
    void Save();



  }
}
