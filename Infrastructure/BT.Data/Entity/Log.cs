using BT.Data.Entity.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BT.Data.Entity;
[Table("TBL_LOG")]
public class Log : BaseEntity {
	[MaxLength(100)] public string YOL { get; set; }
	[MaxLength(50)] public string KULLANICI_IP { get; set; }
	public DateTime ISLEM_TARIHI { get; set; } = DateTime.Now;
	public bool HATA { get; set; } = false;
	public string HATAMESAJI { get; set; } = null;
}