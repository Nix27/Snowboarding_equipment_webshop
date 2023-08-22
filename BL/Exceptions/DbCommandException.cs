using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions
{
    public class DbCommandException : Exception
    {
        public DbCommandException() : this("Unable to execute db command")
        {
        }

        public DbCommandException(string? message) : base(message)
        {
        }
    }
}
