﻿using System;
 using System.Collections.Generic;
using System.Linq;
 using apdb10._2.Entities;
 using Apdb9.Models;
using Microsoft.EntityFrameworkCore;

 namespace Apdb9.Services
 {
     public class StudentService : IStudentService
     {
         private readonly StudentContext _studentContext;

         public StudentService(StudentContext context)
         {
             _studentContext = context;
         }

         public List<GetStudentsResponse> GetStudents()
         {
             var students = _studentContext.Student
                 .Include(s => s.IdEnrollmentNavigation)
                 .ThenInclude(s => s.IdStudyNavigation);
             return students.Select(s => new GetStudentsResponse
             {
                 FirstName = s.FirstName,
                 LastName = s.LastName,
                 BirthDate = s.BirthDate.ToString(),
                 IndexNumber = s.IndexNumber,
                 Semester = s.IdEnrollmentNavigation.Semester,
                 Studies = s.IdEnrollmentNavigation.IdStudyNavigation.Name
             }).ToList();

         }


         public bool AddStudent(Student student)
         {
             try
             {
                 var entry = _studentContext.Add(student);
                 _studentContext.SaveChanges();
                 return true;
             }
             catch (Exception e)
             {
                 Console.WriteLine(e.Message);
                 return false;
             }



         }

         public bool UpdateStudent(Student student)
         {
             try
             {
                 var a = _studentContext.Update(student);
                 _studentContext.SaveChanges();
                 return true;
             }
             catch (Exception e)
             {
                 Console.WriteLine(e.Message);
                 return false;
             }

         }

         public bool DeleteStudent(Student student)
         {
             try
             {
                 _studentContext.Remove(student);
                 _studentContext.SaveChanges();
                 return true;
             }
             catch (Exception e)
             {
                 Console.WriteLine(e.Message);
                 return false;
             }

         }

         public bool EnrollStudent(StudentEnrollRequest s)
         {
             var idStudies = GetIdStudies(s.Studies);
             if (idStudies == -1)
             {
                 return false;
             }
             var idEnrollment = GetIdEnrollment(idStudies);
             if (idEnrollment == -1)
             {
                 idEnrollment = GetNextIdEnrollment();
             }

             try
             {
                 InsertEnrollment(idEnrollment, idStudies);
                 AddStudentToEnrollment(s.IndexNumber, s.FirstName, s.LastName, s.BirthDate, idEnrollment);
             }
             catch (Exception)
             {
                 return false;
             }

             return true;
         }

        

         private int GetIdEnrollment(int idStudies)
         {
             int id;
             var enrollments = _studentContext.Enrollment;
             try
             {
                  id = enrollments.First(e => e.IdStudy == idStudies && e.Semester == 1).IdEnrollment;
             }
             catch (Exception)
             {
                  id = -1;
             }

             return id;
         }

         private int GetIdStudies(string name)
         {
             int id;
             var studies = _studentContext.Studies;
             try
             {
                 id = studies.First(e => e.Name == name).IdStudy;
             }
             catch(Exception)
             {
                 id = -1;
             }

             return id;
         }

         private int GetNextIdEnrollment()
         {
             var enrollment = _studentContext.Enrollment;
             return enrollment.Max().IdEnrollment + 1;
         }

         private void InsertEnrollment(int idEnrollment, int idStudies)
         {

             var enrollment = new Enrollment
             {
                 IdEnrollment = idEnrollment,
                 Semester = 1,
                 IdStudy = idStudies,
                 StartDate = DateTime.Now
             };
             _studentContext.Add(enrollment);
             _studentContext.SaveChanges();
         }

         private void AddStudentToEnrollment(string index, string first, string last, DateTime date, int idEnrollment)
         {
             _studentContext.Add(new Student
             {
                 IndexNumber = index,
                 FirstName = first,
                 LastName = last,
                 BirthDate = date,
                 IdEnrollment = idEnrollment
             });
             _studentContext.SaveChanges();

         }

         public bool PromoteStudents(PromoteRequest p)
         {
             var enrollments = GetEnrollmentsToPromote(p);
             try
             {
                 IncrementEnrollments(enrollments);
             }
             catch (Exception e)
             {
                 return false;
             }

             return true;


         }

         private IEnumerable<Enrollment> GetEnrollmentsToPromote(PromoteRequest p)
         {
             var studies = _studentContext.Studies;
             var enrollments = studies.First(s => s.Name.Equals(p.Studies))
                 .Enrollment.ToList();
             return enrollments.Where(e => e.Semester == p.Semester).ToList();
         }

         private void IncrementEnrollments(IEnumerable<Enrollment> enrollments)
         {
             foreach (var e in enrollments)
             {
                 var enrollment = _studentContext.Enrollment.First(s => s.IdEnrollment == e.IdEnrollment);
                 enrollment.Semester++;
                 _studentContext.Update(enrollment);
             }

             _studentContext.SaveChanges();
         }
     }
 }
