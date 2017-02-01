using System;
using Realms;

namespace SaveTheBill.Free.Model
{
	public class Bill : RealmObject
	{
		[PrimaryKey]
		public int Id { get; set; }
		public string Name { get; set; }
		public string ImageSource { get; set; }
		public string Amount { get; set; }
		public DateTimeOffset ScanDate { get; set; }
		public bool HasGuarantee { get; set; }
		public DateTimeOffset GuaranteeExpireDate { get; set; }
		public int NotifyTime { get; set; }
		public string Location { get; set; }
		public string Additions { get; set; }
	}
}
