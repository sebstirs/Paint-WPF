using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;


namespace Rectangles_On_Image
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        private Point startPoint;
        private Rectangle newRectangle;
        bool rcheckboxValue = false, echeckboxValue = false, pcheckboxValue = false, resizecheckboxValue, dcheckboxValue;
        bool colorRed = false,
             colorBlue = false,
             colorGreen = false,
             colorWhite = false,
             colorPurple = false,
             colorBlack = false;
        SolidColorBrush brush = new SolidColorBrush(Colors.Black);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files| *.jpg;*.png;";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
            {
                picture.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //Draw Rectangles
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //if (rcheckboxValue == true && e.OriginalSource is not Rectangle)
        {
            startPoint = e.GetPosition(myCanvas);

            brush = new SolidColorBrush();

            if (colorRed == true) { brush.Color = Colors.Red; }
            else if (colorBlue == true) { brush.Color = Colors.Blue; }
            else if (colorGreen == true) { brush.Color = Colors.Green; }
            else if (colorWhite == true) { brush.Color = Colors.White; }
            else if (colorBlack == true) { brush.Color = Colors.Black; }
            else if (colorPurple == true) { brush.Color = Colors.Purple; }

            

            newRectangle = new Rectangle
            {
                StrokeThickness = 5
            };

            newRectangle.Stroke = brush;
            Guid guid = Guid.NewGuid();
            newRectangle.Tag = guid;

            Canvas.SetLeft(newRectangle, startPoint.X);
            Canvas.SetTop(newRectangle, startPoint.Y);
            myCanvas.Children.Add(newRectangle);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPoint = e.GetPosition(myCanvas);

            if (e.LeftButton == MouseButtonState.Pressed && resizecheckboxValue == true && e.OriginalSource is Rectangle)
            {
                Rectangle activeRectangle = (Rectangle)e.OriginalSource;

                double x = Math.Min(startPoint.X, currentPoint.X);
                double y = Math.Min(startPoint.Y, currentPoint.Y);

                double width = Math.Max(startPoint.X, currentPoint.X) - x;
                double height = Math.Max(startPoint.Y, currentPoint.Y) - y;

                activeRectangle.Width = width;
                activeRectangle.Height = height;

                Canvas.SetRight(activeRectangle, x);
                Canvas.SetBottom(activeRectangle, y);
            }

            if (e.LeftButton == MouseButtonState.Pressed && rcheckboxValue == true)
            {
                double x = Math.Min(startPoint.X, currentPoint.X);
                double y = Math.Min(startPoint.Y, currentPoint.Y);

                double width = Math.Max(startPoint.X, currentPoint.X) - x;
                double height = Math.Max(startPoint.Y, currentPoint.Y) - y;

                newRectangle.Width = width;
                newRectangle.Height = height;

                Canvas.SetRight(newRectangle, x);
                Canvas.SetBottom(newRectangle, y);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
             
            newRectangle = null;
        }
        //Save rectangles for future interation
        

        //Delete Objects
        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e) 
        {
            if (e.OriginalSource is Rectangle && e.RightButton == MouseButtonState.Pressed) 
            {
                Rectangle activeRectangle = (Rectangle)e.OriginalSource;
                myCanvas.Children.Remove(activeRectangle);
            }
        }

        //Change Rectangle Color
        private void changeColorRed(object sender, RoutedEventArgs e)
        {
            colorRed = true;
        }
        private void changeColorRedUnchecked(object sender, RoutedEventArgs e)
        {
            colorRed = false;
        }
        private void changeColorBlue(object sender, RoutedEventArgs e)
        {
            colorBlue = true;
        }
        private void changeColorBlueUnchecked(object sender, RoutedEventArgs e)
        {
            colorBlue = false;
        }
        private void changeColorGreen(object sender, RoutedEventArgs e)
        {
                colorGreen = true;
        }
        private void changeColorGreenUnchecked(object sender, RoutedEventArgs e)
        {
            colorGreen = false;
        }
        private void changeColorBlack(object sender, RoutedEventArgs e)
        {
                colorBlack = true;
        }
        private void changeColorBlackUnchecked(object sender, RoutedEventArgs e)
        {
            colorBlack = false;
        }
        private void changeColorWhite(object sender, RoutedEventArgs e)
        {
                colorWhite = true;
        }
        private void changeColorWhiteUnchecked(object sender, RoutedEventArgs e)
        {
            colorWhite = false;
        }
        private void changeColorPurple(object sender, RoutedEventArgs e)
        {
                colorPurple = true;
        }
        private void changeColorPurpleUnchecked(object sender, RoutedEventArgs e)
        {
            colorPurple = false;
        }

        //Buttons for draw tools 
        private void RCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            rcheckboxValue = true;
        }
        private void RCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            rcheckboxValue = false;
        }
        private void ECheckBox_Checked(object sender, RoutedEventArgs e)
        {

            echeckboxValue = true;
        }
        private void ECheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            echeckboxValue = false;
        }
        private void PCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            pcheckboxValue = true;
        }
        private void PCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pcheckboxValue = false;
        }

        private void ResizeCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            resizecheckboxValue = true;
        }
        private void ResizeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            resizecheckboxValue = false;
        }
        private void DCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            dcheckboxValue = true;
        }
        private void DCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            dcheckboxValue = false;
        }
        public void clearAll(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
        }
    }
}
