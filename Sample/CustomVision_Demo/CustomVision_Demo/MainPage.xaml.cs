using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media;
using Windows.Web.Http;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CustomVision_Demo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModel ModelGen;
        private Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModelInput ModelInput = new Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModelInput();
        private Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModelOutput ModelOutput;
        private async void LoadModel()
        {
            //Load a machine learning model
            StorageFile modelFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/catdogmodel.onnx"));
            ModelGen = await Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModel.CreateEe7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModel(modelFile);
        }
        public MainPage()
        {
            this.InitializeComponent();
            LoadModel();
        }
        private async void recognizeButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".bmp");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            var inputFile = await fileOpenPicker.PickSingleFileAsync();

            if (inputFile == null)
            {
                // The user cancelled the picking operation
                return;
            }
            TimeSpan duration;
            imageLabel.Text = "Recognizing...";
            DateTime startTime = DateTime.UtcNow;
            SoftwareBitmap softwareBitmap;

            using (IRandomAccessStream stream = await inputFile.OpenAsync(FileAccessMode.Read))
            {
                // Create the decoder from the stream
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                // Get the SoftwareBitmap representation of the file
                softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore);
                var source = new SoftwareBitmapSource();
                await source.SetBitmapAsync(softwareBitmap);
                previewImage.Source = source;

                VideoFrame vf = VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);

                ModelInput.data = vf;
                try
                {
                    ModelOutput = await ModelGen.EvaluateAsync(ModelInput);
                    string labelText = "";
                    labelText += string.Format("{0}\r\n", ModelOutput.classLabel[0]);
                    duration = DateTime.UtcNow - startTime;
                    labelText += string.Format("\r\n\r\n\r\n\r\n{0:#}ms", duration.TotalMilliseconds);
                    imageLabel.Text = labelText;
                }
                catch (Exception ex)
                {
                    imageLabel.Text = ex.Message;
                }
            }
        }

        private async void recognizeButtonCloud_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".bmp");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            var inputFile = await fileOpenPicker.PickSingleFileAsync();

            if (inputFile == null)
            {
                // The user cancelled the picking operation
                return;
            }
            TimeSpan duration;
            imageLabelCloud.Text = "";
            imageLabelCloud.FontSize = 28;
            imageLabelCloud.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
            imageLabelCloud.Text = "Recognizing...";
            DateTime startTime = DateTime.UtcNow;
            SoftwareBitmap softwareBitmap;


            using (IRandomAccessStream stream = await inputFile.OpenAsync(FileAccessMode.Read))
            {
                // Create the decoder from the stream
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                // Get the SoftwareBitmap representation of the file
                softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore);
                var source = new SoftwareBitmapSource();
                await source.SetBitmapAsync(softwareBitmap);
                previewImage.Source = source;

                HttpClient client = new HttpClient();
                HttpResponseMessage response;
                string result;
                client.DefaultRequestHeaders.Add("Prediction-key", APIInfo.predictionKey);
                Uri RequestUri = new Uri(
                    string.Format("https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/{0}/image?iterationId={1}", APIInfo.projectId, APIInfo.iterationId));
                using (HttpStreamContent httpStreamContent = new HttpStreamContent(stream))
                {
                    httpStreamContent.Headers.ContentType = new Windows.Web.Http.Headers.HttpMediaTypeHeaderValue("application/octet-stream");
                    try
                    {
                        response = await client.PostAsync(RequestUri, httpStreamContent);
                        result = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            CustomVisionResponse customVisionResponse = JsonConvert.DeserializeObject<CustomVisionResponse>(result);
                            string topTagName = "";
                            float topProbability = 0;
                            string labelText = "";
                            foreach (PredictionResult predictionResult in customVisionResponse.Predictions)
                            {
                                if (predictionResult.Probability > topProbability)
                                {
                                    topProbability = predictionResult.Probability;
                                    topTagName = predictionResult.TagName;
                                }
                            }
                            labelText += string.Format("{0}\r\n", topTagName);
                            duration = DateTime.UtcNow - startTime;
                            labelText += string.Format("\r\n\r\n\r\n\r\n{0:#}ms", duration.TotalMilliseconds);
                            imageLabelCloud.Text = labelText;
                        }
                        else
                        {
                            imageLabelCloud.Text = response.StatusCode.ToString() + "\r\n" + result;
                        }
                    }
                    catch (Exception ex)
                    {
                        imageLabelCloud.FontSize = 22;
                        imageLabelCloud.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                        imageLabelCloud.Text = ex.Message;
                    }
                }
            }
        }
    }
}
