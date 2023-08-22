using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions
{
    public class DbQueryException : Exception
    {
        public DbQueryException() : this("Unable to execute db query")
        {
        }

        public DbQueryException(string? message) : base(message)
        {
        }
    }
}
