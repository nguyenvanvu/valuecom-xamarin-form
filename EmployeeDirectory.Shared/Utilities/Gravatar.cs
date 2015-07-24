//
//  Copyright 2012, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using System;
using System.Linq;
using System.Text;

namespace EmployeeDirectory.Utilities
{
	public class Gravatar
	{
		static readonly Func<byte[], byte[]> md5;

		static Gravatar ()
		{
#if SILVERLIGHT
            md5 = MD5Core.GetHash;
#else
			md5 = System.Security.Cryptography.MD5.Create ().ComputeHash;
#endif
		}

		public static Uri GetImageUrl (string email, int size)
		{
			if (string.IsNullOrEmpty (email))
				throw new ArgumentException ("Email must be a valid email address.", "email");

			if (size <= 0)
				throw new ArgumentException ("Size must be greater than 0.", "size");

			var hash = md5 (Encoding.UTF8.GetBytes (email.Trim ()));
			var hashString = string.Join (string.Empty, hash.Select (x => x.ToString ("x2")));
			var uri = string.Format ("http://www.gravatar.com/avatar/{0}.jpg?s={1}&d=mm", hashString, size);
			return new Uri (uri);
		}
	}
}

