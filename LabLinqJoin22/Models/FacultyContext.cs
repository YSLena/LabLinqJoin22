using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging; // логирование
using System.IO;

#nullable disable

namespace LabLinqJoin22.Models
{
    public partial class FacultyContext : DbContext
    {
        public FacultyContext()
        {
        }

        public FacultyContext(DbContextOptions<FacultyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chair> Chairs { get; set; }
        public virtual DbSet<Curriculum> Curricula { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Tutor> Tutors { get; set; }

        // файл для логирования
        private readonly StreamWriter _logStream = new StreamWriter(@"D:\TEMP\DB\log.txt", append: false);

        // освободить файл перед уничтожением контекста, иначе логирование будет неполным
        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=magister-v;Initial Catalog=Faculty_UA_22;Integrated Security=True");

                optionsBuilder
                    .LogTo(_logStream.WriteLine, LogLevel.Trace) // в файл
                    .EnableSensitiveDataLogging(); // включать данные

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Ukrainian_CI_AS");

            modelBuilder.Entity<Chair>(entity =>
            {
                entity.ToTable("CHAIRS");

                entity.HasComment("Кафедры");

                entity.Property(e => e.ChairId)
                    .HasColumnName("CHAIR_ID")
                    .HasComment("PK");

                entity.Property(e => e.ChairHeadId)
                    .HasColumnName("CHAIR_HEAD_ID")
                    .HasComment("FK - Посилання на завідувача");

                entity.Property(e => e.ChairNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CHAIR_NUMBER")
                    .HasComment("Номер кафедри");

                entity.Property(e => e.DeputyDeanId)
                    .HasColumnName("DEPUTY_DEAN_ID")
                    .HasComment("FK - Посилання на замдекана");

                entity.HasOne(d => d.ChairHead)
                    .WithMany(p => p.ChairChairHeads)
                    .HasForeignKey(d => d.ChairHeadId)
                    .HasConstraintName("FK_CHAIR_HEAD");

                entity.HasOne(d => d.DeputyDean)
                    .WithMany(p => p.ChairDeputyDeans)
                    .HasForeignKey(d => d.DeputyDeanId)
                    .HasConstraintName("FK_CHAIR_VICE_DEAN");
            });

            modelBuilder.Entity<Curriculum>(entity =>
            {
                entity.ToTable("CURRICULUM");

                entity.HasComment("Учебный план");

                entity.Property(e => e.CurriculumId)
                    .HasColumnName("CURRICULUM_ID")
                    .HasComment("PK");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasComment("FK - Посилання на группу");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SUBJECT_ID")
                    .HasComment("FK - Посилання на предмет");

                entity.Property(e => e.TutorId)
                    .HasColumnName("TUTOR_ID")
                    .HasComment("FK - Посилання на препода");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Curricula)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_CURRICULUM_GROUP");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Curricula)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_CURRICULUM_SUBJECT");

                entity.HasOne(d => d.Tutor)
                    .WithMany(p => p.Curricula)
                    .HasForeignKey(d => d.TutorId)
                    .HasConstraintName("FK_CURRICULUM_TUTOR");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("GROUPS");

                entity.HasComment("Группы");

                entity.HasIndex(e => e.GroupNumber, "U_GROUP_NUMBER")
                    .IsUnique();

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasComment("PK");

                entity.Property(e => e.ChairId)
                    .HasColumnName("CHAIR_ID")
                    .HasComment("FK - Посилання на кафедру");

                entity.Property(e => e.CuratorId)
                    .HasColumnName("CURATOR_ID")
                    .HasComment("FK - Посилання на куратора");

                entity.Property(e => e.GroupNumber)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("GROUP_NUMBER")
                    .HasComment("Номер группы");

                entity.Property(e => e.LabStudies)
                    .HasColumnName("LAB_STUDIES")
                    .HasComment("Кількість лаб");

                entity.Property(e => e.PractStudies)
                    .HasColumnName("PRACT_STUDIES")
                    .HasComment("Кількість практик");

                entity.Property(e => e.SeniorStudentId)
                    .HasColumnName("SENIOR_STUDENT_ID")
                    .HasComment("FK - Посилання на старосту");

                entity.Property(e => e.StudyHours)
                    .HasColumnName("STUDY_HOURS")
                    .HasComment("Обсяг занять в годинах");

                entity.HasOne(d => d.Chair)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.ChairId)
                    .HasConstraintName("FK_GROUPS_CHAIRS");

                entity.HasOne(d => d.Curator)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.CuratorId)
                    .HasConstraintName("FK_GROUPS_CURATOR");

                entity.HasOne(d => d.SeniorStudent)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.SeniorStudentId)
                    .HasConstraintName("FK_GROUPS_SENIOR_STUDENT");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.ScheduleId)
                    .HasColumnName("Schedule_Id")
                    .HasComment("PK");

