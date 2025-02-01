using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Exceptions
{
    public class UnexpectedError : Exception
    {
        public UnexpectedError(string message) : base(message) { }
    }

    public class EntityNotFoundError : Exception
    {
        public EntityNotFoundError(string message) : base(message) { }
    }

    public class UserAuthorizationException : Exception
    {
        public UserAuthorizationException(string message) : base(message) { }
    }

    public class UserForbiddenException : Exception
    {
        public UserForbiddenException(string message) : base(message) { }
    }

    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string message) : base(message) { }
    }

    public class NoContentException : Exception
    {
        public NoContentException(string message) : base(message) { }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
