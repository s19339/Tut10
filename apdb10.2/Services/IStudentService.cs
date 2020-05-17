﻿using System.Collections.Generic;
 using apdb10._2.Entities;
 using Apdb9.Models;

namespace Apdb9.Services
{
    public interface IStudentService
    {
        public List<GetStudentsResponse> GetStudents();
        public bool AddStudent(Student student);
        public bool UpdateStudent(Student student);
        public bool DeleteStudent(Student student);
        public bool EnrollStudent(StudentEnrollRequest s);
        public bool PromoteStudents(PromoteRequest p);
    }
}