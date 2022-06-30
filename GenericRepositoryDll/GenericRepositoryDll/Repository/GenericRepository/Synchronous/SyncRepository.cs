using GenericReositoryDll.Enumrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepositoryDll.Repository.GenericRepository;

  public partial class Repository<T> where T : class
  {

    private string _name;
    public void Add(T model)
    {
      _model.Add(model);
      Save();

    }
    public void AddRange(IEnumerable<T> models)
    {
      _model.AddRange(models);
      Save();
    }
    public bool Any(Expression<Func<T, bool>> query)
    => _model.Any(query);
    public void Delete(long id)
    {
      T model = _context.Find<T>(id);
      if (model != null) _model.Remove(model);
      Save();
    }
    public void DeleteRange(IEnumerable<T> models)
    {
      _model.RemoveRange(models);
      Save();
    }
    public long GetCount(Expression<Func<T, bool>> query = null)
     => _model.Count(query);
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
    public List<T> GetList<T>(Expression<Func<T, bool>> query = null,
      Func<T, object> orderBy = null,
      OrderType? orderType = null,
      List<string> includes = null, int? skip = 0,
      int? take = null, bool? distinct = null)
    => (List<T>)GetList(query, orderBy, orderType, includes, skip, take, distinct);
    public T Find(long id)
     => _model.Find(id);

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

    public T GetSingle<T>(Expression<Func<T, bool>> query,
      List<string> includes = null)
    => (T)GetSingle(query, includes);

    public void Update(T model)
    {
      _model.Update(model);
      Save();
    }
    public void UpdateRange(IEnumerable<T> models)
    {
      _model.UpdateRange(models);
      Save();
    }
    public void Save()
    => _context.SaveChanges();

  }

