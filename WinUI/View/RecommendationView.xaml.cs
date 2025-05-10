using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using WinUI.ViewModel;
using WinUI.View;
using System.Security.Authentication;
using System.Threading.Tasks;
using WinUI.Service;
using WinUI.Model;
using WinUI.Repository;
using WinUI.Proxy;
using System.Net.Http;
using System.Net.Http.Json;

namespace WinUI.View
{
    public sealed partial class RecommendationView : Page
    {
        public RecommendationView()
        {
            var doctor_service = new RecommendationSystemProxy(new HttpClient());
            RecommendationSystemService doctor_manager = new RecommendationSystemService(doctor_service);
            RecommendationSystemModel recommendation_system = new RecommendationSystemModel(doctor_manager);


            this.DataContext = new RecommendationSystemViewModel(recommendation_system);
            this.InitializeComponent();
        }
    }
}
