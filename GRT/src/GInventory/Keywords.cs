namespace GRT.GInventory
{
    public static class Keywords
    {
        public const string ITEM = "item";
        public const string NAME = "name";
        public const string DESCRIPTION = "desc";
        public const string ICON = "icon";
        public const string QUANTITY = "quantity";
        public const string COUNT = "count";
        public const string VOLUME = "volume";

        public const string AUTO_SPAWN = "auto_spawn";
        public const string AUTO_SPAWN2 = "auto-spawn";
        public const string AUTO_SPAWN3 = "autoSpawn";
        public static bool IsAutoSpawn(string key) => key == AUTO_SPAWN || key == AUTO_SPAWN2 || key == AUTO_SPAWN3;

        public const string MODEL = "model";
        public const string ASSET = "asset";
        public const string PROPERTIES = "properties";
        public const string POS = "pos";
        public const string ROT = "rot";
        public const string SCALE = "scale";
        public const string SKILL = "skill";
        public const string TRIGGER = "trigger";
        public const string KEY = "key";
        public const string MOUSE = "mouse";
        public const string TIME = "time";
        public const string CLICK = "click";
        public const string PICK = "pick";

        public const string PICK_QUANTITY = "pick_quantity";
        public const string PICK_QUANTITY2 = "pick-quantity";
        public const string PICK_QUANTITY3 = "pickQuantity";
        public static bool IsPickQuantity(string key) => key == PICK_QUANTITY || key == PICK_QUANTITY2 || key == PICK_QUANTITY3;
    }
}