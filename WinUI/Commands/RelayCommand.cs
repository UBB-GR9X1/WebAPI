using System;
using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool> _can_execute;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class with the specified execution logic.
    /// </summary>
    /// <param name="execute">The action to perform when executing the command.</param>
    public RelayCommand(Action execute) : this(execute, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class with the specified execution and can-execute logic.
    /// </summary>
    /// <param name="execute">The action to perform when executing the command.</param>
    /// <param name="can_execute">A function that determines whether the command can execute.</param>
    public RelayCommand(Action execute, Func<bool> can_execute)
    {
        this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this._can_execute = can_execute;
    }

    /// <summary>
    /// Determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">Data used by the command. Not used in this implementation.</param>
    /// <returns>true if the command can execute; otherwise, false.</returns>
    public bool CanExecute(object parameter) => _can_execute == null || _can_execute();

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">Data used by the command. Not used in this implementation.</param>
    public void Execute(object parameter) => _execute();

    /// <summary>
    // Event for handling when CanExecute changes
    /// </summary>
    public event EventHandler CanExecuteChanged;

    /// <summary>
    /// Raises the <see cref="CanExecuteChanged"/> event to indicate that the return value of <see cref="CanExecute"/> may have changed.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
