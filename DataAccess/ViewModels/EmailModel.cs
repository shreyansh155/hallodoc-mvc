using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DataAccess.ViewModels
{
    public class EmailModel
    {
        public string From { get; set; }
        public string  To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
