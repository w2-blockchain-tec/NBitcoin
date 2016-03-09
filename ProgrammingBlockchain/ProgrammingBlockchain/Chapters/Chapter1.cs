using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingBlockchain.Chapters
{
    class Chapter1
    {
        public void Lesson1()
        {
            Key key = new Key(); //generates a new private key.
            PubKey pubKey = key.PubKey; //gets the matching public key.
            Console.WriteLine("Public Key: {0}", pubKey);
            KeyId hash = pubKey.Hash; //gets a hash of the public key.
            Console.WriteLine("Hashed public key: {0}", hash);
            BitcoinAddress address = pubKey.GetAddress(Network.Main); //retrieves the bitcoin address.
            Console.WriteLine("Address: {0}", address);
            Script scriptPubKeyFromAddress = address.ScriptPubKey;
            Console.WriteLine("ScriptPubKey from address: {0}", scriptPubKeyFromAddress);
            Script scriptPubKeyFromHash = hash.ScriptPubKey;
            Console.WriteLine("ScriptPubKey from hash: {0}", scriptPubKeyFromHash);
        }

        /*
        public void Lesson2()
        {
            Script scriptPubKey = new Script("OP_DUP OP_HASH ?XXXX? OP_EQUALVERIFY OP_CHECKSIG");
            BitcoinAddress address = scriptPubKey.GetDestinationAddress(Network.Main);
            Console.WriteLine("Bitcoin Address: {0}", address);
        }

        public void Lesson3()
        {
            Script scriptPubKey = new Script("OP_DUP OP_HASH160 ?XXXX? OP_EQUALVERIFY OP_CHECKSIG");
            KeyId hash = (KeyId)scriptPubKey.GetDestination();
            Console.WriteLine("Public Key Hash: {0}", hash);
            BitcoinAddress address = new BitcoinAddress(hash, Network.Main);
            Console.WriteLine("Bitcoin Address: {0}", address);
        }
         */

        public void Lesson4()
        {
            Key key = new Key();
            BitcoinSecret secret = key.GetBitcoinSecret(Network.Main);
            Console.WriteLine("Bitcoin Secret: {0}", secret);
        }

        public void GetSendingAddress() //this is just to try; it's not lesson included
        {
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
        }

        public void GetSendingAddress1()
        {
            BitcoinSecret paymentSecret = new
            BitcoinSecret("L2D5NDkwbHHeKyBT5STHNmyhnTur39VhfS8CJNpZwHYxZteQLnSx");

            PubKey pubKey = paymentSecret.PubKey; //gets the matching public key.
            Console.WriteLine("Public Key: {0}", pubKey);
            KeyId hash = pubKey.Hash; //gets a hash of the public key.
            Console.WriteLine("Hashed public key: {0}", hash);
            BitcoinAddress address = pubKey.GetAddress(Network.Main); //retrieves the bitcoin address.
            Console.WriteLine("Address: {0}", address);
            Script scriptPubKeyFromAddress = address.ScriptPubKey;
            Console.WriteLine("ScriptPubKey from address: {0}", scriptPubKeyFromAddress);
            Script scriptPubKeyFromHash = hash.ScriptPubKey;
            Console.WriteLine("ScriptPubKey from hash: {0}", scriptPubKeyFromHash);
        }
    }
}



