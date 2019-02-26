namespace Assignment1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            Answers = new HashSet<Answer>();
        }
        [Display(Name = "QuestionId")]
        public int Id { get; set; }

        [Display(Name = "TestId")]
        public int TestId { get; set; }

        [Required]
        [Display(Name = "Question")]
        public string QuestionText { get; set; }

        [Display(Name = "QuestionNumber")]
        public int QuestionNumber { get; set; }

        [Display(Name = "QuestionType")]
        public int QuestionTypeId { get; set; }

        [StringLength(255)]
        [Display(Name = "Image")]
        public string Photo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }

        public virtual QuestionType QuestionType { get; set; }

        public virtual Test Test { get; set; }
    }
}
