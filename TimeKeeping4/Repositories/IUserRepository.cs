using Microsoft.AspNetCore.Identity;
using TimeKeeping4.Model;

namespace TimeKeeping4.Repositories
{
    public interface IUserRepository
    {
        // Registration Flow Start
        Task<UserRegistrationResponse> userRegistrationAsync(IUserConfirmRegistrationModel model);
        Task<UserRegistrationResponse> CreateUserAsync(UserRegistration model);
        //Registration flow ends

        //Chang Password Flow
        Task<BaseResponse> TryChangeWordAsync(ChangePwdModel model);

        //Forget password flow start 
        Task<InitForgotPwdResponse> TryInitForgotPasswordAsync(InitForgotPasswordModel model);
        Task<ResetPasswordResponse> TryResetPasswordWithConfirmationCodeAsync(ResetPasswordModel model);
        //Forgot Password Flow Ends

        //Login Flow Start
        Task<AuthResponseModel> TryLoginAsync(UserSignIn mode);
        //Login Flow Ends

        //confirmSignUp flow start
        Task<UserSignUpResponse> ConfirmUserSignUpAsync(UserConfirmSignUp model);
    }

}
