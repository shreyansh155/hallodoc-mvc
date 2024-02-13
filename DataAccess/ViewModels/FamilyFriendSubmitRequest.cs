﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;



namespace DataAccess.ViewModels
{
public class FamilyFriendSubmitRequest
    {
        public string  FirstName { get; set;}
        public string LastName { get; set;}
        public string Phone { get; set;}
        public string Email { get; set;}
        public string RelationWithPatient { get; set;}
        public string PatientFirstName { get; set;}
        public string PatientLastName { get; set;}
        public string Symptoms { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PatientEmail { get; set;}
        public string PatientPhone { get; set;}
        public string Street { get; set;}
        public string City { get; set;}
        public string State { get; set;}
        public string ZipCode { get; set;}
        public string Room { get; set;}
        public IFormFile FileUpload { get; set; }

    }
}
