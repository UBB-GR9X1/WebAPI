using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Exceptions;
using ClassLibrary.Model;
using ClassLibrary.Repository;

namespace ClassLibrary.Service
{
    /// <summary>
    /// Service for the Login / Create Account.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ILogInRepository _log_in_repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="_repository">.</param>
        public AuthService(ILogInRepository _repository)
        {
            _log_in_repository = _repository;
        }

        /// <summary>
        /// Validation Numbers, Limits and Digits for Creating an Account.
        /// </summary>
        public enum numbersForValidationsWhenCreatingAnAccount
        {
            /// <summary>
            /// The maximum Limit for the username Length.
            /// </summary>
            LIMIT_FOR_USERNAME_LENGTH = 50,

            /// <summary>
            /// The maximum Limit for the password Length.
            /// </summary>
            LIMIT_FOR_PASSWORD_LENGTH = 255,

            /// <summary>
            /// The maximum Limit for the Mail Length.
            /// </summary>
            LIMIT_FOR_MAIL_LENGTH = 100,

            /// <summary>
            /// The maximum Limit for the name Length.
            /// </summary>
            LIMIT_FOR_NAME_LENGTH = 100,

            /// <summary>
            /// The maximum Limit for the cnp Length.
            /// </summary>
            LIMIT_FOR_CNP_LENGTH = 13,

            /// <summary>
            /// The maximum Limit for the Emergency Contact Length.
            /// </summary>
            LIMIT_FOR_EMERGENCY_CONTACT_LENGTH = 10,

            /// <summary>
            /// The minimum age the user should have for creating an account is 14 years old.
            /// </summary>
            MINIMUM_AGE_FOR_USER = -14,

            /// <summary>
            /// The first digit-index from the cnp.
            /// </summary>
            FIRST_DIGIT_FROM_THE_CNP = 0,

            /// <summary>
            /// Limit for the Birthdate Year Length.
            /// </summary>
            LIMIT_FOR_BIRTH_DATE_YEAR_LENGTH = 4,

            /// <summary>
            /// The age for which the first digit of the cnp changes its values.
            /// </summary>
            AGE_FOR_CHANGING_FIRST_DIGIT_OF_THE_CNP = 1999,

            /// <summary>
            /// Limit for the Birthdate Month Length.
            /// </summary>
            LIMIT_FOR_BIRTH_DATE_MONTH_LENGTH = 1,

            /// <summary>
            /// The second digit-index from the cnp / birthdate.
            /// </summary>
            SECOND_DIGIT_FROM_CNP_OR_BIRTH_DATE = 1,

            /// <summary>
            /// The third digit-index from the cnp / birthdate.
            /// </summary>
            THIRD_DIGIT_FROM_BIRTH_DATE_OR_CNP = 2,

            /// <summary>
            /// The fourth digit-index from the Birthdate.
            /// </summary>
            FOURTH_DIGIT_FROM_BIRTH_DATE = 3,

            /// <summary>
            /// The fourth digit-index from the cnp.
            /// </summary>
            FOURTH_DIGIT_FROM_CNP = 3,

            /// <summary>
            /// The first digit-index from the Birthdate.
            /// </summary>
            FIRST_DIGIT_FROM_BIRTH_DATE = 0,

            /// <summary>
            /// The sixth digit-index from the cnp.
            /// </summary>
            SIXTH_DIGIT_OF_THE_CNP = 5,

            /// <summary>
            /// The fifth digit-index from the cnp.
            /// </summary>
            FIFTH_DIGIT_OF_THE_CNP = 4,

            /// <summary>
            /// The seventh digit-index from the cnp.
            /// </summary>
            SEVENTH_DIGIT_OF_THE_CNP = 6,

        }

        /// <summary>
        /// Gets the user information (given as UserAuthModel).
        /// </summary>
        public UserAuthModel allUserInformation { get; private set; } = UserAuthModel.s_dafault;

        /// <summary>
        /// Loads the user page based on the username.
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <returns>true, no mather what.</returns>
        public async Task<bool> loadUserByUsername(string username)
        {
            this.allUserInformation = await _log_in_repository.getUserByUsername(username).ConfigureAwait(false);
            return true;
        }

        /// <summary>
        /// Checks if the password matches at log in (if the user typed the right password).
        /// </summary>
        /// <param name="user_input_password">The user input password.</param>
        /// <returns>true, if the password matches with the user's one, or false if the one from the input does not match.</returns>
        public async Task<bool> verifyPassword(string user_input_password)
        {
            if (this.allUserInformation == UserAuthModel.s_dafault)
            {
                return false;
            }

            if (!this.allUserInformation.password.Equals(user_input_password))
            {
                return false;
            }

            return await logAction(ActionType.LOGIN);
        }