                entity.Property(e => e.CurriculumId)
                    .HasColumnName("CURRICULUM_ID")
                    .HasComment("FK - Посилання на пункт навч. плану");

                entity.Property(e => e.LessonDate)
                    .HasColumnType("date")
                    .HasColumnName("LESSON_DATE")
                    .HasComment("Дата заняття");

                entity.Property(e => e.LessonFinish)
                    .HasColumnName("LESSON_FINISH")
                    .HasComment("Час закінчення");

                entity.Property(e => e.LessonStart)
                    .HasColumnName("LESSON_START")
                    .HasComment("Час початку");

                entity.Property(e => e.Pair)
                    .HasColumnName("PAIR")
                    .HasComment("Пара за розкладом");

                entity.HasOne(d => d.Curriculum)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.CurriculumId)
                    .HasConstraintName("FK_Schedule_CURRICULUM");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("STUDENTS");

                entity.HasComment("Студенты");

                entity.Property(e => e.StudentId)
                    .HasColumnName("STUDENT_ID")
                    .HasComment("PK");

                entity.Property(e => e.Absences)
                    .HasColumnName("ABSENCES")
                    .HasComment("Пропуски занять в годинах");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasComment("FK - Посилання на групу");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NAME")
                    .HasComment("Им'я");

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PATRONYMIC")
                    .HasComment("По батькові");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SURNAME")
                    .HasComment("Прізвище");

                entity.Property(e => e.UnreadyLabs)
                    .HasColumnName("UNREADY_LABS")
                    .HasComment("Нездані лабораторні");

                entity.Property(e => e.UnreasonableAbsences)
                    .HasColumnName("UNREASONABLE_ABSENCES")
                    .HasComment("Пропуски без поваж. причин в годинах");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_STUDENT_GROUP");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("SUBJECTS");

                entity.HasComment("Предметы");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SUBJECT_ID")
                    .HasComment("PK");

                entity.Property(e => e.ChairExternal)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CHAIR_EXTERNAL")
                    .HasComment("Номер зовннішньої кафедри");

                entity.Property(e => e.ChairId)
                    .HasColumnName("CHAIR_ID")
                    .HasComment("FK - Посилання на кафедру (для ф-та № 3)");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME")
                    .HasComment("Назва дисципліни");
            });

            modelBuilder.Entity<Tutor>(entity =>
            {
                entity.ToTable("TUTORS");

                entity.HasComment("Преподаватели");

                entity.Property(e => e.TutorId)
                    .HasColumnName("TUTOR_ID")
                    .HasComment("PK");

                entity.Property(e => e.ChairExternal)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CHAIR_EXTERNAL")
                    .HasComment("Номер кафедри (для зовнішніх)");

                entity.Property(e => e.ChairId)
                    .HasColumnName("CHAIR_ID")
                    .HasComment("FK - Посилання на кафедру (для ф-та № 3)");

                entity.Property(e => e.Faculty)
                    .HasColumnName("FACULTY")
                    .HasComment("Факультет");

                entity.Property(e => e.NameFio)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NAME_FIO")
                    .HasComment("Прізвище І.П.");

                entity.Property(e => e.Position)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("POSITION")
                    .HasComment("Посада (необов'язкове)");

                entity.HasOne(d => d.Chair)
                    .WithMany(p => p.Tutors)
                    .HasForeignKey(d => d.ChairId)
                    .HasConstraintName("FK_TUTORS_CHAIR");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
