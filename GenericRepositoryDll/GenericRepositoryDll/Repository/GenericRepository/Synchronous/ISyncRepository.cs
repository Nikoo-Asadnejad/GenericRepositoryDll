using GenericReositoryDll.Enumrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepositoryDll.Repository.GenericRepository;

  public partial interface IRepository<T>
  {
    T Find(long id);
    object GetSingle(Expression<Func<T, bool>> query,
    Func<T, object> selector,
    List<string> includes = null);
    T GetSingle<T>(Expression<Func<T, bool>> query,
    List<string> includes = null);
    List<object> GetList(Expression<Func<T, bool>> query = null,
                            Func<T, object> selector = null,
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
    long GetCount(Expression<Func<T, bool>> query = null);
    void Add(T model);
    void AddRange(IEnumerable<T> models);
    void Update(T model);
    void UpdateRange(IEnumerable<T> models);
    void Delete(long id);
    void DeleteRange(IEnumerable<T> models);
    bool Any(Expression<Func<T, bool>> query);
    void Save();
  }

