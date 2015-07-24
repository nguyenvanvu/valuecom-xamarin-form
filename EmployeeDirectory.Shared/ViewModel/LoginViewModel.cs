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
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.ViewModels
{
	public class LoginViewModel : ViewModelBase
	{
		IDirectoryService service;
		static readonly TimeSpan ForceLoginTimespan = TimeSpan.FromMinutes (5);

		public string Username { get; set; }

		public string Password { get; set; }

		public LoginViewModel (IDirectoryService service)
		{
			this.service = service;

			Username = string.Empty;
			Password = string.Empty;
		}

		public Task LoginAsync (CancellationToken cancellationToken)
		{
			IsBusy = true;
			return service
				.LoginAsync (Username, Password, cancellationToken)
				.ContinueWith (t => {
				IsBusy = false;
				if (t.IsFaulted)
					throw new AggregateException (t.Exception);
			}, cancellationToken, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext ());
		}

		public static bool ShouldShowLogin (DateTime? lastUseTime)
		{
			if (!lastUseTime.HasValue)
				return true;

			return (DateTime.UtcNow - lastUseTime) > ForceLoginTimespan;
		}
	}
}

