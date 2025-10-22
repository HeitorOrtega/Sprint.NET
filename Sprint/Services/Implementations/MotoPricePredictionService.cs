using Microsoft.ML;
using Sprint.DTOs.ML;
using System.Collections.Generic;
using Microsoft.ML.Data; 
using Microsoft.ML.Trainers.FastTree;

namespace Sprint.Services
{
    public class MotoPricePredictionService : IMotoPricePredictionService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _trainedModel;
        private readonly PredictionEngine<MotoPriceInput, MotoPricePrediction> _predictionEngine;

        public MotoPricePredictionService()
        {
            _mlContext = new MLContext();
            
            _trainedModel = TrainModel(_mlContext);

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<MotoPriceInput, MotoPricePrediction>(_trainedModel);
        }

        private ITransformer TrainModel(MLContext mlContext)
        {
            var data = GetSampleData();
            var trainingData = mlContext.Data.LoadFromEnumerable(data);
            
            var fastTreeOptions = new FastTreeRegressionTrainer.Options
            {
                LabelColumnName = "Label",
                FeatureColumnName = "Features",
                NumberOfTrees = 100,             
                NumberOfLeaves = 50,             
                MinimumExampleCountPerLeaf = 1   
            };

            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "CorEncoded", inputColumnName: nameof(MotoPriceInput.Cor))
        
                .Append(mlContext.Transforms.NormalizeMinMax(outputColumnName: nameof(MotoPriceInput.DiasUso), inputColumnName: nameof(MotoPriceInput.DiasUso)))
        
                .Append(mlContext.Transforms.Concatenate("Features", "CorEncoded", nameof(MotoPriceInput.DiasUso)))
       
                .Append(mlContext.Regression.Trainers.FastTree(fastTreeOptions));

            return pipeline.Fit(trainingData);
        }
        
        private static List<MotoPriceInput> GetSampleData()
        {
            return new List<MotoPriceInput>
            {
                new MotoPriceInput { Cor = "Vermelha", DiasUso = 15, PrecoAlvo = 28000f },
                new MotoPriceInput { Cor = "Preta", DiasUso = 50, PrecoAlvo = 27500f },
                
                new MotoPriceInput { Cor = "Azul", DiasUso = 180, PrecoAlvo = 24000f },
                new MotoPriceInput { Cor = "Amarela", DiasUso = 365, PrecoAlvo = 19000f },
                new MotoPriceInput { Cor = "Verde", DiasUso = 500, PrecoAlvo = 17500f },
                
                new MotoPriceInput { Cor = "Vermelha", DiasUso = 730, PrecoAlvo = 14000f },
                new MotoPriceInput { Cor = "Preta", DiasUso = 850, PrecoAlvo = 13500f },
                new MotoPriceInput { Cor = "Azul", DiasUso = 1000, PrecoAlvo = 12500f },
                
                new MotoPriceInput { Cor = "Cinza", DiasUso = 250, PrecoAlvo = 21000f },
                new MotoPriceInput { Cor = "Branca", DiasUso = 600, PrecoAlvo = 16500f },
            };
        }

        public MotoPricePrediction PredictPrice(MotoPriceInput input)
        {
            var prediction = _predictionEngine.Predict(input);
            
            if (prediction.PrecoPrevisto < 5000f) 
            {
                prediction.PrecoPrevisto = 5000f;
            }
            
            return prediction;
        }
    }
}