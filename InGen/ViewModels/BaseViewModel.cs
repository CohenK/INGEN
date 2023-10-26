namespace InGen.ViewModels
{
    public partial class BaseViewModel: ObservableObject
    {
        #region member variables
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        public bool IsNotBusy => !IsBusy;

        [ObservableProperty]
        string title;
        #endregion

        public BaseViewModel() { }
    }
}
