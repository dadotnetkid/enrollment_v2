
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Models;
using Newtonsoft.Json;

namespace Models.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal ModelDb context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ModelDb context)
        {
            this.context = context;
            context.Database.Log = (s) => { Debug.WriteLine(s); };
            this.dbSet = context.Set<TEntity>();

        }

        public void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            context.Database.ExecuteSqlCommand(sql, parameters);
        }
        public DbRawSqlQuery<TEntity> SqlQuery(string sql, params object[] parameters)
        {
            var res = context.Database.SqlQuery<TEntity>(sql, parameters);
            return res;
        }
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
                //return orderBy(query);
            }
            else
            {
                return query.ToList();
                //return query;
            }
        }
        public virtual IEnumerable<object> Get(
             Expression<Func<TEntity, object>> selector)
        {
            IQueryable<TEntity> query = dbSet;


            return query.Select(selector).ToList();
        }
        public virtual IQueryable<TEntity> Fetch(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (includeProperties != "")
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);
            if (filter != null)
                query = query.Where(filter);

            return query;
        }

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }
        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }
        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
                query = query.Where(filter);
            return await query.ToListAsync();
        }
        public virtual TEntity Find(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (includeProperties != "")
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

            return query.Where(filter).FirstOrDefault();
        }
        public virtual TEntity Find(string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (includeProperties != "")
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

            return query.FirstOrDefault();
        }
        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter)
        {

            return await dbSet.Where(filter).FirstOrDefaultAsync();
        }
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void InsertRange(IEnumerable<TEntity> entity)
        {
            dbSet.AddRange(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }
        public virtual void Delete(Expression<Func<TEntity, bool>> filter)
        {
            dbSet.RemoveRange(dbSet.Where(filter));
            TrackDeletedEntities(filter);
        }

        public virtual void DeleteRange(Expression<Func<TEntity, bool>> filter)
        {
            dbSet.RemoveRange(dbSet.Where(filter));

        }
        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            var res = await this.FindAsync(filter);
            dbSet.Attach(res);
            dbSet.Remove(res);
            return await context.SaveChangesAsync();
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            var entity = await dbSet.Where(filter).FirstOrDefaultAsync();
            return entity;
        }
        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            dbSet.Add(entity);
            return await context.SaveChangesAsync();
        }
        public virtual async Task<TEntity> CloneAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await dbSet.Where(filter).AsNoTracking().FirstOrDefaultAsync();
        }
        public virtual TEntity Clone(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (includeProperties != "")
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);
            return query.Where(filter).AsNoTracking().FirstOrDefault();
        }
        public void Detach(TEntity entity)
        {
            dbSet.Remove(entity);
        }
        public void DetachRange(IEnumerable<TEntity> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public void TrackModifiedEntities(Expression<Func<TEntity, bool>> filter, TEntity newItem)
        {
            context.Configuration.LazyLoadingEnabled = false;
            context.Configuration.ProxyCreationEnabled = false;
            var unitOfWork = new UnitOfWork();
            var oldItem = Find(filter);


            string oldObj = "";
            string newObj = "";

            foreach (var i in newItem.GetType().GetProperties())
            {
                try
                {
                    if (newItem.GetType().GetProperty(i.Name)?.GetValue(newItem, null).ToString()
                            .Equals(oldItem.GetType().GetProperty(i.Name)?.GetValue(oldItem, null).ToString()) == false && newItem.GetType().GetProperty(i.Name)?.GetValue(newItem, null).ToString().Contains("System.Collections") == false)
                    {
                        oldObj += i.Name + "=" + oldItem.GetType().GetProperty(i.Name)?.GetValue(oldItem, null) + "<br/>";
                        newObj += i.Name + "=" + newItem.GetType().GetProperty(i.Name)?.GetValue(newItem, null) + "<br/>";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }




            if (!oldItem.Equals(newItem))
            {
                var logs = new Logs()
                {
                    NewValues = newObj.ToString(),
                    OldValues = oldObj.ToString(),
                    DateCreated = DateTime.Now,
                    TableName = newItem.GetType().Name,
                    CreatedBy = HttpContext.Current.User.Identity.GetUserId(),
                    Action = "Update",

                };
                unitOfWork.LogsRepo.Insert(logs);
                unitOfWork.Save();
                context.Entry(oldItem).State = EntityState.Detached;
            }

            context.Configuration.LazyLoadingEnabled = true;
            context.Configuration.ProxyCreationEnabled = true;
        }
        public void TrackDeletedEntities(Expression<Func<TEntity, bool>> filter = null)
        {
            context.Configuration.LazyLoadingEnabled = false;
            context.Configuration.ProxyCreationEnabled = false;

            var old = Find(filter);
            var unitOfWork = new UnitOfWork();

            string oldObj = "";

            foreach (var i in old.GetType().GetProperties())
            {
                try
                {
                    if (Convert.ToString(old.GetType().GetProperty(i.Name)?.GetValue(old, null)).Contains("System.Collections") == false)
                        oldObj += i.Name + "=" + Convert.ToString(old.GetType().GetProperty(i.Name)?.GetValue(old, null)) + "<br/>";

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }



            unitOfWork.LogsRepo.Insert(new Logs()
            {
                OldValues = oldObj,
                DateCreated = DateTime.Now,
                TableName = old.GetType().Name.Split('_')[0],
                CreatedBy = HttpContext.Current.User.Identity.GetUserId(),
                Action = "Delete",
            });
            unitOfWork.Save();

            context.Configuration.LazyLoadingEnabled = true;
            context.Configuration.ProxyCreationEnabled = true;
        }
    }


}