﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email{ get; set; }
        public string Senha { get; set; }
    }
}
