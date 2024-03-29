﻿//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using TailwindTraders.Mobile.Features.Common;
//using TailwindTraders.Mobile.Features.Settings;
//using Xamarin.Forms;

//namespace TailwindTraders.Mobile.Features.LogIn
//{
//    public class AuthenticationService : IAuthenticationService
//    {
//        private readonly IRestPoolService restPoolService;

//        private string authenticatedUser;

//        public AuthenticationService()
//        {
//            restPoolService = DependencyService.Get<IRestPoolService>();
//        }

//        public string AuthorizationHeader => $"Bearer {DefaultSettings.AccessToken}";

//        public bool IsAnyOneLoggedIn => authenticatedUser != null;

//        public Task<string> GetUserName()
//        {
//            throw new NotImplementedException();
//        }

//        public async Task LogInAsync(string email, string password)
//        {
//            DefaultSettings.AccessToken = await GetTokenAsync(email, password);

//            var profiles = await restPoolService.ProfilesAPI.GetAsync(AuthorizationHeader);

//            if (!profiles.Any())
//            {
//                throw new InvalidOperationException("No profile could be retrieved.");
//            }

//            authenticatedUser = profiles.First().Email;
//        }

//        public Task RefreshSessionAsync()
//        {
//            throw new NotImplementedException();
//        }

//        private async Task<string> GetTokenAsync(string email, string password)
//        {
//            var request = new TokenRequestDTO()
//            {
//                Username = email,
//                Password = password,
//                GrantType = "password",
//            };

//            var response = await restPoolService.LoginAPI.LoginAsync(request);

//            return response.AccessToken;
//        }
//    }
//}
