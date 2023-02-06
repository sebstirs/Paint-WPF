using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Drawing;
using Rectangle = System.Windows.Shapes.Rectangle;
using Point = System.Windows.Point;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

namespace Rectangles_On_Image
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        //Variables
        private Point startPoint;
        private Rectangle newRectangle;
        private Ellipse newEllipse;
        bool rcheckboxValue = false, echeckboxValue = false, pcheckboxValue = false, resizecheckboxValue, dcheckboxValue;
        bool colorRed = false,
             colorBlue = false,
             colorGreen = false,
             colorWhite = false,
             colorPurple = false,
             colorBlack = false;
        //Image picture = new Image();
        SolidColorBrush brush = new SolidColorBrush(Colors.Black);

        public MainWindow()
        {
            InitializeComponent();
        }

        //Choose and Upload a Picture
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image picture = new System.Windows.Controls.Image();
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files| *.jpg;*.png;";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                Uri fileUri = new Uri(filePath);
                picture.Source = new BitmapImage(fileUri);
                picture.Stretch = Stretch.Uniform;

                picture.Width = 250;
                picture.Height = 250;

                Canvas.SetLeft(picture, (canvas.ActualWidth / 2) - picture.Width / 2);
                Canvas.SetTop(picture, (canvas.ActualHeight / 2) - picture.Height / 2);

                canvas.Children.Add(picture);
            }
        }
        //Save Picture
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG image (*.jpg)|*.jpg";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (saveFileDialog.ShowDialog() == true)
            {
                RenderTargetBitmap renderTargetBitmap =
                    new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(canvas);
                BitmapEncoder bitmapEncoder = new JpegBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                using (Stream stream = saveFileDialog.OpenFile())
                {
                    bitmapEncoder.Save(stream);
                }
            }
        }
        //Close Window
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //Functions for When Mouse Button is Clicked
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(canvas);

            brush = new SolidColorBrush();

            //Check Value of Brush Color
            if (colorRed == true) { brush.Color = Colors.Red; }
            else if (colorBlue == true) { brush.Color = Colors.Blue; }
            else if (colorGreen == true) { brush.Color = Colors.Green; }
            else if (colorWhite == true) { brush.Color = Colors.White; }
            else if (colorBlack == true) { brush.Color = Colors.Black; }
            else if (colorPurple == true) { brush.Color = Colors.Purple; }

            //Recolor a Rectangle
            if (e.OriginalSource is Rectangle && e.LeftButton == MouseButtonState.Pressed)
            {
                Rectangle activeRectangle = e.OriginalSource as Rectangle;
                if (activeRectangle != null)
                {
                    activeRectangle.Stroke = brush;
                }

            }
            //Recolor an Ellipse
            if (e.OriginalSource is Ellipse && e.LeftButton == MouseButtonState.Pressed)
            {
                Ellipse activeEllipse = e.OriginalSource as Ellipse;
                if (activeEllipse != null)
                {
                    activeEllipse.Stroke = brush;
                }
            }
            //Recolor a Line
            if (e.OriginalSource is Line && e.LeftButton == MouseButtonState.Pressed)
            {
                Line activeLine = e.OriginalSource as Line;
                if (activeLine != null)
                {
                    activeLine.Stroke = brush;
                }
            }

            //Draw Rectangle
            if (e.LeftButton == MouseButtonState.Pressed && rcheckboxValue == true)
            {
                startPoint = e.GetPosition(canvas);
                newRectangle = new Rectangle()
                {
                    Stroke = brush,
                    StrokeThickness = 5
                };
                canvas.Children.Add(newRectangle);
                Canvas.SetLeft(newRectangle, startPoint.X);
                Canvas.SetTop(newRectangle, startPoint.Y);
            }
            //Draw Ellipse
            if (e.LeftButton == MouseButtonState.Pressed && echeckboxValue == true)
            {
                startPoint = e.GetPosition(canvas);
                newEllipse = new Ellipse()
                {
                    Stroke = brush,
                    StrokeThickness = 5
                };
                canvas.Children.Add(newEllipse);
                Canvas.SetLeft(newEllipse, startPoint.X);
                Canvas.SetTop(newEllipse, startPoint.Y);
            }
            //Draw Pen
            if (e.LeftButton == MouseButtonState.Pressed && pcheckboxValue == true)
            {
                Line line = new Line();
                line.Stroke = brush;
                line.StrokeThickness = 3;
                line.X1 = startPoint.X;
                line.Y1 = startPoint.Y;
                line.X2 = startPoint.X;
                line.Y2 = startPoint.Y;

                // Add the Line object to the Canvas
                canvas.Children.Add(line);
            }
        }

        //Functions for Mouse Movement When Mouse is over Canvas
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

            //Draw Rectangle
            if (e.LeftButton == MouseButtonState.Pressed && rcheckboxValue == true)
            {
                Point currentPoint = e.GetPosition(canvas);

                double x = Math.Min(startPoint.X, currentPoint.X);
                double y = Math.Min(startPoint.Y, currentPoint.Y);

                double width = Math.Max(startPoint.X, currentPoint.X) - x;
                double height = Math.Max(startPoint.Y, currentPoint.Y) - y;

                newRectangle.Width = width;
                newRectangle.Height = height;

            }
            //Resize Tool
            if (e.LeftButton == MouseButtonState.Pressed && resizecheckboxValue == true)
            {
                Rectangle activeRectangle = e.Source as Rectangle;
                if (activeRectangle != null)
                {
                    Point currentPoint = e.GetPosition(canvas);

                    activeRectangle.Width = currentPoint.X;
                    activeRectangle.Height = currentPoint.Y;
                }

            }
            //Drag Tool
            if (e.LeftButton == MouseButtonState.Pressed && dcheckboxValue == true)
            {
                Point currentPoint = e.GetPosition(canvas);
                Rectangle activeRectangle = e.Source as Rectangle;
                if (activeRectangle != null)
                {

                    double x = currentPoint.X - (activeRectangle.ActualWidth / 2);
                    double y = currentPoint.Y - (activeRectangle.ActualHeight / 2);

                    Canvas.SetLeft(activeRectangle, x);
                    Canvas.SetTop(activeRectangle, y);
                }
            }
            //Ellipse Tool
            if (e.LeftButton == MouseButtonState.Pressed && echeckboxValue == true)
            {
                Point currentPoint = e.GetPosition(canvas);
                if(newEllipse != null) 
                {
                    double x = Math.Min(startPoint.X, currentPoint.X);
                    double y = Math.Min(startPoint.Y, currentPoint.Y);

                    double width = Math.Max(startPoint.X, currentPoint.X) - x;
                    double height = Math.Max(startPoint.Y, currentPoint.Y) - y;

                    newEllipse.Width = width;
                    newEllipse.Height = height;
                }
                
            }
            //Line Tool
            if (e.LeftButton == MouseButtonState.Pressed && pcheckboxValue == true)
            {
                Point currentPoint = e.GetPosition(canvas);

                // Update the end point of the Line object
                Line line = (Line)canvas.Children[canvas.Children.Count - 1];
                line.X2 = currentPoint.X;
                line.Y2 = currentPoint.Y;
            }

        }

        //set Rectangle and Ellipse to null when Mouse is not clicked
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            newRectangle = null;
            newEllipse = null;
        }

        //Keep Objects from excceding Canvas Bounds
        private void canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            canvas.Children.OfType<UIElement>().ToList().ForEach(x => x.IsHitTestVisible = false);
        }

        //Delete All Objects on Canvas
        public void clearAll(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }
        //Delete Single Object from Canvas
        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Delete Rectangle
            if (e.OriginalSource is Rectangle && e.RightButton == MouseButtonState.Pressed)
            {
                Rectangle activeRectangle = (Rectangle)e.OriginalSource;
                canvas.Children.Remove(activeRectangle);
            }
            //Delete Ellipse
            if (e.OriginalSource is Ellipse && e.RightButton == MouseButtonState.Pressed)
            {
                Ellipse activeEllipse = (Ellipse)e.OriginalSource;
                canvas.Children.Remove(activeEllipse);
            }
            //Delete Line
            if (e.OriginalSource is Line && e.RightButton == MouseButtonState.Pressed)
            {
                Line activeLine = (Line)e.OriginalSource;
                canvas.Children.Remove(activeLine);
            }
        }

        //Radio Buttons for Changing Color
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

        //Radio Buttons for draw tools 
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
    }
}