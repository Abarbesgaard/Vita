using System.Collections.ObjectModel;
using VitaHusApp.Model;
using VitaHusApp.Services;

namespace VitaHusApp.Pages
{
    public partial class VideoMenu : ContentPage
    {
        private readonly VideoService _videoService;
        public ObservableCollection<Video> Videos { get; set; } = new ObservableCollection<Video>();

        public VideoMenu()
        {
            InitializeComponent();
            _videoService = new VideoService(); // Initialiser VideoService
            BindingContext = this; // Bind denne side til XAML for at bruge bindinger
            LoadVideos(); // Hent videoer
        }

        private async void LoadVideos()
        {
            var videoList = await _videoService.GetAllVideo();
            if (videoList != null)
            {
                foreach (var video in videoList)
                {
                    Videos.Add(video); // Tilføj videoer til ObservableCollection
                }
            }
        }
    }
}
