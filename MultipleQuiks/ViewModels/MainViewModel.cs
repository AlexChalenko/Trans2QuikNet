using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MultipleQuiks.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<QuikViewModel> _quikViewModels = [];

        public Func<QuikViewModel?> AddNewQuik { get; set; }


        [RelayCommand]
        private void AddQuik()
        {
            var newQuik = AddNewQuik();
            QuikViewModels.Add(newQuik);
        }
    }
}
