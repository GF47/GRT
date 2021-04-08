namespace GRT.FSM
{
    public static class FSMUtility
    {
        /// <summary>
        /// 当[ID]为此值时，没有对应的状态
        /// </summary>
        public static int NullStateID = 0;

        #region 现在好想并没有什么卵用
        public static int EntryStateID = -1;
        public static int ExitStateID = -2;
        #endregion

        public static bool Validated(int id)
        {
            return id != NullStateID;
        }
    }
}
