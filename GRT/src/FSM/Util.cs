namespace GRT.FSM
{
    public static class Util
    {
        /// <summary>
        /// 当 ID 为此值时，没有对应的状态
        /// </summary>
        public const int NullStateID = int.MinValue;

        public const int EntryStateID = int.MinValue;
        public const int ExitStateID = int.MaxValue;

        /// <summary>
        /// 判断是否为合法的ID
        /// </summary>
        public static bool IsValid(int anyID) => anyID != NullStateID;
    }
}