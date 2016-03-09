﻿using NBitcoin.BitcoinCore;
using NBitcoin.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBitcoin
{

	public static class StandardScripts
	{
		static readonly ScriptTemplate[] _StandardTemplates = new ScriptTemplate[] 
		{
			PayToPubkeyHashTemplate.Instance, 
			PayToPubkeyTemplate.Instance,
			PayToScriptHashTemplate.Instance,
			PayToMultiSigTemplate.Instance,
			TxNullDataTemplate.Instance
		};
		public static Script PayToAddress(BitcoinAddress address)
		{
			return PayToPubkeyHash((KeyId)address.Hash);
		}

		private static Script PayToPubkeyHash(KeyId pubkeyHash)
		{
			return PayToPubkeyHashTemplate.Instance.GenerateScriptPubKey(pubkeyHash);
		}

		public static Script PayToPubkey(PubKey pubkey)
		{
			return PayToPubkeyTemplate.Instance.GenerateScriptPubKey(pubkey);
		}

		public static bool IsStandardTransaction(Transaction tx)
		{
			return new StandardTransactionPolicy().Check(tx, null).Length == 0;
		}

		public static bool AreOutputsStandard(Transaction tx)
		{
			return tx.Outputs.All(vout => IsStandardScriptPubKey(vout.ScriptPubKey));
		}

		public static ScriptTemplate GetTemplateFromScriptPubKey(Script script)
		{
			return _StandardTemplates.FirstOrDefault(t => t.CheckScriptPubKey(script));
		}

		public static bool IsStandardScriptPubKey(Script scriptPubKey)
		{
			return _StandardTemplates.Any(template => template.CheckScriptPubKey(scriptPubKey));
		}
		private static bool IsStandardScriptSig(Script scriptSig, Script scriptPubKey)
		{
			var template = GetTemplateFromScriptPubKey(scriptPubKey);
			if(template == null)
				return false;

			return template.CheckScriptSig(scriptSig, scriptPubKey);
		}

		//
		// Check transaction inputs, and make sure any
		// pay-to-script-hash transactions are evaluating IsStandard scripts
		//
		// Why bother? To avoid denial-of-service attacks; an attacker
		// can submit a standard HASH... OP_EQUAL transaction,
		// which will get accepted into blocks. The redemption
		// script can be anything; an attacker could use a very
		// expensive-to-check-upon-redemption script like:
		//   DUP CHECKSIG DROP ... repeated 100 times... OP_1
		//
		public static bool AreInputsStandard(Transaction tx, CoinsView coinsView)
		{
			if(tx.IsCoinBase)
				return true; // Coinbases don't use vin normally

			for(int i = 0 ; i < tx.Inputs.Count ; i++)
			{
				TxOut prev = coinsView.GetOutputFor(tx.Inputs[i]);
				if(prev == null)
					return false;
				if(!IsStandardScriptSig(tx.Inputs[i].ScriptSig, prev.ScriptPubKey))
					return false;
			}

			return true;
		}
	}
}
