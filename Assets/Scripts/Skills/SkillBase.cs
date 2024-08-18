namespace Base.Skills
{
    public class SkillBase
    {
        public string SkillID;
        public string DisplayName;
        public float TotalExp;
        public int Level;

        public virtual void Init()
        {
        }
        public int GetLevelFromExp(float exp)
        {
            return 0;
        }
        public void AddExp(float exp)
        {
            TotalExp += exp;
            Level = GetLevelFromExp(TotalExp);
        }
        public void RemoveExp(float exp)
        {
            TotalExp -= exp;
            Level = GetLevelFromExp(TotalExp);
        }
    }
}
