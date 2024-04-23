using BT.Data;
using BT.Data.Entity.Abstract;
using BT.Repository.Abstract;

namespace BT.ServiceHelper.Abstract;
public abstract class BaseService<TRepository, TEntity> where TRepository : BaseRepository<TEntity>, new() where TEntity : class, IBaseEntity {
	private BTContext _dbContext;
	protected BTContext DbContext { get { _dbContext ??= new BTContext(); return _dbContext; } }
	protected TRepository Repository { get; set; }
	public BaseService() => Repository = (TRepository)Activator.CreateInstance(typeof(TRepository), DbContext);
	public virtual ResponseModel<IQueryable<TEntity>> GetAll() => Execute(Repository.GetAll);
	public virtual ResponseModel<TEntity> GetById(int id) => Execute(() => Repository.GetById(id));
	public virtual ServiceResponse Insert(TEntity entity) => ExecuteAction(() => Repository.Insert(entity));
	public virtual ResponseModel<int> InsertAndComplete(TEntity entity) => Execute(() => Repository.InsertAndComplete(entity));
	public virtual ServiceResponse Delete(int id) => ExecuteAction(() => Repository.Delete(id));
	public virtual ServiceResponse DeleteAndComplete(int id) => ExecuteAction(() => Repository.DeleteAndComplete(id));
	public virtual ServiceResponse Update(TEntity entity) => ExecuteAction(() => Repository.Update(entity));
	public virtual ServiceResponse UpdateAndComplete(TEntity entity) => ExecuteAction(() => Repository.UpdateAndComplete(entity));
	public virtual ServiceResponse Complete() => ExecuteAction(Repository.Complete);
	public virtual ResponseModel<T> Execute<T>(Func<T> action) {
		ResponseModel<T> response = new();
		try {
			response.Data = action();
		}
		catch (Exception ex) {
			response.Success = false;
			response.ErrorMessage = ex.Message;
		}
		return response;
	}
	public virtual ServiceResponse ExecuteAction(Action action) {
		ServiceResponse response = new();
		try {
			action();
		}
		catch (Exception ex) {
			response.Success = false;
			response.ErrorMessage = ex.Message;
		}
		return response;
	}
}

public class ResponseModel<T> {
	public bool Success { get; set; }
	public string Message { get; set; }
	public string ErrorMessage { get; set; }
	public T Data { get; set; }
}
public class ServiceResponse {
	public bool Success { get; set; }
	public string Message { get; set; }
	public string ErrorMessage { get; set; }
}