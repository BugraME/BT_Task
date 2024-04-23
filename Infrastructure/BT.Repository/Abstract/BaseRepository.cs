using BT.Data.Entity.Abstract;
using Microsoft.EntityFrameworkCore;
namespace BT.Repository.Abstract;
public abstract class BaseRepository<TEntity> where TEntity : class, IBaseEntity {
	protected readonly DbContext DbContext;
	protected readonly DbSet<TEntity> DbSet;
	public BaseRepository() { }
	public BaseRepository(DbContext dbContext) { DbContext = dbContext; DbSet = DbContext.Set<TEntity>(); }
	public virtual int GetCount() => DbSet.AsNoTracking().Count();
	public virtual IQueryable<TEntity> GetAll() => DbSet.AsNoTracking();
	public virtual TEntity GetById(int id) => DbSet.AsNoTracking().FirstOrDefault(x => x.ID == id);
	public virtual void Insert(TEntity entity) => DbSet.Add(entity);
	public virtual int InsertAndComplete(TEntity entity) { Insert(entity); Complete(); return entity.ID; }
	public virtual void Delete(int id) => DbSet.Remove(DbSet.SingleOrDefault(x => x.ID == id));
	public virtual void DeleteAndComplete(int id) { Delete(id); Complete(); }
	public virtual void Update(TEntity entity) => DbContext.Update(entity);
	public virtual void UpdateAndComplete(TEntity entity) { Update(entity); Complete(); }
	public virtual void Complete() => DbContext.SaveChanges();
}