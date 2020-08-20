using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GeocacheV2.Helper
{
    //class RelayCommand<T> : ICommand where T : class
    //{
    //    public event EventHandler CanExecuteChanged;

    //    private readonly Action<T> method;
    //    private readonly Func<bool> canExecute;

    //    public RelayCommand(Action<T> method)
    //    {
    //        this.method = method;
    //        //always enabled
    //        this.canExecute = () => { return true; };
    //    }

    //    public RelayCommand(Action<T> methodToExecute, Func<bool> func)
    //    {
    //        this.method = methodToExecute;
    //        this.canExecute = func;
    //    }

    //    public void RaiseCanExecuteChanged()
    //    {
    //        CanExecuteChanged?.Invoke(this, new EventArgs());
    //    }

    //    public bool CanExecute(object parameter)
    //    {
    //        if (canExecute != null)
    //            return canExecute();
    //        return true;
    //    }

    //    public void Execute(object parameter)
    //    {
    //        if (method != null)
    //        {
    //            this.method.Invoke((T)parameter);
    //        }
    //        else
    //            this.canExecute.Invoke();
    //    }
    //}
}
