using Microsoft.EntityFrameworkCore;
using Comp.Survey.Core.Entities;

namespace Comp.Survey.Infrastructure.Data
{
    public sealed class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext() { }

        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }
        
        public DbSet<Core.Entities.Survey> Surveys { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }

        public DbSet<CompUser> CompUsers { get; set; }
        public DbSet<CompUserSurvey> CompUserSurveys { get; set; }
        public DbSet<CompUserSurveyDetail> CompUserSurveyDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // SurveyQuestion
            modelBuilder.Entity<SurveyQuestion>()
                .HasOne<Core.Entities.Survey>()
                .WithMany(p => p.SurveyQuestions)
                .HasForeignKey(p => p.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);

            // QuestionOption
            modelBuilder.Entity<QuestionOption>()
                .HasOne<SurveyQuestion>()
                .WithMany(p => p.QuestionOptions)
                .HasForeignKey(p => p.SurveyQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // CompUserSurvey
            modelBuilder.Entity<CompUserSurvey>()
                .HasOne<CompUser>()
                .WithMany(p => p.CompUserSurveys)
                .HasForeignKey(p => p.CompUserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<CompUserSurvey>()
                .HasOne<Core.Entities.Survey>()
                .WithMany(p => p.CompUserSurveys)
                .HasForeignKey(p => p.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);

            // CompUserSurveyDetail
            modelBuilder.Entity<CompUserSurveyDetail>()
                .HasOne<CompUserSurvey>()
                .WithMany(p => p.CompUserSurveyDetails)
                .HasForeignKey(p => p.CompUserSurveyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompUserSurveyDetail>()
                .HasOne<SurveyQuestion>()
                .WithMany(p => p.CompUserSurveyDetails)
                .HasForeignKey(p => p.SurveyQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompUserSurveyDetail>()
                .HasOne<QuestionOption>()
                .WithMany(p => p.CompUserSurveyDetails)
                .HasForeignKey(p => p.SelectedOptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
