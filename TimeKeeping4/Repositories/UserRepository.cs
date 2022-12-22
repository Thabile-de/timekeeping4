
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Internal;
using Microsoft.Graph;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using TimeKeeping4.Repositories;

namespace TimeKeeping4.Model
{
    public class UserRepository : IUserRepository
    {
        private readonly AppConfig _cloudConfig;
        private readonly CognitoUserPool _userPool;
        private readonly IAmazonCognitoIdentityProvider _provider;

        public UserRepository(IConfiguration configuration)
        {
            _cloudConfig = new AppConfig
            {
                AccessKeyId = configuration["Appconfig:AccessKeyId"],
                AppClientId = configuration["Appconfig:AppClientId"],
                AccessSecretKey = configuration["Appconfig:AccessSecretKey"],
                AWSRegion = configuration["Appconfig:AWSRegion"],
                UserPoolId = configuration["Appconfig:UserPoolId"],
            };

            _provider = new AmazonCognitoIdentityProviderClient(
                            _cloudConfig.AccessKeyId,
                            _cloudConfig.AccessSecretKey,
                            RegionEndpoint.GetBySystemName(_cloudConfig.AWSRegion));

            _userPool = new CognitoUserPool(
                _cloudConfig.UserPoolId,
                _cloudConfig.AppClientId,
                _provider);
        }
        public Task<BaseResponse> TryChangeWordAsync(ChangePwdModel model)
        {
            throw new NotImplementedException();
        }
        public Task<InitForgotPwdResponse> TryInitForgotPasswordAsync(InitForgotPasswordModel model)
        {
            throw new NotImplementedException();
        }
        public async Task<AuthResponseModel> TryLoginAsync(UserSignIn model)
        {
            try
            {
                CognitoUser user = new CognitoUser(
                       model.Email,
                        _cloudConfig.AppClientId,
                        _userPool,
                        _provider);

                InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
                {
                    Password = model.Password
                };
                AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest);
                var result = authResponse.AuthenticationResult;

                var authResponseModel = new AuthResponseModel();
                authResponseModel.EmailAddress = user.UserID;
                authResponseModel.UserName = user.Username;
                authResponseModel.Tokens = new TokenModel
                {
                    IdToken = result.IdToken,
                    AccessToken = result.AccessToken,
                    ExpiresIn = result.ExpiresIn,
                    RefreshToken = result.RefreshToken
                };
                authResponseModel.IsSuccess = true;
                return authResponseModel;
            }
            catch (UserNotConfirmedException)
            {
                // Occurs if the User has signed up 
                // but has not confirmed his EmailAddress
                // In this block we try sending 
                // the Confirmation Code again and ask user to confirm
                return new AuthResponseModel
                {
                    IsSuccess = false,
                    Message = "EmailAddress not confirmed."
                };
            }
            catch (UserNotFoundException)
            {
                // Occurs if the provided emailAddress 
                // doesn't exist in the UserPool
                return new AuthResponseModel
                {
                    IsSuccess = false,
                    Message = "EmailAddress not found."
                };
            }
            catch (NotAuthorizedException)
            {
                return new AuthResponseModel
                {
                    IsSuccess = false,
                    Message = "Incorrect username or password",
                };             
            }
        }
        public Task<ResetPasswordResponse> TryResetPasswordWithConfirmationCodeAsync(ResetPasswordModel model)
        {
            throw new NotImplementedException();
        }

        public Task<UserRegistrationResponse> userRegistrationAsync(IUserConfirmRegistrationModel model)
        {
            throw new NotImplementedException();
        }

        public Task<UserRegistrationResponse> UserRegistrationAsync(IUserConfirmRegistrationModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<UserSignUpResponse> ConfirmUserSignUpAsync(UserConfirmSignUp model)
        {
            ConfirmSignUpRequest request = new ConfirmSignUpRequest
            {
                ClientId = _cloudConfig.AppClientId,
                ConfirmationCode = model.ConfirmationCode,
                Username = model.Username
            };

            try
            {
                var response = await _provider.ConfirmSignUpAsync(request);
                return new UserSignUpResponse
                {                    
                    Message = "User Confirmed",
                    IsSuccess = true
                };
            }
            catch (CodeMismatchException)
            {
                return new UserSignUpResponse
                {
                    IsSuccess = false,
                    Message = "Invalid Confirmation Code",                   
                };
            }
        }
        public async Task<UserRegistrationResponse> CreateUserAsync(UserRegistration model)
        {

            // create a RegistrationRequest        
            var registrationRequest = new SignUpRequest
            {
                ClientId = _cloudConfig.AppClientId,
                Password = model.Password,
                Username = model.Username
            };

            // add all the attributes 
            // you want to add to the New User
            registrationRequest.UserAttributes.Add(new AttributeType
            {
                Name = "email",
                Value = model.Email
            });

            try
            {
                // call RegistrationAsync() method
                SignUpResponse? response = await _provider.SignUpAsync(registrationRequest);

                var registrationResponse = new UserRegistrationResponse
                {
                    UserId = response.UserSub,
                    EmailAddress = model.Email, 
                    Message = $"Registration Successful!! Confirmation Code sent to {response.CodeDeliveryDetails.Destination} via {response.CodeDeliveryDetails.DeliveryMedium.Value}",
                    IsSuccess = true
                };
                return registrationResponse;
            }
            catch (UsernameExistsException)
            {
                return new UserRegistrationResponse
                {
                    IsSuccess = false,
                    Message = "EmailAddress Already Exists"
                };
            }
        }
    }
}
