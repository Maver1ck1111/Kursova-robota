using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<Desk>(provider => new Desk(DeskZone, 80));
            services.AddSingleton<IMovable, Desk>(provider => provider.GetRequiredService<Desk>());
            services.AddSingleton<IUtils, Desk>(provider => provider.GetRequiredService<Desk>());

            ServiceProvider container = services.BuildServiceProvider();
            container.GetRequiredService<Desk>();

            Container.SetContainer(container);

            UtilsHandler handler = new UtilsHandler(UtilsHandler);
            Algorithms algoritms = new Algorithms(Algoritms);
        }
    }
}