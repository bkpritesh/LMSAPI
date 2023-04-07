using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Students
{
	public class RequestRegister
	{

		public string FName { get; set; }
		public string MName { get; set; }
		public string LName { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string? State { get; set; }
		public string? City { get; set; }
		public string? Country { get; set; }
		public string ContactNo { get; set; }
		public string Education { get; set; }
		public string SkillSet { get; set; }
		public DateTime? BirthDate { get; set; }
		public DateTime? JoiningDate { get; set; }
		public string AccountType { get; set; }
		public string CategoryCode { get; set; }
		public string  CourseCode { get; set; }
		public decimal CourseFees { get; set; }
		public decimal Discount { get; set; }
		public decimal TotalFees { get; set; }
		public bool IsPaid { get; set; }
		public decimal PaidAmount { get; set; }
		public bool IsStudent { get; set; }

	}
}
