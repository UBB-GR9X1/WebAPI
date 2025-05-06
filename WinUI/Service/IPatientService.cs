using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinUI.Model;

namespace WinUI.Service
{
    public interface IPatientService
    {

        /// <summary>
        /// Gets the information of a single patient, typically the currently selected or loaded patient.
        /// </summary>
        PatientJointModel _patient_info { get; }

        /// <summary>
        /// Gets the list of all patients available in the system.
        /// </summary>
        List<PatientJointModel> _patient_list { get; }

        /// <summary>
        /// Loads patient information for the specified user ID.
        /// </summary>
        /// <param name="_user_id">The unique identifier of the user whose patient information is to be loaded.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the information was loaded successfully; otherwise, false.</returns>
        Task<bool> loadPatientInfoByUserId(int _user_id);

        /// <summary>
        /// Loads the information for all patients.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the information was loaded successfully; otherwise, false.</returns>
        Task<bool> loadAllPatients();

        /// <summary>
        /// Updates the weight for the specified user.
        /// </summary>
        /// <param name="_user_id">The unique identifier of the user.</param>
        /// <param name="weight">The new weight to set (in kilograms).</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updateWeight(int _user_id, double weight);

        /// <summary>
        /// Updates the height for the specified user.
        /// </summary>
        /// <param name="_user_id">The unique identifier of the user.</param>
        /// <param name="_height">The new height to set (in centimeters).</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updateHeight(int _user_id, int _height);

        /// <summary>
        /// Updates the emergency contact information for the specified user.
        /// </summary>
        /// <param name="_user_id">The unique identifier of the user.</param>
        /// <param name="_emergency_contact">The new emergency contact to set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> updateEmergencyContact(int _user_id, string _emergency_contact);

        /// <summary>
        /// Logs an update action for the specified user.
        /// </summary>
        /// <param name="_user_id">The unique identifier of the user.</param>
        /// <param name="_action">The type of action performed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the log was successful; otherwise, false.</returns>
        Task<bool> logUpdate(int _user_id, ActionType _action);
    }
}
