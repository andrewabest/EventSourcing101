using System;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    internal class SystemClock : IClock
    {
        public DateTimeOffset UtcNow
        {
            get { return DateTimeOffset.UtcNow; }
        }
    }
}
