using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;

using System.ComponentModel;
using System.Threading;
using System.Windows.Interop;

using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.UserInterface;

using Alturos.Yolo;
using Alturos.Yolo.Model; 

using Point = OpenCvSharp.Point;
using Rect = OpenCvSharp.Rect;
using Size = OpenCvSharp.Size;




namespace ComputerVisionTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        //Window Constructor 
        public MainWindow()
        {
            InitializeComponent();
        }


        //died yolo sharp constants 
        #region const/readonly
        //YOLOv3
        //https://github.com/pjreddie/darknet/blob/master/cfg/yolov3.cfg
        private const string Cfg = "yolov3.cfg";

        //https://pjreddie.com/media/files/yolov3.weights
        private const string Weight = "yolov3.weights";

        //https://github.com/pjreddie/darknet/blob/master/data/coco.names
        private const string Names = "coco.names";

        //file location
        private const string Location = @"D:\Dropbox\!!_MyFiles\Architecture\C#\Computer Vision\Data\DarknetData\darknet-master\darknet-master\data";

        //random assign color to each label
        private static readonly Scalar[] Colors = Enumerable.Repeat(false, 80).Select(x => Scalar.RandomColor()).ToArray();

        //get labels from coco.names
        private static readonly string[] Labels = File.ReadAllLines(Path.Combine(Location, Names)).ToArray();
        #endregion

        //Fields 
        #region Fields  

        private List<string> inputTypes = new List<string>() { "Select Type", "Image", "Video", "Webcam" };

        #endregion

        //handle window loaded event 
        #region Window Loaded Event 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbox_InputType.ItemsSource = inputTypes;
            cbox_InputType.SelectedIndex = 0;
        }
        #endregion

        //window control events 
        #region Window Control Events 


        private void chbox_Detect_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chbox_Detect_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbox_Classify_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbox_Classify_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void cbox_Mask_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbox_Mask_Unchecked(object sender, RoutedEventArgs e)
        {

        }
        private void chbox_FaceDetect_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chbox_FaceDetect_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        //Combo Box Type Selection Methods
        #region Combo Box Type Selection


        //handle different selections types
        private void cbox_InputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {


                // add different enablers based on whats been selected - for now enable all
                switch (cbox_InputType.SelectedIndex)
                {
                    case 0:
                        tbox_FilePath.Clear();
                        checkAll(false);
                        break;
                    case 1:
                        tbox_FilePath.Clear();
                        checkAll(true);
                        cbox_WebCamSelector.IsEnabled = false;
                        break;
                    case 2:
                        tbox_FilePath.Clear();
                        checkAll(true);
                        cbox_WebCamSelector.IsEnabled = false;
                        break;
                    case 3:
                        checkAll(true);
                        tbox_FilePath.Clear();
                        tbox_FilePath.IsEnabled = false;
                        btn_Search.IsEnabled = false;

                        cbox_WebCamSelector.IsEnabled = true;

                  
                        break;
                    default:
                        checkAll(false);
                        break;
                }
            }
        }

     
        //function to check all checkboxes by bool value
        private void checkAll(bool Bool)
        {
            cbox_WebCamSelector.IsEnabled = Bool;
            tbox_FilePath.IsEnabled = Bool;
            btn_Search.IsEnabled = Bool;
            chbox_Detect.IsEnabled = Bool;
            chbox_Classify.IsEnabled = Bool;
            chbox_Mask.IsEnabled = Bool;

        }
        #endregion

        ////file browser
        #region FileBrowser
        string filePath = "";
        BitmapImage TestImage;

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            if (cbox_InputType.SelectedIndex == 1)
            {
                //open dialog for image

                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Title = "Select an Image";
                dialog.Filter = "Images (*.jpeg,*.png,*.jpg)|*.jpeg;*.png;*.jpg |All Files (*.*)|*.*";
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == true)
                {
                    filePath = dialog.FileName;

                    var index = filePath.LastIndexOf("\\");
                    var name = filePath.Substring(index + 1);
                    tbox_FilePath.Text = name;

                    try
                    {
                        TestImage = new BitmapImage(new Uri(filePath));
                        img_ContentViewer.Source = TestImage;

                        img_ContentViewer.Visibility = Visibility.Visible;
                    }
                    catch
                    {

                    }

                }
                else { }

            }
            else if (cbox_InputType.SelectedIndex == 2)
            {
                //open dialog for video 


                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Title = "Select a Video";
                dialog.Filter = "Videos (*.avi,*.mp4)|*.avi;*.mp4; |All Files (*.*)|*.*";
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == true)
                {
                    filePath = dialog.FileName;


                    var index = filePath.LastIndexOf("\\");
                    var name = filePath.Substring(index + 1);
                    tbox_FilePath.Text = name;

                }
                else { }

            }
            else if (cbox_InputType.SelectedIndex == 3)
            {
                //webcam - do nothing 


            }
        }

        #endregion


        //run image classifier 
        #region Run Image Classifier 

        private void btn_Run_Click(object sender, RoutedEventArgs e)
        {
            if (cbox_InputType.SelectedIndex == 1)
                if (img_ContentViewer.Source != null)
                {

                    //configure 

                    #region parameter
              //      var image = System.IO.Path.Combine(Location, "kite.jpg");
                    var cfg = Path.Combine(Location, Cfg);
                    var model = Path.Combine(Location, Weight);
                    const float threshold = 0.4f;       //for confidence 
                    const float nmsThreshold = 0.3f;    //threshold for nms
                    #endregion

                    //get image
                    var org = new Mat(filePath);

                    //setting blob, size can be:320/416/608
                    //opencv blob setting can check here https://github.com/opencv/opencv/tree/master/samples/dnn#object-detection
                    var blob = CvDnn.BlobFromImage(org, 1.0 / 255, new Size(320, 320), new Scalar(), true, false);

                    //load model and config, if you got error: "separator_index < line.size()", check your cfg file, must be something wrong.
                    var net = CvDnn.ReadNetFromDarknet(cfg, model);
                    #region set preferable
                    //    net.SetPreferableBackend(Net.Backend.OPENCV);

                    net.SetPreferableBackend( Net.Backend.OPENCV);
                    /*
                    0:DNN_BACKEND_DEFAULT 
                    1:DNN_BACKEND_HALIDE 
                    2:DNN_BACKEND_INFERENCE_ENGINE
                    3:DNN_BACKEND_OPENCV 
                     */
                    net.SetPreferableTarget(0);
                    /*
                    0:DNN_TARGET_CPU 
                    1:DNN_TARGET_OPENCL
                    2:DNN_TARGET_OPENCL_FP16
                    3:DNN_TARGET_MYRIAD 
                    4:DNN_TARGET_FPGA 
                     */
                    #endregion

                    //input data
                    net.SetInput(blob);

                    //get output layer name
                    var outNames = net.GetUnconnectedOutLayersNames();
                    //create mats for output layer
                    var outs = outNames.Select(_ => new Mat()).ToArray();

                    #region forward model
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    net.Forward(outs, outNames);

                    sw.Stop();
                    Console.WriteLine($"Runtime:{sw.ElapsedMilliseconds} ms");
                    #endregion

                    //get result from all output
                    GetResult(outs, org, threshold, nmsThreshold);

           //         using (new OpenCvSharp.Window("Image Classification", org))
           //         {
           //             Cv2.WaitKey();
           //         }
           

                    //convert mat to bitmapimage 
                 img_ContentViewer.Source = Extention.ToBitmapImage(  OpenCvSharp.Extensions.BitmapConverter.ToBitmap(org));

                    if (cbox_InputType.SelectedIndex == 2)
                    {
                        //run video classifier 
                    }

                    if (cbox_InputType.SelectedIndex == 3)
                    {
                        //Run WebCam Classifier
                        videoStat();

                        //Make visible
                        img_ContentViewer.Visibility = Visibility.Visible;
                   //     runVideoCapture();
                    }
                }
        }


        #endregion


        ////Webcam Stream
        #region Webcam Stream

        // Fields

        private VideoCapture capture;

        #region From Exampels test


        private PictureBoxIpl _pictureBoxIpl1;
        private BackgroundWorker _worker;


        private static double getFps(VideoCapture capture)
        {
            using (var image = new Mat())
            {
                while (true)
                {
                    /* start camera */
                    capture.Read(image);
                    if (!image.Empty())
                    {
                        break;
                    }
                }
            }

            using (var image = new Mat())
            {
                double counter = 0;
                double seconds = 0;
                var watch = Stopwatch.StartNew();
                while (true)
                {
                    capture.Read(image);
                    if (image.Empty())
                    {
                        break;
                    }

                    counter++;
                    seconds = watch.ElapsedMilliseconds / (double)1000;
                    if (seconds >= 3)
                    {
                        watch.Stop();
                        break;
                    }
                }
                var fps = counter / seconds;
                return fps;
            }
        }

        private void videoStat()
        {
            if (_worker != null && _worker.IsBusy)
            {
                return;
            }

            _worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            ///** make changes here
            ///

            //Run for Video
          //  _worker.DoWork += workerDoReadVideo;

            //Run For Web Vam
            _worker.DoWork += workerDoReadCamera;

            //if (RadioAvi.Checked)
            //{
            //    _worker.DoWork += workerDoReadVideo;
            //}

            //if (RadioWebCam.Checked)
            //{
            //    _worker.DoWork += workerDoReadCamera;
            //}

            _worker.ProgressChanged += workerProgressChanged;
            _worker.RunWorkerCompleted += workerRunWorkerCompleted;
            _worker.RunWorkerAsync();

            btn_Run.IsEnabled = false;
        }

        private void BtnStop_Click(object sender, System.EventArgs e)
        {
            if (_worker != null)
            {
                _worker.CancelAsync();
                _worker.Dispose();
            }
            btn_Run.IsEnabled = true;
        }

       //private void FrmMain_Load(object sender, System.EventArgs e)
       //{
       //    _pictureBoxIpl1 = new PictureBoxIpl
       //    {
       //        AutoSize = true
       //    };
       //    flowLayoutPanel1.Controls.Add(_pictureBoxIpl1);
       //}
      
        private void workerDoReadCamera(object sender, DoWorkEventArgs e)
        {
            using (var capture = new VideoCapture(index: 0))
            {
                var fps = getFps(capture);
                capture.Fps = fps;
                var interval = (int)(1000 / fps);

                using (var image = new Mat())
                {
                    while (_worker != null && !_worker.CancellationPending)
                    {
                        capture.Read(image);
                        if (image.Empty())
                            break;

                        _worker.ReportProgress(0, image);
                        Thread.Sleep(interval);
                    }
                }
            }
        }

        private void workerDoReadVideo(object sender, DoWorkEventArgs e)
        {
            using (var capture = new VideoCapture(@"..\..\Videos\drop.avi"))
            {
                var interval = (int)(1000 / capture.Fps);

                using (var image = new Mat())
                {
                    while (_worker != null && !_worker.CancellationPending)
                    {
                        capture.Read(image);
                        if (image.Empty())
                        {
                            break;
                        }

                        img_ContentViewer.Source = Extention.ToBitmapImage(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image));

                        _worker.ReportProgress(0, image);
                        Thread.Sleep(interval);
                    }
                }
            }
        }

        private void workerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var image = e.UserState as Mat;
            if (image == null) return;

            //Cv.Not(image, image);
            _pictureBoxIpl1.RefreshIplImage(image);
        }

        private void workerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _worker.Dispose();
            _worker = null;
            btn_Run.IsEnabled = true;
        }



        #endregion



        //private void runVideoCapture()
        //{
        //    try
        //    {
        //        //gets camera input index - work out way to select this // also works with parsing video file
        //        capture = new VideoCapture(0);
        //    }
        //    catch (Exception Ex)
        //    {
        //        MessageBox.Show(Ex.Message);
        //        return;
        //    }

        //    //check if this event handler is correct (actually functions)
        //    ComponentDispatcher.ThreadIdle += new System.EventHandler(ProcessFame);


        //}

        //private void ProcessFame(object sender, EventArgs e)
        //{
        //    // capture image through small frame to reduce processing 
        //   // var img = capture.QuerySmallFrame();

        //    var img = capture.frame

        //    var imgeOrigenal = img.ToImage<Bgr, Byte>().AsBitmap<Bgr, Byte>();

        //    TestImage = Extention.ToBitmapImage(imgeOrigenal);
        //    img_ContentViewer.Source = TestImage;


        //    Dectect();
        //}


        //private void Dectect()
        //{


        //    var configurationDetector = new ConfigurationDetector();
        //    var config = configurationDetector.Detect();
        //    using (var yoloWrapper = new YoloWrapper(config))
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {

        //            Bitmap bitmap = Extention.BitmapImage2Bitmap(TestImage);

        //            bitmap.Save(ms, ImageFormat.Png);


        //            ////BitmapSource bitmapSource = TestImage;
        //            ////Bitmap bitmap = new System.Drawing.Bitmap(bitmapSource);

        //            //using (var fs = new FileStream(filePath, FileMode.Open)) // keep file around
        //            //    {
        //            //        // create and save bitmap to memorystream
        //            //        using (var bmp = new Bitmap(TestImage.StreamSource)
        //            //        {
        //            //            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //            //        }
        //            //    }
        //            //    // write the PNG back to the same file from the memorystream
        //            //    using (var png = Image.FromStream(ms))
        //            //    {
        //            //        png.Save(path);
        //            //    }




        //            //   var byteArray = ImageToByte(TestImage);
        //            var items = yoloWrapper.Detect(ms.ToArray());

        //            dgrid_DataGrid.ItemsSource = items;

        //            DrawImage(bitmap, items);

        //        }
        //    }
        //}

        //private void DrawImage(Bitmap bmp, IEnumerable<YoloItem> items)
        //{
        //    var img = bmp;
        //    var font = new Font("Segoe UI", 18, System.Drawing.FontStyle.Bold);
        //    var brush = new SolidBrush(System.Drawing.Color.Red);
        //    Graphics graphics = Graphics.FromImage(img);
        //    foreach (var item in items)
        //    {
        //        var x = item.X;
        //        var y = item.Y;
        //        var width = item.Width;
        //        var height = item.Height;
        //        var tung = item.Type;
        //        var rect = new System.Drawing.Rectangle(x, y, width, height);
        //        var pen = new System.Drawing.Pen(System.Drawing.Color.LightGreen, 6);
        //        var point = new System.Drawing.Point(x, y);

        //        graphics.DrawRectangle(pen, rect);
        //        graphics.DrawString(item.Type, font, brush, point);
        //    }
        //    img_ContentViewer.Source = Extention.ToBitmapImage(img);
        //}


        #endregion


        #region New Methods from died_YolowithOpenCV4

        /// <summary>
        /// Get result form all output
        /// </summary>
        /// <param name="output"></param>
        /// <param name="image"></param>
        /// <param name="threshold"></param>
        /// <param name="nmsThreshold">threshold for nms</param>
        /// <param name="nms">Enable Non-maximum suppression or not</param>
        private static void GetResult(IEnumerable<Mat> output, Mat image, float threshold, float nmsThreshold, bool nms = true)
        {
            //for nms
            var classIds = new List<int>();
            var confidences = new List<float>();
            var probabilities = new List<float>();
            var boxes = new List<Rect2d>();

            var w = image.Width;
            var h = image.Height;
            /*
             YOLO3 COCO trainval output
             0 1 : center                    2 3 : w/h
             4 : confidence                  5 ~ 84 : class probability 
            */
            const int prefix = 5;   //skip 0~4

            foreach (var prob in output)
            {
                for (var i = 0; i < prob.Rows; i++)
                {
                    var confidence = prob.At<float>(i, 4);
                    if (confidence > threshold)
                    {
                        //get classes probability
                        Cv2.MinMaxLoc(prob.Row(i).ColRange(prefix, prob.Cols), out _, out OpenCvSharp.Point max);
                        var classes = max.X;
                        var probability = prob.At<float>(i, classes + prefix);

                        if (probability > threshold) //more accuracy, you can cancel it
                        {
                            //get center and width/height
                            var centerX = prob.At<float>(i, 0) * w;
                            var centerY = prob.At<float>(i, 1) * h;
                            var width = prob.At<float>(i, 2) * w;
                            var height = prob.At<float>(i, 3) * h;

                            if (!nms)
                            {
                                // draw result (if don't use NMSBoxes)
                                Draw(image, classes, confidence, probability, centerX, centerY, width, height);
                                continue;
                            }

                            //put data to list for NMSBoxes
                            classIds.Add(classes);
                            confidences.Add(confidence);
                            probabilities.Add(probability);
                            boxes.Add(new Rect2d(centerX, centerY, width, height));
                        }
                    }
                }
            }

            if (!nms) return;

            //using non-maximum suppression to reduce overlapping low confidence box
            CvDnn.NMSBoxes(boxes, confidences, threshold, nmsThreshold, out int[] indices);

            Console.WriteLine($"NMSBoxes drop {confidences.Count - indices.Length} overlapping result.");

            foreach (var i in indices)
            {
                var box = boxes[i];
                Draw(image, classIds[i], confidences[i], probabilities[i], box.X, box.Y, box.Width, box.Height);
            }

        }

        /// <summary>
        /// Draw result to image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="classes"></param>
        /// <param name="confidence"></param>
        /// <param name="probability"></param>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private static void Draw(Mat image, int classes, float confidence, float probability, double centerX, double centerY, double width, double height)
        {
            //label formating
            var label = $"{Labels[classes]} {probability * 100:0.00}%";
            Console.WriteLine($"confidence {confidence * 100:0.00}% {label}");
            var x1 = (centerX - width / 2) < 0 ? 0 : centerX - width / 2; //avoid left side over edge
            //draw result
            image.Rectangle(new Point(x1, centerY - height / 2), new Point(centerX + width / 2, centerY + height / 2), Colors[classes], 2);
            var textSize = Cv2.GetTextSize(label, HersheyFonts.HersheyTriplex, 0.5, 1, out var baseline);
            Cv2.Rectangle(image, new Rect(new Point(x1, centerY - height / 2 - textSize.Height - baseline),
                new Size(textSize.Width, textSize.Height + baseline)), Colors[classes], Cv2.FILLED);
            var textColor = Cv2.Mean(Colors[classes]).Val0 < 70 ? Scalar.White : Scalar.Black;
            Cv2.PutText(image, label, new Point(x1, centerY - height / 2 - baseline), HersheyFonts.HersheyTriplex, 0.5, textColor);
        }


        #endregion

    }

    //Extensions Methods
    #region Extension methods 
    public static class Extention
    {
       

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

      
    }

    #endregion
}


// new random test

