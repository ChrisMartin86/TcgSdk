using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcgSdk.Common
{
    public interface ITcgSdkResponse<T>
    {
        T[] Cards { get; }

        T[] Sets { get; }
    }
}
