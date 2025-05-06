using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Model;

namespace WinUI.Repository
{
    public interface IPatientRepository
    {
        /// <summary>
        /// Retrieves a list of all patients with their associated details.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="PatientJointModel"/> objects.</returns>
        Task<List<PatientJointModel>> getAllPatients();

        /// <summary>
        /// Retrieves a patient's details by their user ID.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="PatientJointModel"/> for the specified user.</returns>
        Task<PatientJointModel> getPatientByUserId(int user_id);

        /// <summary>
        /// Updates the password for a specified user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <param name="password">The new password to set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updatePassword(int user_id, string password);

        /// <summary>
        /// Updates the email address for a specified user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <param name="email">The new email address to set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updateEmail(int user_id, string email);

        /// <summary>
        /// Updates the username for a specified user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <param name="username">The new username to set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updateUsername(int user_id, string username);

        /// <summary>
        /// Updates the full name for a specified user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <param name="name">The new name to set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updateName(int user_id, string name);

        /// <summary>
        /// Updates the birth date for a specified user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <param name="birth_date">The new birth date to set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updateBirthDate(int user_id, DateOnly birth_date);

        /// <summary>
        /// Updates the address for a specified user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <param name="address">The new address to set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updateAddress(int user_id, string address);

        /// <summary>
        /// Updates the phone number for a specified user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <param name="phone_number">The new phone number to set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updatePhoneNumber(int user_id, string phone_number);

        /// <summary>
        /// Updates the emergency contact information for a specified user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <param name="emergency_contact">The new emergency contact to set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updateEmergencyContact(int user_id, string emergency_contact);

        /// <summary>
        /// Updates the weight for a specified user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <param name="weight">The new weight to set (in kilograms).</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updateWeight(int user_id, double weight);

        /// <summary>
        /// Updates the height for a specified user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <param name="height">The new height to set (in centimeters).</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updateHeight(int user_id, int height);

        /// <summary>
        /// Logs an update action for a specified user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <param name="type">The type of action performed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the log was successful; otherwise, false.</returns>
        Task<bool> logUpdate(int user_id, ActionType type);
    }
}
