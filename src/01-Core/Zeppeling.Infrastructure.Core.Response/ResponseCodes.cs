using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Zeppeling.Infrastructure.Core.Response
{
    public static class ResponseCodes
    {
        public const string Success = "RC0000";
        public const string NotNullable = "RC0001";
        public const string Failed = "RC0002";
        public const string NotFound = "RC0003";
        public const string Unauthorized = "RC0004";
        public const string BadRequest = "RC0005";
        public const string UserAlreadyExistButNotConfirmed= "RC10001";
        public const string ConfirmedButDetailsWereNotFilled= "RC10002";
        public const string UserAlreadyExist= "RC10003";
        public const string NeedToConfirmAccount = "RC10004";
        public const string LockedAccount = "RC10005";
        public const string UseRemoved = "RC10006";
        public const string AccountLockedFor30Min = "RC10007";
        public const string SignInFailed = "RC10008";
        public const string SMSCodeNotMatched = "RC10009";
        public const string UserNotFound = "RC10010";
        public const string PasswordNotMatched = "RC10011";
    }
}