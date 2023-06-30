using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Services.Books
{
    public interface IBooksServices : IDisposable
    {
    }

    public class BooksServices : BaseServices, IBooksServices
    {
        public BooksServices(string connetionString) : base(connetionString)
        {

        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
