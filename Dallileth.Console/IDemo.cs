using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dallileth.Sandbox
{
    public interface IDemo
    {
        Task Run(ScreenRegion region);
    }
}
