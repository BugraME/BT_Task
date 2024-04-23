using System.ComponentModel.DataAnnotations;

namespace BT.Data.Entity.Abstract;
public abstract class BaseEntity : IBaseEntity {
	[Key] public int ID { get; set; }
}