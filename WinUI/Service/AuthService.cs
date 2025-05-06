using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Exceptions;
using WinUI.Model;
using WinUI.Repository;

namespace WinUI.Service
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
        public enum NumbersForValidationsWhenCreatingAnAccount
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
        public UserAuthModel all_user_information { get; private set; } = UserAuthModel.s_dafault;

        /// <summary>
        /// Loads the user page based on the username.
        /// </summary>
        /// <param name="_username">The user's username</param>
        /// <returns>true, no mather what.</returns>
        public async Task<bool> loadUserByUsername(string _username)
        {
            this.all_user_information = await _log_in_repository.getUserByUsername(_username).ConfigureAwait(false);
            return true;
        }

        /// <summary>
        /// Checks if the password matches at log in (if the user typed the right password).
        /// </summary>
        /// <param name="_user_input_password">The user input password.</param>
        /// <returns>true, if the password matches with the user's one, or false if the one from the input does not match.</returns>
        public async Task<bool> verifyPassword(string _user_input_password)
        {
            if (this.all_user_information == UserAuthModel.s_dafault)
            {
                return false;
            }

            if (!this.all_user_information.password.Equals(_user_input_password))
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
            if (this.all_user_information == UserAuthModel.s_dafault)
            {
                throw new AuthenticationException("Not logged in");
            }

            bool _result_for_logging_out = await logAction(ActionType.LOGOUT);

            if (_result_for_logging_out)
            {
                this.all_user_information = UserAuthModel.s_dafault;
            }

            return _result_for_logging_out;
        }

        /// <summary>
        /// Creates an accout, checking for validation errors in the user's inputs.
        /// </summary>
        /// <param name="_model_for_creating_user_account">The user's information Model given as UserCreateAccountModel.</param>
        /// <returns>User action: LOGIN if the accout got created, LOGOUT otherwise.</returns>
        /// <exception cref="AuthenticationException">Exceptions if the inputs were not valid
        /// + messages for each validation error.</exception>
        public async Task<bool> createAccount(UserCreateAccountModel _model_for_creating_user_account)
        {
            if (string.IsNullOrWhiteSpace(_model_for_creating_user_account.username) || _model_for_creating_user_account.username.Contains(' '))
            {
                throw new AuthenticationException("Invalid username!\nCan't be null or with space");
            }

            if (_model_for_creating_user_account.username.Length > (int)NumbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_USERNAME_LENGTH)
            {
                throw new AuthenticationException("Invalid username!\nCan't be more than 50 characters");
            }

            if (string.IsNullOrEmpty(_model_for_creating_user_account.password) || _model_for_creating_user_account.password.Contains(' '))
            {
                throw new AuthenticationException("Invalid password!\nCan't be null or with space");
            }

            if (_model_for_creating_user_account.password.Length > (int)NumbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_PASSWORD_LENGTH)
            {
                throw new AuthenticationException("Invalid password!\nCan't be more than 255 characters");
            }

            if (string.IsNullOrEmpty(_model_for_creating_user_account.mail) || !_model_for_creating_user_account.mail.Contains('@') || !_model_for_creating_user_account.mail.Contains('.'))
            {
                throw new AuthenticationException("Invalid mail!\nCan't be null, has to contain '@' and '.'");
            }

            if (_model_for_creating_user_account.mail.Length > (int)NumbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_MAIL_LENGTH)
            {
                throw new AuthenticationException("Invalid mail!\nCan't be more than 100 characters");
            }

            if (string.IsNullOrEmpty(_model_for_creating_user_account.name) || !_model_for_creating_user_account.name.Contains(' '))
            {
                throw new AuthenticationException("Invalid name!\nCan't be null, has to contain space");
            }

            if (_model_for_creating_user_account.name.Length > (int)NumbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_NAME_LENGTH)
            {
                throw new AuthenticationException("Invalid name!\nCan't be more than 100 characters");
            }

            if (_model_for_creating_user_account.cnp.Length != (int)NumbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_CNP_LENGTH)
            {
                throw new AuthenticationException("Invalid cnp!\nHas to have length 13");
            }

            foreach (char characterFromCNP in _model_for_creating_user_account.cnp)
            {
                if (!char.IsDigit(characterFromCNP))
                {
                    throw new AuthenticationException("Invalid cnp!\nOnly numbers allowed");
                }
            }

            if (_model_for_creating_user_account.emergency_contact.Length != (int)NumbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_EMERGENCY_CONTACT_LENGTH)
            {
                throw new AuthenticationException("Invalid emergency contact!\nIt must have length 10");
            }

            foreach (char oneCharacterFromEmergencyContact in _model_for_creating_user_account.emergency_contact)
            {
                if (!char.IsDigit(oneCharacterFromEmergencyContact))
                {
                    throw new AuthenticationException("Invalid emergency contact!\nOnly numbers allowed");
                }
            }

            // check if model is at least 14 years old
            DateOnly _today_date = DateOnly.FromDateTime(DateTime.Now);
            DateOnly _min_valid_date = _today_date.AddYears((int)NumbersForValidationsWhenCreatingAnAccount.MINIMUM_AGE_FOR_USER);
            if (_model_for_creating_user_account.birth_date > _min_valid_date)
            {
                throw new AuthenticationException("Invalid Date\nPatient must be at least 14 years old!");
            }

            // check if valid gender
            switch (_model_for_creating_user_account.birth_date.Year <= (int)NumbersForValidationsWhenCreatingAnAccount.AGE_FOR_CHANGING_FIRST_DIGIT_OF_THE_CNP)
            {
                case true:
                    if (_model_for_creating_user_account.cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_THE_CNP] != '1' && _model_for_creating_user_account.cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_THE_CNP] != '2')
                    {
                        throw new AuthenticationException("cnp gender is invalid");
                    }

                    break;
                case false:
                    if (_model_for_creating_user_account.cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_THE_CNP] != '5' && _model_for_creating_user_account.cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_THE_CNP] != '6')
                    {
                        throw new AuthenticationException("cnp gender is invalid");
                    }

                    break;
            }

            if (_model_for_creating_user_account.birth_date.Year.ToString().Length != (int)NumbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_BIRTH_DATE_YEAR_LENGTH)
            {
                throw new AuthenticationException("cnp birth year errorYou may be old, but you surely aren't this old :)!");
            }

            // check if valid match between birth date and cnp birth date
            if (_model_for_creating_user_account.birth_date.Year.ToString().Substring((int)NumbersForValidationsWhenCreatingAnAccount.THIRD_DIGIT_FROM_BIRTH_DATE_OR_CNP, (int)NumbersForValidationsWhenCreatingAnAccount.THIRD_DIGIT_FROM_BIRTH_DATE_OR_CNP)
                != _model_for_creating_user_account.cnp.Substring((int)NumbersForValidationsWhenCreatingAnAccount.SECOND_DIGIT_FROM_CNP_OR_BIRTH_DATE, (int)NumbersForValidationsWhenCreatingAnAccount.THIRD_DIGIT_FROM_BIRTH_DATE_OR_CNP))
            {
                throw new AuthenticationException("Mismatch between Birth year and cnp birth year");
            }

            if (_model_for_creating_user_account.birth_date.Month.ToString().Length == (int)NumbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_BIRTH_DATE_MONTH_LENGTH)
            {
                if (_model_for_creating_user_account.birth_date.Month.ToString()[(int)NumbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_BIRTH_DATE]
                    != _model_for_creating_user_account.cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FIFTH_DIGIT_OF_THE_CNP]
                    || _model_for_creating_user_account.cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FOURTH_DIGIT_FROM_CNP] != '0')
                {
                    throw new AuthenticationException("Mismatch between Birth Month and cnp birth month");
                }
            }
            else
                if (_model_for_creating_user_account.birth_date.Month.ToString() !=
                _model_for_creating_user_account.cnp.Substring((int)NumbersForValidationsWhenCreatingAnAccount.FOURTH_DIGIT_FROM_BIRTH_DATE, (int)NumbersForValidationsWhenCreatingAnAccount.THIRD_DIGIT_FROM_BIRTH_DATE_OR_CNP))
            {
                throw new AuthenticationException("Mismatch between Birth Month and cnp birth month");
            }

            if (_model_for_creating_user_account.birth_date.Day.ToString().Length == (int)NumbersForValidationsWhenCreatingAnAccount.LIMIT_FOR_BIRTH_DATE_MONTH_LENGTH)
            {
                if (_model_for_creating_user_account.birth_date.Day.ToString()[(int)NumbersForValidationsWhenCreatingAnAccount.FIRST_DIGIT_FROM_BIRTH_DATE] != _model_for_creating_user_account.cnp[(int)NumbersForValidationsWhenCreatingAnAccount.SEVENTH_DIGIT_OF_THE_CNP] || _model_for_creating_user_account.cnp[(int)NumbersForValidationsWhenCreatingAnAccount.SIXTH_DIGIT_OF_THE_CNP] != '0')
                {
                    throw new AuthenticationException("Mismatch between Birth Day and cnp birth day");
                }
            }
            else
                if (_model_for_creating_user_account.birth_date.Day.ToString() != _model_for_creating_user_account.cnp.Substring((int)NumbersForValidationsWhenCreatingAnAccount.SIXTH_DIGIT_OF_THE_CNP, (int)NumbersForValidationsWhenCreatingAnAccount.THIRD_DIGIT_FROM_BIRTH_DATE_OR_CNP))
            {
                throw new AuthenticationException("Mismatch between Birth Day and cnp birth day");
            }

            bool _result = await _log_in_repository.createAccount(_model_for_creating_user_account);
            if (_result)
            {
                if (await loadUserByUsername(_model_for_creating_user_account.username))
                {
                    await logAction(ActionType.CREATE_ACCOUNT);
                    return await logAction(ActionType.LOGIN);
                }
            }

            return _result;
        }

        /// <summary>
        /// Sets the user's action.
        /// </summary>
        /// <param name="_action_type_login_or_logout">The type of the user's action.</param>
        /// <returns>The action setter.</returns>
        public async Task<bool> logAction(ActionType _action_type_login_or_logout)
        {
            return await _log_in_repository.authenticationLogService(all_user_information.user_id, _action_type_login_or_logout);
        }
    }
}
