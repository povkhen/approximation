using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Events;
using LiveCharts.Wpf;

namespace ApproximationGUI
{
    public partial class MainWindow : Window
    {
        public bool point_get = false;
        double chosePontX = 0;
        double chosePontY = 0;
        public double y_mouse = 0;
        public static List<Point> points = new List<Point>();
        public static ChartValues<ScatterPoint> values_point_start = new ChartValues<ScatterPoint>();
        public static ChartValues<ObservablePoint> lagrange = new ChartValues<ObservablePoint>();
        public static ChartValues<ObservablePoint> squeres = new ChartValues<ObservablePoint>();
        public SeriesCollection MainSeriesCollection { get; set; }
        public SeriesCollection OnlyPointCollection { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            myBrowser.Navigate(new Uri("https://github.com/povkhen"));
            DataContext = this;
            pointsGrid.ItemsSource = points;
            pointsGrid2.ItemsSource = points;

            MainSeriesCollection = new SeriesCollection
            {
                new ScatterSeries
                {
                    Title = "StartPoints",
                    Values = values_point_start,
                    MinPointShapeDiameter = 15,
                    MaxPointShapeDiameter = 15,
                },
                new LineSeries
                {
                    Title = "Squeres",
                    Values = squeres
                    
                   
                },
                new LineSeries
                {
                    Title = "Lagrange",
                    Values = lagrange
                },
            };

            OnlyPointCollection = new SeriesCollection
            {
                new ScatterSeries
                {
                    Values = values_point_start,
                    MinPointShapeDiameter = 15,
                    MaxPointShapeDiameter = 15,
                }
            };

        }
        private void Update_Graph()
        {
            Apr_func_Gauss squer_func = new Apr_func_Gauss(points, 3);
            lagrange.Clear();
            squeres.Clear();
            for (double i = 0; i < points.Count + 1;)
            {
                lagrange.Add(new ObservablePoint(i, Apr_func_Lagr.InterpolateLagrangePolynomial(i, Get_lists().Item1, Get_lists().Item2, points.Count)));
                if (points.Count >= 3)
                {
                    squeres.Add(new ObservablePoint(i, squer_func.Get_y(i)));
                    CONTENT.Text = squer_func + "";
                }
                else CONTENT.Text = "Results...";
                i += 0.3;
            }
        }

 
        private void ButtonPopUpLogout_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();       
        private void Item_Create_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => tab_items.SelectedIndex = 0;
        private void Item_Message_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => tab_items.SelectedIndex = 1;
        private void Item_Count_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => tab_items.SelectedIndex = 2;
        private void Item_Chart_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => tab_items.SelectedIndex = 3;
        private void Item_Git_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => tab_items.SelectedIndex = 4;


        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {

            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }
        
        private void Button_Clear_Table_Click(object sender, RoutedEventArgs e)
        {
            points.Clear();
            values_point_start.Clear();
            Point.counter = 1;
            if (double.TryParse(Value_Point_x.Text, out double x))
            {
                Value_Point_x.Text = "0";
            }
            Update_DataGrid();
            Update_Graph();
        }
        private void Button_Delete_Point_Click(object sender, RoutedEventArgs e)
        {
            if (points.Any()) //prevent IndexOutOfRangeException for empty list
            {
                points.RemoveAt(points.Count - 1);
                values_point_start.RemoveAt(values_point_start.Count - 1);
                Point.counter -= 1;
                Update_DataGrid();
                Update_Graph();
                Button_Sub_Step_ClickX();
            }
        }

        private void Update_DataGrid()
        {
            pointsGrid.ItemsSource = null;
            pointsGrid.ItemsSource = points;

            pointsGrid2.ItemsSource = null;
            pointsGrid2.ItemsSource = points;
        }

        private void Button_Add_Step_ClickX()
        {
            if (double.TryParse(Value_Point_x.Text , out double x))
            {
                Value_Point_x.Text = (Math.Round(x + 1,0)).ToString();
            }
        }

        private void Button_Sub_Step_ClickX()
        {
            if (double.TryParse(Value_Point_x.Text, out double x))
            {
                Value_Point_x.Text = (Math.Round(x - 1, 0)).ToString();
            }
        }

        private void Button_Add_StepY_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Value_Point_y.Text, out double y))
            {
                Value_Point_y.Text = (Math.Round(y + slValue.Value, 2)).ToString();
            }
        }

        private void Button_Sub_StepY_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Value_Point_y.Text, out double y))
            {
                Value_Point_y.Text = (Math.Round(y - slValue.Value, 2)).ToString();
            }
        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            Error_textbox.Visibility = Visibility.Hidden;
            if (double.TryParse(Value_Point_x.Text, out double x) && double.TryParse(Value_Point_y.Text, out double y))
            {
                if (Point.counter <= 5)
                {
                points.Add(new Point(x, y));
                values_point_start.Add(new ScatterPoint(x, y));
                Button_Add_Step_ClickX();
                Update_DataGrid();
                Update_Graph();
                }
            }
            else
            {
                Error_textbox.Visibility = Visibility.Visible;
            }

        }
        

        private static Tuple<List<double>,List<double>> Get_lists()
        {
            List<double> x_values = new List<double>();
            List<double> y_values = new List<double>();
            foreach (Point point in points)
            {
                x_values.Add(point.Arg);
                y_values.Add(point.Value);
            }
            return new Tuple<List<double>, List<double>>(x_values, y_values);
    }


    private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Info",
                "Help Window",
                MessageBoxButton.OK,
                MessageBoxImage.Information,
                MessageBoxResult.OK,
                MessageBoxOptions.DefaultDesktopOnly);
        }

        private void ChartOnDataClick(object sender, ChartPoint p)
        {
            if (point_get == false)
            {
                var r = new Random();
                foreach (var series in OnlyPointCollection)
                {
                    foreach (var bubble in series.Values.Cast<ScatterPoint>())
                    {
                        if (bubble.X == p.X && bubble.Y == p.Y)
                        {
                            chosePontX = bubble.X;
                            chosePontY = bubble.Y;
                        }
                    }
                }
                point_get = true;
            }
            else
            {
                point_get = false;
            }

        }
        
        private void ChartMouseMove(object sender, MouseEventArgs e)
        {
            var point = Chart.ConvertToChartValues(e.GetPosition(Chart));
            y_mouse = point.Y;
            if (point_get)
            {
                foreach (var series in OnlyPointCollection)
                {
                    foreach (var bubble in series.Values.Cast<ScatterPoint>())
                    {
                        if (bubble.X == chosePontX && bubble.Y == chosePontY)
                        {
                            chosePontY = y_mouse;
                            bubble.Y = y_mouse;                            
                        }                        
                    }
                }
                foreach (Point item in points)
                {
                    if (item.Arg == chosePontX)
                    {
                        item.Value = y_mouse;
                        Update_DataGrid();
                        Update_Graph();
                    }
                }
            }
            
            
            X.Text = point.X.ToString("N");
            Y.Text = point.Y.ToString("N");
        }

        private void btnInternal_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            myBrowser.Navigate(new Uri("http://www.google.com"));
        }

        //private void btnExternal_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    Uri ui = new Uri(txtLoad.Text.Trim(), UriKind.RelativeOrAbsolute) ?? new Uri("https://github.com/povkhen");
        //    myBrowser.Navigate(ui);
        //}


    }
}
