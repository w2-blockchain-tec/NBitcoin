using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingBlockchain.Chapters
{
    class Chapter2
    {
        public void Lesson1()
        {
            var blockr = new BlockrTransactionRepository();
            Transaction transaction = blockr.Get("9609ece7fc5b531c52e566d9e72202fe45df368c9b2052ab8d4d82c78520416d");
            Console.WriteLine(transaction.ToString());
        }
    }
}
