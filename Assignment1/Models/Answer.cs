namespace Assignment1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Answer
    {
        [Display(Name = "AnswerId")]
        public int Id { get; set; }

        [Display(Name = "QuestionId")]
        public int QuestionId { get; set; }

        [Column("Answer")]
        [Required]
        [Display(Name = "Answer")]
        public string Answer1 { get; set; }

        [Display(Name = "Score")]
        public double Score { get; set; }

        [StringLength(255)]
        [Display(Name = "Image")]
        public string Photo { get; set; }

        public virtual Question Question { get; set; }
    }
}
