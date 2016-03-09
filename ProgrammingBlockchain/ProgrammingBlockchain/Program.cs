using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingBlockchain.Chapters;

namespace ProgrammingBlockchain
{
    class Program
    {
        static void Main(string[] args)
        {
            //Select the chapter here.

            //*
            //BitcoinSecret paymentSecret = new
            //BitcoinSecret("L2D5NDkwbHHeKyBT5STHNmyhnTur39VhfS8CJNpZwHYxZteQLnSx");

            BitcoinSecret bookKey = new
            BitcoinSecret("5JwhZTk2JC2zzhw6pYrhcEnEYKQDbLv3A7NfdayAJ4wppzwRDJW");

            var chapter = new Chapter14();
            chapter.GetAssetId();
            //chapter.OpenAssetIssuanceCoin(bookKey);
            //chapter.Lesson1(paymentSecret);
            //chapter.Proof_of_ownership(paymentSecret);
            //this will hold the window open for you to read the output.
            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();
            // */

            /*
            BitcoinSecret paymentSecret = new
            BitcoinSecret("L2D5NDkwbHHeKyBT5STHNmyhnTur39VhfS8CJNpZwHYxZteQLnSx");

            var chapter = new Chapter1();           

            //call the lesson here.
            chapter.GetSendingAddress1();
            Console.WriteLine("\n\n\nPress enter to continue.");
            Console.ReadLine();

            /*
            //this will hold the window open for you to read the output.
            Console.WriteLine("\n\n\nPress enter to continue.");
            Console.ReadLine();
            chapter.Lesson2();
            //this will hold the window open for you to read the output.
            Console.WriteLine("\n\n\nPress enter to continue.");
            Console.ReadLine();
            chapter.Lesson3();
            //this will hold the window open for you to read the output.
            Console.WriteLine("\n\n\nPress enter to continue.");
            Console.ReadLine();
             */

            /*
            BitcoinSecret paymentSecret = new
            BitcoinSecret("L2D5NDkwbHHeKyBT5STHNmyhnTur39VhfS8CJNpZwHYxZteQLnSx");

            PubKey pubKey = paymentSecret.PubKey; //gets the matching public key.
            Console.WriteLine("Public Key: {0}", pubKey);
            Console.WriteLine("\n\n\nPress enter to continue.");
            Console.ReadLine();
            KeyId hash = pubKey.Hash; //gets a hash of the public key.
            Console.WriteLine("Hashed public key: {0}", hash);
            Console.WriteLine("\n\n\nPress enter to continue.");
            Console.ReadLine();
            BitcoinAddress address = pubKey.GetAddress(Network.Main); //retrieves the bitcoin address.
            Console.WriteLine("Address: {0}", address);
            Console.WriteLine("\n\n\nPress enter to continue.");
            Console.ReadLine();
            */

            /*
            chapter.GetSendingAddress();
            Console.WriteLine("\n\n\nPress enter to continue.");
            Console.ReadLine();
             */
        }
    }
}
