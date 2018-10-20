// Copyright (c) Patrick Evers. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace PMDEvers.Boilerplate.Client
{
	public class BoilerplateException : Exception
	{
		public BoilerplateException(string content, int statusCode)
		{
			Content = content;
			StatusCode = statusCode;
		}

		public string Content { get; }
		public int StatusCode { get; }
	}
}
