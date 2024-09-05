namespace GradesPrototype.Services
{
    class ClassFullException : Exception
    {
        // Custom data: the name of the class that is full
        // Code that catches this exception can query the public ClassName property to determine which class caused the exception
        private string _className;
        public virtual string ClassName
        {
            get
            {
                return _className;
            }
        }

        // Delegate functionality for the common constructors directly to the Exception class
        public ClassFullException()
        {
        }

        public ClassFullException(string message)
            : base(message)
        {
        }

        public ClassFullException(string message, Exception inner)
            : base(message, inner)
        {
        }

        // Custom constructors that populate the className field.
        // The code that invokes this exception is expected to provide the class name
        public ClassFullException(string message, string cls)
            : base(message)
        {
            _className = cls;
        }

        public ClassFullException(string message, string cls, Exception inner)
            : base(message, inner)
        {
            _className = cls;
        }
    }
}