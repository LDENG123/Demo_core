using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_core.Models
{
    public class SqlDb
    {
        public int Id { get; set; }
        public string User_Name { get; set; }
        public string User_Pass { get; set; }
        public SqlDb(int id, string username, string userpass)
        {
            Id = id;
            User_Name = username;
            User_Pass = userpass;
        }
    }
}
