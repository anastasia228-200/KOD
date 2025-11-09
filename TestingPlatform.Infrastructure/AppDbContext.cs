using Microsoft.EntityFrameworkCore;
using TestingPlatform.domain.Models;

namespace TestingPlatform.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Direction> Directions => Set<Direction>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<Attempt> Attempts => Set<Attempt>();
    public DbSet<UserAttemptsAnswer> UserAttemptAnswers => Set<UserAttemptsAnswer>();
    public DbSet<UserSelectedOption> UserSelectedOptions => Set<UserSelectedOption>();
    public DbSet<UserTextAnswer> UserTextAnswers => Set<UserTextAnswer>();
    public DbSet<TestResult> TestResults => Set<TestResult>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Login).IsUnique();
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Login).IsRequired();
            e.Property(x => x.Email).IsRequired();
            e.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(50);
            e.Property(x => x.MiddleName).IsRequired(false);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            
            e.HasOne(x => x.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Student>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Phone).HasMaxLength(30).IsRequired(false);
            e.Property(x => x.VkProfileLink).IsRequired(false);
            e.Property(x => x.UserId).IsRequired(); 
        });

        modelBuilder.Entity<Direction>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Course>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Project>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Group>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();

            e.HasOne(x => x.Direction)
                .WithMany(d => d.Groups)
                .HasForeignKey(x => x.DirectionId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Course)
                .WithMany(c => c.Groups)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Project)
                .WithMany(p => p.Groups)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Test>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Title).IsRequired().HasMaxLength(200);
            e.Property(t => t.Description).IsRequired().HasMaxLength(1000);
            e.Property(t => t.IsRepeatable).HasDefaultValue(false);
            e.Property(t => t.Type).HasConversion<string>().HasMaxLength(20);
            e.Property(t => t.AnswerType).HasConversion<string>().HasMaxLength(20);
            e.Property(t => t.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.Property(t => t.PublishedAt).IsRequired();
            e.Property(t => t.DeadLine).IsRequired();
            e.Property(t => t.DurationMinutes).IsRequired(false);
            e.Property(t => t.IsPublic).HasDefaultValue(false);
            e.Property(t => t.PassingScore).IsRequired(false);
            e.Property(t => t.MaxAttempts).IsRequired(false);

            e.HasMany(x => x.Groups)
            .WithMany(g => g.Tests)
            .UsingEntity(j => j.ToTable("test_groups"));

            e.HasMany(x => x.Projects)
            .WithMany(p => p.Tests)
            .UsingEntity(j => j.ToTable("test_projects"));

            e.HasMany(x => x.Courses)
            .WithMany(c => c.Tests)
            .UsingEntity(j => j.ToTable("test_courses"));

            e.HasMany(x => x.Directions)
            .WithMany(d => d.Tests)
            .UsingEntity(j => j.ToTable("test_directions"));
        });

        modelBuilder.Entity<Question>(e =>
        {
            e.HasKey(q => q.Id);
            e.Property(q => q.Text).IsRequired().HasMaxLength(4000);
            e.Property(q => q.Number).IsRequired();
            e.Property(q => q.Description).HasMaxLength(2000);
            e.Property(q => q.AnswerType).HasConversion<string>().HasMaxLength(50);
            e.Property(q => q.IsScoring).HasDefaultValue(true);
            e.Property(q => q.MaxScore).IsRequired(false);
            e.Property(q => q.TestId).IsRequired();

            // Связь с Test (many:1 с каскадом) - ТОЛЬКО ОДИН РАЗ
            e.HasOne(q => q.Test)
                .WithMany(t => t.Questions)
                .HasForeignKey(q => q.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Уникальный составной индекс (TestId, Number)
            e.HasIndex(q => new { q.TestId, q.Number })
                .IsUnique();
        });

        modelBuilder.Entity<Answer>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Text).IsRequired().HasMaxLength(1000);
            e.Property(a => a.IsCorrect).IsRequired();
            e.Property(a => a.QuestionId).IsRequired();

            // Связь с Question (many:1 с каскадом)
            e.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Attempt>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.StartedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            //e.Property(a => a.SubmittedAt).IsRequired(false);
            e.Property(a => a.Score).IsRequired(false);
            e.Property(a => a.TestId).IsRequired();
            e.Property(a => a.StudentId).IsRequired();

            // Связь с Test (many:1 с Restrict)
            e.HasOne(a => a.Test)
                .WithMany()
                .HasForeignKey(a => a.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с Student (many:1 с Cascade)
            e.HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserAttemptsAnswer>(e =>
        {
            e.HasKey(ua => ua.Id);
            e.Property(ua => ua.IsCorrect).IsRequired();
            e.Property(ua => ua.ScoreAwarded).IsRequired();
            e.Property(ua => ua.AttemptId).IsRequired();
            e.Property(ua => ua.QuestionId).IsRequired();

            // Связь с Attempt (many:1 с каскадом)
            e.HasOne(ua => ua.Attempt)
                .WithMany(a => a.UserAttemptAnswers)
                .HasForeignKey(ua => ua.AttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с Question (many:1 с Restrict)
            e.HasOne(ua => ua.Question)
                .WithMany(q => q.UserAttemptsAnswer)
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Уникальный составной индекс (AttemptId, QuestionId)
            e.HasIndex(ua => new { ua.AttemptId, ua.QuestionId })
                .IsUnique();
        });

        // UserSelectedOption
        modelBuilder.Entity<UserSelectedOption>(e =>
        {
            e.HasKey(uso => uso.Id);
            e.Property(uso => uso.UserAttemptAnswerId).IsRequired();
            e.Property(uso => uso.AnswerId).IsRequired();

            // Связь с UserAttemptAnswer (many:1 с каскадом)
            e.HasOne(uso => uso.UserAttemptAnswer)
                .WithMany(uaa => uaa.UserSelectedOptions)
                .HasForeignKey(uso => uso.UserAttemptAnswerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с Answer (many:1 с Restrict)
            e.HasOne(uso => uso.Answer)
                .WithMany()
                .HasForeignKey(uso => uso.AnswerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // UserTextAnswer
        modelBuilder.Entity<UserTextAnswer>(e =>
        {
            e.HasKey(uta => uta.Id);
            e.Property(uta => uta.TextAnswer).IsRequired().HasMaxLength(4000);
            e.Property(uta => uta.UserAttemptAnswerId).IsRequired();

            // Связь 1:1 с UserAttemptAnswer с каскадом
            e.HasOne(uta => uta.UserAttemptAnswer)
                .WithOne(uaa => uaa.UserTextAnswers)
                .HasForeignKey<UserTextAnswer>(uta => uta.UserAttemptAnswerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // TestResult
        modelBuilder.Entity<TestResult>(e =>
        {
            e.HasKey(tr => tr.Id);
            e.Property(tr => tr.Passed).IsRequired();
            e.Property(tr => tr.TestId).IsRequired();
            e.Property(tr => tr.AttemptId).IsRequired();
            e.Property(tr => tr.StudentId).IsRequired();

            // Связь с Test (many:1 с Restrict)
            e.HasOne(tr => tr.Test)
                .WithMany()
                .HasForeignKey(tr => tr.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с Attempt (many:1 с каскадом)
            e.HasOne(tr => tr.Attempt)
                .WithMany()
                .HasForeignKey(tr => tr.AttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с Student (many:1 с каскадом)
            e.HasOne(tr => tr.Student)
                .WithMany()
                .HasForeignKey(tr => tr.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Уникальный составной индекс (TestId, StudentId, AttemptId)
            e.HasIndex(tr => new { tr.TestId, tr.StudentId, tr.AttemptId })
                .IsUnique();
        });
    }
}