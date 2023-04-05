using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Character : BaseModel
{
    public Passive passive;
    public UltimateSkill ultimateSkill;


    public class Skill : BaseModel
    {
        public new virtual void Init()
        {
            base.Init();
        }
    }

    public class Passive : Skill
    {
        public string name;
        public string explain;
    }

    public class UltimateSkill : Skill
    {
        public string name;
        public string explain;
    }

}
