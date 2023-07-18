using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.template.core.Model;

namespace amorphie.template.data;

public static class DbInitializer
{
    public static void Initialize(TemplateDbContext context)
    {
        context.Database.EnsureCreated();

        // Look for any students.
        if (context.Students.Any())
        {
            return; // DB has been seeded
        }

        var students = new Student[]
        {
            new Student
            {
                FirstMidName = "Carson",
                LastName = "Alexander",
                EnrollmentDate = DateTime.Parse("2005-09-01").ToUniversalTime()
            },
            new Student
            {
                FirstMidName = "Meredith",
                LastName = "Alonso",
                EnrollmentDate = DateTime.Parse("2002-09-01").ToUniversalTime()
            },
            new Student
            {
                FirstMidName = "Arturo",
                LastName = "Anand",
                EnrollmentDate = DateTime.Parse("2003-09-01").ToUniversalTime()
            },
            new Student
            {
                FirstMidName = "Gytis",
                LastName = "Barzdukas",
                EnrollmentDate = DateTime.Parse("2002-09-01").ToUniversalTime()
            },
            new Student
            {
                FirstMidName = "Yan",
                LastName = "Li",
                EnrollmentDate = DateTime.Parse("2002-09-01").ToUniversalTime()
            },
            new Student
            {
                FirstMidName = "Peggy",
                LastName = "Justice",
                EnrollmentDate = DateTime.Parse("2001-09-01").ToUniversalTime()
            },
            new Student
            {
                FirstMidName = "Laura",
                LastName = "Norman",
                EnrollmentDate = DateTime.Parse("2003-09-01").ToUniversalTime()
            },
            new Student
            {
                FirstMidName = "Nino",
                LastName = "Olivetto",
                EnrollmentDate = DateTime.Parse("2005-09-01").ToUniversalTime()
            }
        };
        foreach (Student s in students)
        {
            context.Students.Add(s);
        }
        context.SaveChanges();

        var courses = new Course[]
        {
            new Course
            {
                Id = Guid.NewGuid(),
                Title = "Chemistry",
                Credits = 3
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Title = "Microeconomics",
                Credits = 3
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Title = "Macroeconomics",
                Credits = 3
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Title = "Calculus",
                Credits = 4
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Title = "Trigonometry",
                Credits = 4
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Title = "Composition",
                Credits = 3
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Title = "Literature",
                Credits = 4
            }
        };
        foreach (Course c in courses)
        {
            context.Courses.Add(c);
        }
        context.SaveChanges();

        var enrollments = new Enrollment[]
        {
            new Enrollment
            {
                Student = students[0],
                Course = courses[0],
                Grade = Grade.A
            },
            new Enrollment
            {
                Student = students[0],
                Course = courses[2],
                Grade = Grade.C
            },
            new Enrollment
            {
                Student = students[0],
                Course = courses[3],
                Grade = Grade.B
            },
            new Enrollment
            {
                Student = students[1],
                Course = courses[1],
                Grade = Grade.B
            },
            new Enrollment
            {
                Student = students[1],
                Course = courses[4],
                Grade = Grade.F
            },
            new Enrollment
            {
                Student = students[1],
                Course = courses[4],
                Grade = Grade.F
            },
            new Enrollment { Student = students[2], Course = courses[0] },
            new Enrollment { Student = students[3], Course = courses[0] },
            new Enrollment
            {
                Student = students[3],
                Course = courses[2],
                Grade = Grade.F
            },
            new Enrollment
            {
                Student = students[4],
                Course = courses[3],
                Grade = Grade.C
            },
            new Enrollment { Student = students[5], Course = courses[1] },
            new Enrollment
            {
                Student = students[6],
                Course = courses[4],
                Grade = Grade.A
            },
        };
        foreach (Enrollment e in enrollments)
        {
            context.Enrollments.Add(e);
        }
        context.SaveChanges();
    }
}
