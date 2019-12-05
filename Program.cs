using System;

namespace DnetChainResp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Chain of Responsiblity Pattern");

            User u1 = new User
            {
                Age = 21,
                Name = "John Doe",
                SSN = "123456789"
            };

            User u2 = new User
            {
                Age = 18,
                Name = "Jane Smith",
                SSN = "12345"
            };

            var handler = new SSNHandler();
            handler.SetNext(new AgeHandler());

            handler.Handle(u1); // Passes
            handler.Handle(u2); // Fails
        }
    }

    public interface IHandler<T> where T : class
    {
        public IHandler<T> SetNext(IHandler<T> next);
        public void Handle(T request);
    }

    public abstract class Handler<T> : IHandler<T> where T : class
    {
        private IHandler<T> Next { get; set; }

        public virtual void Handle(T request)
        {
            Next?.Handle(request);
        }

        public IHandler<T> SetNext(IHandler<T> next)
        {
            Next = next;
            return Next;
        }
    }

    public class User
    {
        public string Name { get; set; }
        public string SSN { get; set; }
        public int Age { get; set; }
    }

    public class SSNHandler : Handler<User>
    {
        public override void Handle(User request)
        {
            if (request.SSN.Length != 9)
            {
                throw new Exception("Failed SSN Handler");
            }
            base.Handle(request);
        }
    }

    public class AgeHandler : Handler<User>
    {
        public override void Handle(User request)
        {
            if (request.Age <= 18)
            {
                throw new Exception("Failed Aged Handler");
            }
            base.Handle(request);
        }
    }
}
