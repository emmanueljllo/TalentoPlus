using System;
namespace TalentoPlus.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException(string message) : base(message) { }
    }

namespace TalentoPlus.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message) { }
    }
}