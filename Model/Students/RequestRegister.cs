using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Students
{
	public class RequestRegister
	{
		public string Fname { get; set; }
		public string MName { get; set; }
		public string LName { get; set; }
		public string Address { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string County { get; set; }
		public string ContactNo { get; set; }
		public string Education { get; set; }
		public string SkillSet { get; set; }
		public string BrithDate { get; set; }
		public string JoingDate { get; set; }
		public string AdmissionDate { get; set; }
		public string AccounType { get; set; }
		public string CourseCode { get; set; }
		public decimal Price { get; set; }
		public decimal discount { get; set; }
		public decimal TotalFee { get; set; }
		public bool IsPaid { get; set; }
		public decimal PaidAmount { get; set; }
		public bool IsStudent { get; set; }

	}
}
