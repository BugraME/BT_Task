using BT.Data;
using BT.Data.Entity;
using BT.Repository.Abstract;
namespace BT.Repository;
public class LogRepository : BaseRepository<Log> {
	public LogRepository() { }
	public LogRepository(BTContext dbContext) : base(dbContext) { }
}