namespace CustomVision_Demo
{
    public class PredictionResult
    {
        public float Probability;
        public string TagId;
        public string TagName;
    }
    public class CustomVisionResponse
    {
        public string Id;
        public string Project;
        public string Iteration;
        public string Created;
        public PredictionResult[] Predictions;
    }
}
