﻿using System;

namespace CMS_BackEnd.Model
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string ImgSrc { get; set; }
    }
}
