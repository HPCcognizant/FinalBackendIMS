﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceManagementSystem.Models
{
    public class Claim
    {
        [Key]
        public int ClaimID { get; set; }

        [ForeignKey("Policy")]
        public int PolicyID { get; set; }

        [ForeignKey("Customer")]
        public int Customer_ID { get; set; }
        public decimal ClaimAmount { get; set; }
        public string? Status { get; set; } = "Filed";
        public string? ClaimReason { get; set; }
        public string? AdminReason { get; set; }
        public decimal RemainigCAmount { get; set; } = 0;

        //Navigation Property
        public Policy? Policy { get; set; }

        public Customer? Customer { get; set; }

    }
}
