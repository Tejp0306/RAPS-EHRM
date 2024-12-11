using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Models
{
    public class Result
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
        public int IFormFile { get; set; }
        public string FileName { get; set; }
    }
}
