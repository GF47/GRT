﻿namespace GRT.GInventory
{
    public static class Keywords
    {
        public const string ITEM = "item";
        public const string NAME = "name";
        public const string DESCRIPTION = "desc";
        public const string QUANTITY = "quantity";
        public const string COUNT = "count";
        public const string VOLUME = "volume";

        public const string ICON = "icon";

        public const string PROTOTYPE = "prototype";
        public const string POS = "pos";
        public const string ROT = "rot";
        public const string SCALE = "scale";

        public const string DOSE = "dose";

        public const string SPAWN = "spawn";

        public const string ASSET = "asset";
        public const string PROPERTIES = "properties";
        public const string SKILL = "skill";
        public const string TRIGGER = "trigger";
        public const string KEY = "key";
        public const string MOUSE = "mouse";
        public const string TIME = "time";
        public const string CLICK = "click";
        public const string PICK = "pick";
        public const string DROP = "drop";
        public const string THROW = "throw";

        public const string AUTO_SELECT = "auto_select";
        public const string AUTO_SELECT_2 = "auto-select";
        public const string AUTO_SELECT_3 = "autoSelect";
        public static bool IsAutoSelect(string str) => str == AUTO_SELECT || str == AUTO_SELECT_2 || str == AUTO_SELECT_3;
    }
}