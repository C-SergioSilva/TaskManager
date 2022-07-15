using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerAPI.Dtos 
{ 
    public class ErrorsDto
    {
        public int Status { get; set; }
        public string Error { get; set; }
    }
}
