using GRT.Data;

namespace GRT.Editor.GXSerialization
{
    [GXNode(Name = "actor")]
    public class Actor
    {
        [GXAttribute(Name = "name", Default = "GF47")]
        public string Name {  get; set; }

        [GXAttribute(Name = "age", Default = "3")]
        public int Age {  get; set; }

        [GXAttribute(Name = "sex", Default = "false")]
        public bool Sex {  get; set; }

        [GXNode(Name = "mood", Default = "happy")]
        public string Mood { get; set; }

        [GXNode(Name = "magic", Default = "100.00", Decimal = 1)]
        public float Magic { get; set; }

        [GXNode(Name = "spouse")]
        public Actor Spouse { get; set; }

        [GXNode(Name = "friend")]
        [GXArray(Container = "friends")]
        public Actor[] Friends { get; set; }
    }
}