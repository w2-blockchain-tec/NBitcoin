using NBitcoin;
using NBitcoin.OpenAsset;
using NBitcoin.Crypto;
using NBitcoin.Protocol;
using NBitcoin.DataEncoders;
using NBitcoin.Stealth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingBlockchain.Chapters
{
    class Chapter14
    {
        public void Lesson1(BitcoinSecret paymentSecret)
        {
            //start by looking at the transaction that contains the TxOut that you want to spend
            var blockr = new BlockrTransactionRepository();
            Transaction fundingTransaction = blockr.Get("9609ece7fc5b531c52e566d9e72202fe45df368c9b2052ab8d4d82c78520416d");

            //For the payment you will need to reference this output in the transaction. You create a transaction as follows:
            Transaction payment = new Transaction();
            payment.Inputs.Add(new TxIn()
            {
                PrevOut = new OutPoint(fundingTransaction.GetHash(), 0)
            });

            //The book’s donation address is: 1KF8kUVHK42XzgcmJF4Lxz4wcL5WDL97PB
            //BitcoinSecret paymentSecret = new
            //BitcoinSecret("L2D5NDkwbHHeKyBT5STHNmyhnTur39VhfS8CJNpZwHYxZteQLnSx");
            var paymentAddress = new BitcoinAddress("12itMJwyRNnCfqioBuKjd4yZRYbzHj9ZnN");
            var programmingBlockchain = new BitcoinAddress("1KF8kUVHK42XzgcmJF4Lxz4wcL5WDL97PB");
            payment.Outputs.Add(new TxOut()
            {
                Value = Money.Coins(0.004m),
                ScriptPubKey = programmingBlockchain.ScriptPubKey
                });
                payment.Outputs.Add(new TxOut()
            {
            //change, considering the present amount balance of 0.011 (-0.004)
            Value = Money.Coins(0.0069m),
            ScriptPubKey = paymentAddress.ScriptPubKey
            });
            //Now add your feedback! This must be less than 40 bytes, or it will crash the application.
            //Feedback !
            var message = "Thanks a lot! :-)";
            var bytes = Encoding.UTF8.GetBytes(message);
            payment.Outputs.Add(new TxOut()
            {
            Value = Money.Zero,
            ScriptPubKey = TxNullDataTemplate.Instance.GenerateScriptPubKey(bytes)
            });
            Console.WriteLine(payment);

            //First insert the scriptPubKey in the scriptSig.
            //Since the scriptPubKey is nothing but paymentAddress.ScriptPubKey this is simple.
            //Then you need to give your private key for signing.
            payment.Inputs[0].ScriptSig = paymentAddress.ScriptPubKey;
            //also OK :
            //payment.Inputs[0].ScriptSig = fundingTransaction.Outputs[1].ScriptPubKey;
            payment.Sign(paymentSecret, false);

            //Congratulations, you have signed your first transaction! Your transaction is ready to roll!
            //All that is left is to propagate it to the network so the miners can see it.
            //Be sure to have Bitcoin Core running (for local connection) and then: using (var node = Node.ConnectToLocal(Network.Main))
            //For remote node connection: Node.Connect(Network.Main,"ip:port")
            using (var node = Node.Connect(Network.Main, "74.196.43.80:8333")) //Connect to the remote node
            {
                node.VersionHandshake(); //Say hello
                //Advertize your transaction (send just the hash)
                node.SendMessage(new InvPayload(InventoryType.MSG_TX, payment.GetHash()));
                //Send it
                node.SendMessage(new TxPayload(payment));
                Thread.Sleep(500); //Wait a bit
            }
            //The using code block will take care of closing the connection to the node. That's it!
        }

        public void OpenAssetIssuanceCoin(BitcoinSecret bookKey)
        {
            var coin = new Coin(fromTxHash: new
                uint256("9609ece7fc5b531c52e566d9e72202fe45df368c9b2052ab8d4d82c78520416d"),
                fromOutputIndex: 1, amount: Money.Satoshis(280000),
                scriptPubKey: new
                Script(Encoders.Hex.DecodeData("76a914980a4889318253f30f6c0d98a53622d792c3b21288ac")));
            var issuance = new IssuanceCoin(coin);
            var nico = BitcoinAddress.Create("1Erv2f8fJZkkAUkYf3xoTFaebitoBUP4oA");
            //var bookKey = new BitcoinSecret("5JwhZTk2JC2zzhw6pYrhcEnEYKQDbLv3A7NfdayAJ4wppzwRDJW");

            /*
            var builder = new TransactionBuilder();
            var tx = builder
                .AddKeys(bookKey)
                .AddCoins(issuance)
                .IssueAsset(nico, new AssetMoney(issuance.AssetId, 10))
                .SendFees(Money.Coins(0.0001m))
                .SetChange(bookKey.GetAddress())
                .BuildTransaction(true);

            Console.WriteLine(tx);

            //The transaction is ready to be sent on the network:
            //using (var node = Node.ConnectToLocal(Network.Main)) //Connect to the node
            using (var node = Node.Connect(Network.Main, "74.196.43.80:8333")) //Connect to the node
            {
                node.VersionHandshake(); //Say hello
                //Advertize your transaction (send just the hash)
                node.SendMessage(new InvPayload(InventoryType.MSG_TX, tx.GetHash()));
                //Send it
                node.SendMessage(new TxPayload(tx));
                Thread.Sleep(500); //Wait a bit
            }
            */
        }

        public void Transfer_an_Asset()
        {
            var coin = new Coin(fromTxHash: new
                uint256("fa6db7a2e478f3a8a0d1a77456ca5c9fa593e49fd0cf65c7e349e5a4cbe58842"),
                fromOutputIndex: 0,
                amount: Money.Satoshis(2000000),
                scriptPubKey: new Script(Encoders.Hex.DecodeData("76a914356facdac5f5bcae995d13e667bb5864fd1e7d5988ac")));
            
            BitcoinAssetId assetId = new BitcoinAssetId("AVAVfLSb1KZf9tJzrUVpktjxKUXGxUTD4e");
            ColoredCoin colored = coin.ToColoredCoin(assetId, 10);

            var book = BitcoinAddress.Create("1KF8kUVHK42XzgcmJF4Lxz4wcL5WDL97PB");
            var nicoSecret = new BitcoinSecret("??????????");
            var nico = nicoSecret.GetAddress(); //15sYbVpRh6dyWycZMwPdxJWD4xbfxReeHe
            var forFees = new Coin(fromTxHash: new
                uint256("7f296e96ec3525511b836ace0377a9fbb723a47bdfb07c6bc3a6f2a0c23eba26"),
                fromOutputIndex: 0,
                amount: Money.Satoshis(4425000),
                scriptPubKey: new Script(Encoders.Hex.DecodeData("76a914356facdac5f5bcae995d13e667bb5864fd1e7d5988ac")));
            
            /*
            TransactionBuilder builder = new TransactionBuilder();
            var tx = 
                builder
                .AddKeys(nicoSecret)
                .AddCoins(colored, forFees)
                .SendAsset(book, new AssetMoney(assetId, 10))
                .SetChange(nico)
                .SendFees(Money.Coins(0.0001m))
                .BuildTransaction(true);
            
            Console.WriteLine(tx);
            */
        }

        public void GetGenesis()
        {
            Console.WriteLine(Network.Main.GetGenesis().Transactions[0].ToString());
        }

        public void GetAssetId()
        {
            var book = BitcoinAddress.Create("1KF8kUVHK42XzgcmJF4Lxz4wcL5WDL97PB");
            var assetId = new AssetId(book).GetWif(Network.Main);
            Console.WriteLine(assetId);
        }

        public void Proof_of_ownership(BitcoinSecret paymentSecret)
        {
            var address = new BitcoinAddress("1KF8kUVHK42XzgcmJF4Lxz4wcL5WDL97PB");
            var msg = "Nicolas Dorier Book Funding Address";
            var sig = "H1jiXPzun3rXi0N9v9R5fAWrfEae9WPmlL5DJBj1eTStSvpKdRR8Io6/uT9tGH/3OnzG6ym5yytuWoA9ahkC3dQ=";

            Console.WriteLine(address.VerifyMessage(msg, sig));
            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();

            msg = "Nicolas, thanks a lot! :-)";
            sig = paymentSecret.PrivateKey.SignMessage(msg);
            Console.WriteLine(paymentSecret.GetAddress().VerifyMessage(msg, sig));
        }

        public void Entropy()
        {
            var result = "hello";
            var derived = SCrypt.BitcoinComputeDerivedKey(result, new byte[] { 1, 2, 3 });
            Console.WriteLine(derived);
       
            //RandomUtils.AddEntropy(result);
            //RandomUtils.AddEntropy(new byte[] { 1, 2, 3 });
            //var nsaProofKey = new Key();
        }

        public void Encryption()
        {
            var key = new Key();
            BitcoinSecret wif = key.GetBitcoinSecret(Network.Main);
            Console.WriteLine(wif);
            BitcoinEncryptedSecret encrypted = wif.Encrypt("Baba Nam Kevalam");
            Console.WriteLine(encrypted);
            wif = encrypted.GetSecret("Baba Nam Kevalam");
            Console.WriteLine(wif);
        }

        public void PassphraseCode()
        {
            BitcoinPassphraseCode passphraseCode = new BitcoinPassphraseCode("Baba Krpahi Kevalam", Network.Main, null);
            EncryptedKeyResult encryptedKey1 = passphraseCode.GenerateEncryptedSecret();
            Console.WriteLine(encryptedKey1.GeneratedAddress);
            Console.WriteLine(encryptedKey1.EncryptedKey);
            Console.WriteLine(encryptedKey1.ConfirmationCode);

            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();

            var generatedAddress = encryptedKey1.GeneratedAddress;
            var encryptedKey = encryptedKey1.EncryptedKey;
            var confirmationCode = encryptedKey1.ConfirmationCode;

            Console.WriteLine(confirmationCode.Check("Baba Krpahi Kevalam", generatedAddress));
            BitcoinSecret privateKey = encryptedKey.GetSecret("Baba Krpahi Kevalam");
            Console.WriteLine(privateKey.GetAddress() == generatedAddress);
            Console.WriteLine(privateKey);
        }

        public void multi_sig_coin()
        {
            var bob = new Key();
            var alice = new Key();
            var bobAlice = PayToMultiSigTemplate
            .Instance
            .GenerateScriptPubKey(2, bob.PubKey, alice.PubKey);
            Transaction init = new Transaction();
            
            //var aliceCoin = new Coin();
            //var bobCoin = new Coin();
            //var bobAliceCoin = new Coin();

            init.Outputs.Add(new TxOut(Money.Coins(1.0m), alice.PubKey));
            init.Outputs.Add(new TxOut(Money.Coins(1.0m), bob.PubKey.Hash));
            init.Outputs.Add(new TxOut(Money.Coins(1.0m), bobAlice));

            var satoshi = new Key();
            Coin[] coins = init.Outputs.AsCoins().ToArray();

            /*
            var builder = new TransactionBuilder();
            Transaction tx = builder
            .AddCoins(bobCoin)
            .AddKeys(bob)
            .Send(satoshi, Money.Coins(0.2m))
            .SetChange(bob)
            .Then()
            .AddCoins(aliceCoin)
            .AddKeys(alice)
            .Send(satoshi, Money.Coins(0.3m))
            .SetChange(alice)
            .Then()
            .AddCoins(bobAliceCoin)
            .AddKeys(bob, alice)
            .Send(satoshi, Money.Coins(0.5m))
            .SetChange(bobAlice)
            .SendFees(Money.Coins(0.0001m))
            .BuildTransaction(sign: true);

            Console.WriteLine(builder.Verify(tx));
             */
        }

    }
}
