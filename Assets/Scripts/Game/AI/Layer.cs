using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Layer{
    int numberOfInputs;     //Number of neurons that are inputing in the layer
    int numberOfOutputs;        //Number of neurons that are outputing in the layer
    double learningRate;  //Learning rate of the layer

    double[] inputs;    //Inputs of the layer
    public double[] outputs;   //Outputs of the layer
    public double[][] weights;  //Weights of the layer, divided in rows and columns Ex.: [[w11, w12, w13], [w21, w22, w23]] 2 outputs, 3 inputs
    double[,] weightsDelta;    //The values that will update the weights
    public double[] gamma;     //Gamma value of each neuron, the value of the error times the derivative of the activation function
    double[] error;     //Error value of each neuron
    public double[] bias;     //Bias of the layer
    public double[] biasWeight;     //Bias of the layer
    public double[] biasWeightDelta;     //Bias of the layer
    Func<double, double> activationFunction;     //Activation function of the layer
    Func<double, double> activationFunctionDerivative;     //Derivative of the activation function of the layer
    public Layer(int numberOfInputs, int numberOfOutputs, int randomSeed = -1, double bias = 1, double learningRate = 0.001){
        if(randomSeed == -1){
            randomSeed = (int)DateTime.Now.Ticks;       //If the random seed is -1, use the current time as the seed
        }
        this.numberOfInputs = numberOfInputs;
        this.numberOfOutputs = numberOfOutputs;
        this.learningRate = learningRate;
        this.bias = new double[numberOfOutputs];
        this.biasWeight = new double[numberOfOutputs];
        this.biasWeightDelta = new double[numberOfOutputs];
        this.inputs = new double[numberOfInputs];
        this.outputs = new double[numberOfOutputs];
        this.weights = new double[numberOfOutputs][];
        this.weightsDelta = new double[numberOfOutputs, numberOfInputs];
        this.gamma = new double[numberOfOutputs];
        this.error = new double[numberOfOutputs];
        this.activationFunction = (x) => {return (1 / (1 + Math.Exp(-x)));};
        this.activationFunctionDerivative = (x) => {return (1 - (x * x));};

        for (int i = 0; i < this.outputs.Length; i++){
            this.weights[i] = new double[numberOfInputs];
        }
        InitializeWeights(randomSeed, bias:bias);
    }

    public void InitializeWeights(int randomSeed, double bias=1){
        System.Random gen = new System.Random(Seed: randomSeed);
        for (int i = 0; i < this.outputs.Length; i++){
            this.bias[i] = bias;     //Random value between -0.5 and 0.5
            this.biasWeight[i] = gen.NextDouble() - 0.5;     //Random value between -0.5 and 0.5
            for (int j = 0; j < this.inputs.Length; j++)
            {
                this.weights[i][j] = gen.NextDouble() - 0.5;     //Random value between -0.5 and 0.5
            }
        }
    }


    public double[] FeedFoward(double[] inputs){
        this.inputs = inputs;       //reinicializate the inputs
        for (int i = 0; i < this.outputs.Length; i++)
        {
            this.outputs[i] = this.bias[i] * this.biasWeight[i];
            for(int j = 0; j < this.inputs.Length; j++){
                this.outputs[i] += this.inputs[j] * this.weights[i][j];
            }
            this.outputs[i] = ActivationFunction(this.outputs[i]);
        }
        return outputs;
    }

    //Activation function TANH
    public double ActivationFunction(double value){
        return this.activationFunction(value);
        //return Math.Tanh(value);
    }

    public double ActivationFunctionDerivative(double value){
        return this.activationFunctionDerivative(value);
        //return (1 - (value * value));
    }

    public void SetFunctions(Func<double, double> activationFunction, Func<double, double> activationFunctionDerivative){
        this.activationFunction = activationFunction;
        this.activationFunctionDerivative = activationFunctionDerivative;
    }

    public double BackPropagationOutputLayer(double[] expected){
        //Calculate the error of each neuron
        for(int i = 0; i < this.error.Length; i++){
            this.error[i] = this.outputs[i] - expected[i];   //Calculate the real error
        }
        for(int i = 0; i < this.error.Length; i++){
            
            this.gamma[i] = this.error[i] * ActivationFunctionDerivative(this.outputs[i]);      //Calculate the error depending on the output (chain rule)
        }
        //Calculate the delta of the weights
        for(int i = 0; i < this.outputs.Length; i++){
            this.biasWeightDelta[i] = this.gamma[i] * this.bias[i];
            for(int j = 0; j < this.inputs.Length; j++){
                this.weightsDelta[i,j] = this.gamma[i] * this.inputs[j];
            }
        }

        //Sum the error of each neuron to stop the training
        double sum = 0;
        for(int i = 0; i < this.error.Length; i++){
            sum += (this.error[i] * this.error[i]);
        }

        return sum;
    }

    public void BackPropagationHiddenLayer(double[] gammaFoward, double[][] foward){
        for(int i = 0; i < this.outputs.Length; i++){
            this.gamma[i] = 0;
            for(int j = 0; j < gammaFoward.Length; j++){
                //Calculate the gamma of the hidden layer, based on the gamma of the foward layer and the weight between this neuron and the next
                this.gamma[i] += gammaFoward[j] * foward[j][i];      
            }
            this.gamma[i] *= ActivationFunctionDerivative(this.outputs[i]);     //Calculate the gamma of the hidden layer, based on the output of the neuron
        }

        //Calculate the delta of the weights, to update the weights later
        for(int i = 0; i < this.outputs.Length; i++){
            this.biasWeightDelta[i] = this.gamma[i] * this.bias[i];
            for(int j = 0; j < this.inputs.Length; j++){
                this.weightsDelta[i,j] = this.gamma[i] * this.inputs[j];
            }
        }
    }

    public void UpdateWeights(){
        for (int i = 0; i < this.outputs.Length; i++)
        {
            this.biasWeight[i] -= this.biasWeightDelta[i] * this.learningRate;
            for (int j = 0; j < this.inputs.Length; j++)
            {
                this.weights[i][j] -= this.weightsDelta[i,j] * this.learningRate;
            }
        }
    }


    

}