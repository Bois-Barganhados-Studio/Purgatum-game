using System.Collections;
using System.Collections.Generic;
using System;

public class NeuralNetwork
{
    int[] layer;
    Layer[] layers;
    int epochs;
    double tolerate;
    int randomSeed;
    double bias;
    int maxAttempts;
    public NeuralNetwork(int[] layer, int epochs = 100, int randomSeed = -1, double learningRate = 0.001, double bias = 1, double tolerate = 0.001, int maxAttempts = 5)
    {
        if(randomSeed == -1){
            randomSeed = (int)DateTime.Now.Ticks;       //If the random seed is -1, use the current time as the seed
        }
        this.maxAttempts = maxAttempts;
        this.bias = bias;
        this.epochs = epochs;
        this.tolerate = tolerate;
        this.randomSeed = randomSeed;
        //Deep copy	of layer array, the array of the number of neurons in each layer
        this.layer = new int[layer.Length];
        for (int i = 0; i < layer.Length; i++)
            this.layer[i] = layer[i];
        
        //Create layers of the neural network
        layers = new Layer[layer.Length -1];
        for (int i = 0; i < layers.Length; i++){
            layers[i] = new Layer(layer[i], layer[i + 1], randomSeed:randomSeed, bias:bias,learningRate:learningRate);
            layers[i].SetFunctions(Functions.Sigmoid, Functions.DSigmoid);
        }
    }

    public double[] FeedFoward(double[] inputs){
        this.layers[0].FeedFoward(inputs);      //FeedFoward the first layer with the inputs of the network
        for (int i = 1; i < this.layers.Length; i++){
            this.layers[i].FeedFoward(this.layers[i-1].outputs);        //Uses the previous layer to feed the next layer
        }

        return this.layers[this.layers.Length - 1].outputs;     //return the output of the last layer, the output layer
    }

    public double BackPropagation(double[] Expected){
        double totalError = 0;
        for(int i = layers.Length-1; i >= 0; i--){
            if(i == layers.Length - 1){
                totalError = this.layers[i].BackPropagationOutputLayer(Expected);       //If it is the output layer, use the BackPropagationOutputLayer method
            }
            else{
                 //If it is a hidden layer, use the BackPropagationHiddenLayer method
                this.layers[i].BackPropagationHiddenLayer(this.layers[i+1].gamma,this.layers[i+1].weights);    
            }
        }
        for (int i = 0; i < layers.Length; i++)
            layers[i].UpdateWeights();      //Update the weights of the layers

        return totalError;
    }
    
    public void SetFunctions(Func<double, double> activationFunction, Func<double, double> dactivationFunction){
        for (int i = 0; i < layers.Length; i++){
            layers[i].SetFunctions(activationFunction, dactivationFunction);      //Set the activation functions of the layers
        }
    }
    public void SetFunctions(int layer,Func<double, double> activationFunction, Func<double, double> dactivationFunction){
        layer -= 1;     //Ignoring the input layer
        if(layer < 0 || layer >= layers.Length)
            throw new Exception("Invalid layer");
        layers[layer].SetFunctions(activationFunction, dactivationFunction);      //Set the activation functions of the layers
    }

    public void train(double[][] inputs, double[][] outputs, double reiniciate = 0.1,bool log = false){
        double totalError = 0;
        int attempt = 0;
        if(inputs.Length != outputs.Length)
            throw new Exception("The number of inputs and outputs must be the same");
        for (int i = 0; i < epochs; i++){
            totalError = 0;
            for (int j = 0; j < inputs.Length; j++){
                FeedFoward(inputs[j]);
                totalError += BackPropagation(outputs[j]);
                // if(totalError < tolerate){
                //     i = epochs;
                //     j = inputs.Length;
                // }
            }
            //Shuffle the inputs and outputs to avoid overfitting
            shuffle(inputs, outputs);
            
            if(i % 100 == 0 && log)
                Console.WriteLine("Epoch: " + i + " Error: " + totalError + " executado: " + (((double)i)/epochs)*100 + "%");
            
            if(Math.Abs(totalError) < tolerate && log){
                System.Console.WriteLine("Stoped at epoch: " + i + " Error: " + totalError);
                i = epochs;
            }

            if(Math.Abs(totalError) > 100 && (((double)i)/epochs) > reiniciate){
                int seed = (int)DateTime.Now.Ticks;
                //Caso o erro seja muito grande, reinitialize os pesos da rede e treine novamente
                for (int k = 0; k < layers.Length; k++){
                    layers[k].InitializeWeights(randomSeed:seed,bias:bias);
                }
                System.Console.WriteLine("Explode at epoch: " + i + " Error: " + totalError + " attempt " + attempt + " of " + maxAttempts);
                i = 0;
                attempt++;
            }
            if(attempt > maxAttempts){
                System.Console.WriteLine("Max attempts reached, please change the parameters and try again");
                i = epochs;
            }
        }
    }

    public void shuffle(double[][] inputs, double[][] outputs){
        Random rnd = new Random(Seed:randomSeed);
        for (int i = 0; i < inputs.Length; i++){
            int index = rnd.Next(inputs.Length);
            double[] temp = inputs[i];
            inputs[i] = inputs[index];
            inputs[index] = temp;

            temp = outputs[i];
            outputs[i] = outputs[index];
            outputs[index] = temp;
        }
    }

    //Main Activation Functions
    public static class Functions{
        public static double Sigmoid(double x){
            return 1 / (1 + Math.Exp(-x));
        }
        public static double TanH(double x){
            return Math.Tanh(x);
        }
        public static double ReLu(double x){
            return Math.Max(0, x);
        }
        public static double LeakyReLu(double x){
            return Math.Max(0.01 * x, x);
        }
        public static double BinaryStep(double x){
            if(x < 0.5)
                return 0;
            else
                return 1;
        }
        public static double Linear(double x){
            return x;
        }
        public static double SoftPlus(double x){
            return Math.Log(1 + Math.Exp(x));
        }
        //Derivatives of the Activation Functions
        public static double DSigmoid(double x){
            return x * (1 - x);
        }
        public static double DTanH(double x){
            return 1 - (x * x);
        }
        public static double DReLu(double x){
            if(x > 0)
                return 1;
            else
                return 0;
        }
        public static double DLeakyReLu(double x){
            if(x > 0)
                return 1;
            else
                return 0.01;
        }
        public static double DBinaryStep(double x){
            return 0;
        }
        public static double DSoftPlus(double x){
            return 1 / (1 + Math.Exp(-x));
        }
        public static double DLinear(double x){
            return 1;
        }
        
    }

}