        /// <summary>
        /// Logs out the user from the application (goes back to Main Window).
        /// </summary>
        /// <returns>the result of the logging out</returns>
        /// <exception cref="AuthenticationException">Checks if the user is logged in, throws an exception if not.</exception>
        public async Task<bool> logOut()
        {
            if (this.allUserInformation == UserAuthModel.s_dafault)
            {
                throw new AuthenticationException("Not logged in");
            }

            bool result_for_logging_out = await logAction(ActionType.LOGOUT);

            if (result_for_logging_out)
            {
                this.allUserInformation = UserAuthModel.s_dafault;
            }

            return result_for_logging_out;
        }

        /// <summary>
        /// Creates an accout, checking for validation errors in the user's inputs.
        /// </summary>
        /// <param name="model_for_creating_user_account">The user's information Model given as UserCreateAccountModel.</param>
        /// <returns>User action: LOGIN if the accout got created, LOGOUT otherwise.</returns>
        /// <exception cref="AuthenticationException">Exceptions if the inputs were not valid
        /// + messages for each validation error.</exception>
        public async Task<bool> createAccount(UserCreateAccountModel model_for_creating_user_account)
        {
            if (string.IsNullOrWhiteSpace(model_for_creating_user_account.username) || model_for_creating_user_account.username.Contains(' '))
            {
                throw new AuthenticationException("Invalid username!\nCan't be null or with space");
            }

            if (model_for_creating_user_account.username.Length > (int)numbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_USERNAME_LENGTH)
            {
                throw new AuthenticationException("Invalid username!\nCan't be more than 50 characters");
            }

            if (string.IsNullOrEmpty(model_for_creating_user_account.password) || model_for_creating_user_account.password.Contains(' '))
            {
                throw new AuthenticationException("Invalid password!\nCan't be null or with space");
            }

            if (model_for_creating_user_account.password.Length > (int)numbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_PASSWORD_LENGTH)
            {
                throw new AuthenticationException("Invalid password!\nCan't be more than 255 characters");
            }

            if (string.IsNullOrEmpty(model_for_creating_user_account.mail) || !model_for_creating_user_account.mail.Contains('@') || !model_for_creating_user_account.mail.Contains('.'))
            {
                throw new AuthenticationException("Invalid mail!\nCan't be null, has to contain '@' and '.'");
            }

            if (model_for_creating_user_account.mail.Length > (int)numbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_MAIL_LENGTH)
            {
                throw new AuthenticationException("Invalid mail!\nCan't be more than 100 characters");
            }

            if (string.IsNullOrEmpty(model_for_creating_user_account.name) || !model_for_creating_user_account.name.Contains(' '))
            {
                throw new AuthenticationException("Invalid name!\nCan't be null, has to contain space");
            }

            if (model_for_creating_user_account.name.Length > (int)numbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_NAME_LENGTH)
            {
                throw new AuthenticationException("Invalid name!\nCan't be more than 100 characters");
            }

            if (model_for_creating_user_account.cnp.Length != (int)numbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_CNP_LENGTH)
            {
                throw new AuthenticationException("Invalid cnp!\nHas to have length 13");
            }

            foreach (char characterFromCNP in model_for_creating_user_account.cnp)
            {
                if (!char.IsDigit(characterFromCNP))
                {
                    throw new AuthenticationException("Invalid cnp!\nOnly numbers allowed");
                }
            }

            if (model_for_creating_user_account.emergencyContact.Length != (int)numbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_EMERGENCY_CONTACT_LENGTH)
            {
                throw new AuthenticationException("Invalid emergency contact!\nIt must have length 10");
            }

            foreach (char oneCharacterFromEmergencyContact in model_for_creating_user_account.emergencyContact)
            {
                if (!char.IsDigit(oneCharacterFromEmergencyContact))
                {
                    throw new AuthenticationException("Invalid emergency contact!\nOnly numbers allowed");
                }
            }

            // check if model is at least 14 years old
            DateOnly todayDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly minValidDate = todayDate.AddYears((int)numbersForValidationsWhenCreatingAnAccount.MINIMUM_AGE_FOR_USER);
            if (model_for_creating_user_account.birthDate > minValidDate)
            {
                throw new AuthenticationException("Invalid Date\nPatient must be at least 14 years old!");
            }

            // check if valid gender
            switch (model_for_creating_user_account.birthDate.Year <= (int)numbersForValidationsWhenCreatingAnAccount.AGE_FOR_CHANGING_FIRST_DIGIT_OF_THE_CNP)
            {
                case true:
                    if (model_for_creating_user_account.cnp[(int)numbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_THE_CNP] != '1' && model_for_creating_user_account.cnp[(int)numbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_THE_CNP] != '2')
                    {
                        throw new AuthenticationException("cnp gender is invalid");
                    }

                    break;
                case false:
                    if (model_for_creating_user_account.cnp[(int)numbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_THE_CNP] != '5' && model_for_creating_user_account.cnp[(int)numbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_THE_CNP] != '6')
                    {
                        throw new AuthenticationException("cnp gender is invalid");
                    }

                    break;
            }

            if (model_for_creating_user_account.birthDate.Year.ToString().Length != (int)numbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_BIRTH_DATE_YEAR_LENGTH)
            {
                throw new AuthenticationException("cnp birth year errorYou may be old, but you surely aren't this old :)!");
            }

            // check if valid match between birth date and cnp birth date
            if (model_for_creating_user_account.birthDate.Year.ToString().Substring((int)numbersForValidationsWhenCreatingAnAccount.THIRD_DIGIT_FROM_BIRTH_DATE_OR_CNP, (int)numbersForValidationsWhenCreatingAnAccount.THIRD_DIGIT_FROM_BIRTH_DATE_OR_CNP)
                != model_for_creating_user_account.cnp.Substring((int)numbersForValidationsWhenCreatingAnAccount.SECOND_DIGIT_FROM_CNP_OR_BIRTH_DATE, (int)numbersForValidationsWhenCreatingAnAccount.THIRD_DIGIT_FROM_BIRTH_DATE_OR_CNP))
            {
                throw new AuthenticationException("Mismatch between Birth year and cnp birth year");
            }

            if (model_for_creating_user_account.birthDate.Month.ToString().Length == (int)numbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_BIRTH_DATE_MONTH_LENGTH)
            {
                if (model_for_creating_user_account.birthDate.Month.ToString()[(int)numbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_BIRTH_DATE]
                    != model_for_creating_user_account.cnp[(int)numbersForValidationsWhenCreatingAnAccount.FIFTH_DIGIT_OF_THE_CNP]
                    || model_for_creating_user_account.cnp[(int)numbersForValidationsWhenCreatingAnAccount.FOURTH_DIGIT_FROM_CNP] != '0')
                {
                    throw new AuthenticationException("Mismatch between Birth Month and cnp birth month");
                }
            }
            else
                if (model_for_creating_user_account.birthDate.Month.ToString() !=
                model_for_creating_user_account.cnp.Substring((int)numbersForValidationsWhenCreatingAnAccount.FOURTH_DIGIT_FROM_BIRTH_DATE, (int)numbersForValidationsWhenCreatingAnAccount.THIRD_DIGIT_FROM_BIRTH_DATE_OR_CNP))
            {
                throw new AuthenticationException("Mismatch between Birth Month and cnp birth month");
            }

            if (model_for_creating_user_account.birthDate.Day.ToString().Length == (int)numbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_BIRTH_DATE_MONTH_LENGTH)
            {
                if (model_for_creating_user_account.birthDate.Day.ToString()[(int)numbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_BIRTH_DATE] != model_for_creating_user_account.cnp[(int)numbersForValidationsWhenCreatingAnAccount.SEVENTH_DIGIT_OF_THE_CNP] || model_for_creating_user_account.cnp[(int)numbersForValidationsWhenCreatingAnAccount.SIXTH_DIGIT_OF_THE_CNP] != '0')
                {
                    throw new AuthenticationException("Mismatch between Birth Day and cnp birth day");
                }
            }
            else
                if (model_for_creating_user_account.birthDate.Day.ToString() != model_for_creating_user_account.cnp.Substring((int)numbersForValidationsWhenCreatingAnAccount.SIXTH_DIGIT_OF_THE_CNP, (int)numbersForValidationsWhenCreatingAnAccount.THIRD_DIGIT_FROM_BIRTH_DATE_OR_CNP))
            {
                throw new AuthenticationException("Mismatch between Birth Day and cnp birth day");
            }

            bool result = await _log_in_repository.createAccount(model_for_creating_user_account);
            if (result)
            {
                if (await loadUserByUsername(model_for_creating_user_account.username))
                {
                    await logAction(ActionType.CREATE_ACCOUNT);
                    return await logAction(ActionType.LOGIN);
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the user's action.
        /// </summary>
        /// <param name="action_type_login_or_logout">The type of the user's action.</param>
        /// <returns>The action setter.</returns>
        public async Task<bool> logAction(ActionType action_type_login_or_logout)
        {
            return await _log_in_repository.authenticationLogService(allUserInformation.userId, action_type_login_or_logout);
        }
    }
}
