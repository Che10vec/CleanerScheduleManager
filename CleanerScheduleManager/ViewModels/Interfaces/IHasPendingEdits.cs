namespace CleanerScheduleManager.ViewModels.Base
{
    public abstract partial class ViewModelBase
    {
        public interface IHasPendingEdits
        {
            void FinalizeEdits();
        }
    }
}
