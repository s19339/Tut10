﻿using System;
 using apdb10._2.Entities;
 using Apdb9.Models;
 using Apdb9.Services;
using Microsoft.AspNetCore.Mvc;

namespace Apdb9.Controllers
{
    [Route("api/students")]
    [ApiController ]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService service)
        {
            _studentService = service;
        }
        

        [HttpGet("get")]
        public IActionResult GetStudents()
        {
            var students = _studentService.GetStudents();
            return Ok(students);
        }

        [HttpPost("add")]
        public IActionResult AddStudent(Student student)
        {
            var isAdded  = _studentService.AddStudent(student);
            if (!isAdded) return BadRequest("incorrect data entered");
            return Ok("added new student");
        }

        [HttpPost("update")]
        public IActionResult UpdateStudent(Student student)
        {
            var isUpdated = _studentService.UpdateStudent(student);
            if (!isUpdated) return BadRequest("incorrect data  entered");
             return Ok("updated the student");
             
        }

        [HttpPost("delete")]
        public IActionResult DeleteStudent(Student student)
        {
            var isDeleted = _studentService.DeleteStudent(student);
            if (isDeleted) return Ok("student deleted");
            else return BadRequest("provided student doesn't exist");
        }

        [HttpPost("enroll")]
        public IActionResult Enroll(StudentEnrollRequest s)
        {
            var isEnrolled = _studentService.EnrollStudent(s);
            if (isEnrolled) return Ok("student enrolled");
            else 
                return BadRequest("an error occured");
        }

        [HttpPost("promote")]
        public IActionResult Promote(PromoteRequest p)
        {
            var isPromoted = _studentService.PromoteStudents(p);
            if (isPromoted) return Ok("student were promoted");
            else return BadRequest("an error occured");
        }
    
        
    }
}