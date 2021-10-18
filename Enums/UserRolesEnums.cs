using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zmgTestBack.Models
{
    public enum RolesEnums
    {
        Editor = 1,
        Writer = 2,
        Viewer = 3

    }

    public static class RolesConstants
    {
        public const string Editor = "1";
        public const string Writer = "2";
        public const string Viewer = "3";
    }
}
