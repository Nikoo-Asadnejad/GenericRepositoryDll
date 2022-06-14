
using GenericReositoryDll.Enumrations;
using System.Linq.Expressions;

namespace GenericReositoryDll.Repository.GenericRepository
{
  
  public interface IRepository<T>  where T : class 
  {

    Task<T> GetSingleAsync(long id);
    Task<object> GetSingleAsync(Expression<Func<T, bool>> query,
      Func<T, object> selector ,
      List<string> includes = null);

    Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> query,
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

    Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> query = null,
                                Func<T, object> orderBy = null,
                                OrderType? orderType = null,
                                List<string> includes = null,
                                int? skip = 0,
                                int? take = null,
                                bool? distinct = null);
    Task<long> GetCountAsync(Expression<Func<T, bool>> query = null);

    Task AddAsync(T model);
    Task AddRangeAsync(IEnumerable<T> models);
    Task UpdateAsync(T model);
    Task UpdateRangeAsync(IEnumerable<T> models);
    Task DeleteAsync(long id);
    Task DeleteRangeAsync(IEnumerable<T> models);

    Task<bool> AnyAsync(Expression<Func<T, bool>> query);

    Task SaveAsync();



  }
}
