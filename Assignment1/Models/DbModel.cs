namespace Assignment1.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DbModel : DbContext
    {
        public DbModel()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Test> Tests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>()
                .Property(e => e.Answer1)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.QuestionText)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionType>()
                .Property(e => e.QuestionType1)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionType>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.QuestionType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Test>()
                .Property(e => e.author)
                .IsUnicode(false);

            modelBuilder.Entity<Test>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Test>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Test>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.Test)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Answer>()
            .Property(e => e.Photo)
            .IsUnicode(false);

            modelBuilder.Entity<Question>()
            .Property(e => e.Photo)
            .IsUnicode(false);
        }
    }
}
