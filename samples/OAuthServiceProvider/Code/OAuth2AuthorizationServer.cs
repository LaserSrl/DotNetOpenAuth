﻿namespace OAuthServiceProvider.Code {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Web;
	using DotNetOpenAuth.Messaging;
	using DotNetOpenAuth.Messaging.Bindings;
	using DotNetOpenAuth.OAuth.ChannelElements;
	using DotNetOpenAuth.OAuthWrap;

	internal class OAuth2AuthorizationServer : IAuthorizationServer {
		private static readonly byte[] secret;

		private readonly INonceStore nonceStore = new DatabaseNonceStore();

		static OAuth2AuthorizationServer()
		{
			RandomNumberGenerator crypto = new RNGCryptoServiceProvider();
			secret = new byte[16];
			crypto.GetBytes(secret);
		}

		#region Implementation of IAuthorizationServer

		public byte[] Secret {
			get { return secret; }
		}

		public DotNetOpenAuth.Messaging.Bindings.INonceStore VerificationCodeNonceStore {
			get { return this.nonceStore; }
		}

		public IConsumerDescription GetClient(string clientIdentifier) {
			var consumerRow = Global.DataContext.OAuthConsumers.SingleOrDefault(
				consumerCandidate => consumerCandidate.ConsumerKey == clientIdentifier);
			if (consumerRow == null) {
				throw new ArgumentOutOfRangeException("clientIdentifier");
			}

			return consumerRow;
		}

		#endregion
	}
}