using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.AI.MachineLearning.Preview;

// ee7165d4-31e3-468e-ba0c-565f0b33d377_1bc49a81-827d-48b3-84bf-3a9555b4c54e

namespace CustomVision_Demo
{
    public sealed class Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModelInput
    {
        public VideoFrame data { get; set; }
    }

    public sealed class Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModelOutput
    {
        public IList<string> classLabel { get; set; }
        public IDictionary<string, float> loss { get; set; }
        public Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModelOutput()
        {
            this.classLabel = new List<string>();
            this.loss = new Dictionary<string, float>()
            {
                { "cat", float.NaN },
                { "dog", float.NaN },
            };
        }
    }

    public sealed class Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModel
    {
        private LearningModelPreview learningModel;
        public static async Task<Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModel> CreateEe7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModel(StorageFile file)
        {
            LearningModelPreview learningModel = await LearningModelPreview.LoadModelFromStorageFileAsync(file);
            Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModel model = new Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModel();
            model.learningModel = learningModel;
            return model;
        }
        public async Task<Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModelOutput> EvaluateAsync(Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModelInput input) {
            Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModelOutput output = new Ee7165d4_x002D_31e3_x002D_468e_x002D_ba0c_x002D_565f0b33d377_1bc49a81_x002D_827d_x002D_48b3_x002D_84bf_x002D_3a9555b4c54eModelOutput();
            LearningModelBindingPreview binding = new LearningModelBindingPreview(learningModel);
            binding.Bind("data", input.data);
            binding.Bind("classLabel", output.classLabel);
            binding.Bind("loss", output.loss);
            LearningModelEvaluationResultPreview evalResult = await learningModel.EvaluateAsync(binding, string.Empty);
            return output;
        }
    }
}
