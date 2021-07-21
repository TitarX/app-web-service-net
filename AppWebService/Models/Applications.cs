using System;

namespace AppWebService.Models
{
    public class Applications
    {
        public virtual int NumberApplication { get; set; }
        public virtual String DateApplication { get; set; }
        public virtual String CompanyName { get; set; }
        public virtual String UserName { get; set; }
        public virtual String Position { get; set; }
        public virtual String Email { get; set; }
    }
}
