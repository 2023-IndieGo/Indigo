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
            //baseModel.Init()
            base.Init();
        }
    }

    public class Passive : Skill
    {
        public string name;
        public string explain;
        public override void Init()
        {
            //Skill.Init()
            base.Init();
        }
    }

    public class UltimateSkill : Skill
    {
        public string name;
        public string explain;
        public override void Init()
        {
            //Skill.Init()
            base.Init();
        }
    }

}
