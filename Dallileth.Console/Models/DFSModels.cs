using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dallileth.Sandbox.Models
{
    public enum PathType { Directory,File};
    public struct PathSearch
    {
        public string Path { get; set; }
        public PathType PathType { get; set; }
    }
}
