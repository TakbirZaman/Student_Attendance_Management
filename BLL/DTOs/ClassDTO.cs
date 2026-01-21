
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL.DTOs
{
    public class ClassDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string TeacherName { get; set; }


    }
}
