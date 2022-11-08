using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.GlobalErrorHandling
{
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
    public interface IHandler<T> where T : class
    {
        IHandler<T> SetNext(IHandler<T> next);
        void Handle(T request);
    }

    //public class OnlyAdultValidationHandler : Handler<User>, IHandler<User>
    //{
    //    public override void Handle(User user)
    //    {
    //        if (user.BirthDate.Year > DateTime.Now.Year - 18)
    //        {
    //            throw new Exception("Age under 18 is not allowed.");
    //        }
    //        base.Handle(user);
    //    }
    //}
}
