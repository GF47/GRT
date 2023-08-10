using System.Collections.Generic;

namespace GRT
{
    public interface ILender<T>
    {
        T Wares { get; }

        ICollection<IBorrower<T>> Borrowers { get; }

        void Dun();
    }

    public interface IBorrower<T>
    {
        ILender<T> Lender { get; }

        bool Borrow(ILender<T> lender);

        void Repay();
    }

    public static class Notary<T> where T : class
    {
        public static void Borrow(IBorrower<T> borrower, ILender<T> lender)
        {
            if (borrower.Borrow(lender))
            {
                lender.Borrowers.Add(borrower);
            }
        }

        public static void Repay(IBorrower<T> borrower, ILender<T> lender, bool removeBorrower = true)
        {
            if (borrower.Lender == lender)
            {
                borrower.Repay();

                if (removeBorrower)
                {
                    lender.Borrowers.Remove(borrower);
                }
            }
        }
    }
}