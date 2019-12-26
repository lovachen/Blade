using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration.Create
{
    public interface IDownstreamKeyCreator
    {
        string Create(DownstreamProvider downstream);
    }
}
