﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mysqlapi.Models
{
    public class StudentDetailDto
    {
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "varchar(11)")]
        public string Phone_Number { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string Birth_Date { get; set; }
    }
}
