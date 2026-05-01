using Microsoft.EntityFrameworkCore;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Data
{
    public class OmiaDbContext : DbContext
    {
        public OmiaDbContext(DbContextOptions<OmiaDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Institute> Institutes { get; set; }
        public DbSet<UserToken> Tokens { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
        public DbSet<AssistantCourse> AssistantCourses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseContent> CourseContents { get; set; }
        public DbSet<LiveSession> LiveSessions { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<QuizAnswer> QuizAnswers { get; set; }
        public DbSet<CourseDiscussion> CourseDiscussions { get; set; }
        public DbSet<CourseProgress> CourseProgresses { get; set; }
        public DbSet<CourseContentComment> CourseContentComments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TPH for BaseUserEntity
            /*
            modelBuilder.Entity<BaseUserEntity>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Admin>("Admin")
                .HasValue<Teacher>("Teacher")
                .HasValue<Assistant>("Assistant")
                .HasValue<Student>("Student")
                .HasValue<Parent>("Parent")
                .HasValue<Institute>("Institute");
            */
            // Configure TPT for BaseUserEntity
            modelBuilder.Entity<BaseUserEntity>().ToTable("Users");

            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Teacher>().ToTable("Teachers");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Parent>().ToTable("Parents");
            modelBuilder.Entity<Institute>().ToTable("Institutes");

            // Apply all configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OmiaDbContext).Assembly);

            // Add global query filter for IsDeleted
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Only apply to the root of the inheritance hierarchy
                if (entityType.BaseType == null && typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                    var propertyMethodInfo = typeof(EF).GetMethod("Property")?.MakeGenericMethod(typeof(bool));
                    var isDeletedProperty = System.Linq.Expressions.Expression.Call(null, propertyMethodInfo!, parameter, System.Linq.Expressions.Expression.Constant("IsDeleted"));
                    var compareExpression = System.Linq.Expressions.Expression.MakeBinary(System.Linq.Expressions.ExpressionType.Equal, isDeletedProperty, System.Linq.Expressions.Expression.Constant(false));
                    var lambda = System.Linq.Expressions.Expression.Lambda(compareExpression, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
    }
}
