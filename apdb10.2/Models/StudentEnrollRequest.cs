using System;

namespace Apdb9.Models
{
    public class StudentEnrollRequest
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Studies { get; set; }


        public bool IsValid()
        {
            if (String.IsNullOrEmpty(IndexNumber) ||
                String.IsNullOrEmpty(FirstName) ||
                String.IsNullOrEmpty(LastName) ||
                BirthDate == null ||
                String.IsNullOrEmpty(Studies))
                return false;
            else return true;
        }
    }
}