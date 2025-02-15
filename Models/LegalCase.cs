using System;
using System.ComponentModel.DataAnnotations;

namespace LegalCaseManagement.Models
{
    public class LegalCase
    {
       
        public int Id { get; set; }
        public string CaseTitle { get; set; }
        public string Description { get; set; }
        public string AssignedLawyer { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate {  get; set; }
    }
}
