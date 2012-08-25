using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyControl
{
    public interface Level
    {
        void run();

        int getScore();
    }
}
