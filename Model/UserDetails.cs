﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserDetails
    {
        public string? AccountId { get; set; }
        public bool? IsInstructor { get; set; }
        public bool? IsStudent { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsGuest { get; set; }
        public string Email { get; set; }
        public string? FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ContactNo { get; set; }
        public string? Education { get; set; }
        public string? SkillSet { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime JoingDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public bool? IsLeaving { get; set; }
        public string ProfileDes { get; set; }
        public string ProfileImg { get; set; }

        public string? CreatedBy { get; set; }
        public string? ModifiedBy{ get; set; }
        public string AccounType { get; set; }
        public string StudentCode { get; set; }
        public string? InstructorCode { get; set; }
    }
}