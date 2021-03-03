using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSGateway.Models
{
    public class UserLevel
    {
        public int Level { get; set; }
        public string Desc { get; set; }
    }

    public class DropdownUserLevel
    {
        public List<UserLevel> dpUserlevel { get; set; }
        public DropdownUserLevel()
        {
            dpUserlevel = new List<UserLevel>();
            int[] levels = { 1, 2, 3, 4 };
            string[] desc = { "Supervisor", "Creator", "Editor", "Viewer" };
            for (int i = 0; i < 4; i++)
            {
                dpUserlevel.Add(new UserLevel { Level = levels[i], Desc = desc[i] });
            }
        }      
    }
}
