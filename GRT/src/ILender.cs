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

        void Borrow(ILender<T> lender);

        T Return();
    }

    public static class Notary<T> where T : class
    {
        public static void Borrow(IBorrower<T> borrower, ILender<T> lender)
        {
            borrower.Borrow(lender);

            lender.Borrowers.Add(borrower);
        }

        public static T Return(IBorrower<T> borrower, ILender<T> lender, bool removeBorrower = true)
        {
            if (borrower.Lender == lender)
            {
                var wares = borrower.Return();

                if (removeBorrower)
                {
                    lender.Borrowers.Remove(borrower);
                }

                return wares;
            }
            else
            {
                return null;
            }
        }
    }
}