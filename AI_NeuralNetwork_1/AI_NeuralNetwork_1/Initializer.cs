using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_NeuralNetwork_1
{
    class Initializer
    {
        private NeuralNet net;
        double high, mid, low;

        public Initializer()
        {
            high = .99;
            low = .01;
            mid = .5;
        }

        public void StartTraining()
        {
            double ll, lh, hl, hh;
            int count, iterations;
            bool verbose;
            double[][] input, output;
            StringBuilder strBuilder;

            net = new NeuralNet();
            strBuilder = new StringBuilder();

            input = new double[4][];
            input[0] = new double[] { high, high };
            input[1] = new double[] { low, high };
            input[2] = new double[] { high, low };
            input[3] = new double[] { low, low };

            output = new double[4][];
            output[0] = new double[] { low };
            output[1] = new double[] { high };
            output[2] = new double[] { high };
            output[3] = new double[] { low };

            verbose = false;
            count = 0;
            iterations = 5;

            net.Initialize(1, 2, 2, 1);

            do
            {
                count++;

                net.LearningRate = 3;
                net.Train(input, output, TrainingType.BackPropogation, iterations);

                net.PerceptionLayer[0].Output = low;
                net.PerceptionLayer[1].Output = low;

                net.Pulse();

                ll = net.OutputLayer[0].Output;

                net.PerceptionLayer[0].Output = high;
                net.PerceptionLayer[1].Output = low;

                net.Pulse();

                hl = net.OutputLayer[0].Output;

                net.PerceptionLayer[0].Output = low;
                net.PerceptionLayer[1].Output = high;

                net.Pulse();

                lh = net.OutputLayer[0].Output;

                net.PerceptionLayer[0].Output = high;
                net.PerceptionLayer[1].Output = high;

                net.Pulse();

                hh = net.OutputLayer[0].Output;

                if (verbose)
                {
                    strBuilder.Remove(0, strBuilder.Length);

                    strBuilder.Append("PERCEPTION LAYER<<<<<<<<<<<<<<<<<<<<<<<<");
                    foreach (Neuron pn in net.PerceptionLayer)
                        AppendNeuronInfo(strBuilder, pn);

                    strBuilder.Append("\nHIDDEN LAYER<<<<<<<<<<<<<<<<<<<<<<<<<<<");
                    foreach (Neuron hn in net.HiddenLayer)
                        AppendNeuronInfo(strBuilder, hn);

                    strBuilder.Append("\nOUTPUT LAYER<<<<<<<<<<<<<<<<<<<<<<<<<<<");
                    foreach (Neuron on in net.OutputLayer)
                        AppendNeuronInfo(strBuilder, on);

                    strBuilder.Append("\n");
                    strBuilder.Append("hh: \t").Append(hh.ToString()).Append(" \t< .5\n");
                    strBuilder.Append("ll: \t").Append(ll.ToString()).Append(" \t< .5\n");

                    strBuilder.Append("hl: \t").Append(hl.ToString()).Append(" \t> .5\n");
                    strBuilder.Append("lh: \t").Append(lh.ToString()).Append(" \t> .5\n");

                    Console.Write(strBuilder.ToString());
                }
            }
            while (hh > (mid + low) / 2 
                 || lh < (mid + high) / 2 
                 || hl < (mid + low) / 2 
                 || ll > (mid + high) / 2);

            net.PerceptionLayer[0].Output = low;
            net.PerceptionLayer[1].Output = low;

            net.Pulse();

            ll = net.OutputLayer[0].Output;

            net.PerceptionLayer[0].Output = high;
            net.PerceptionLayer[1].Output = low;

            net.Pulse();

            hl = net.OutputLayer[0].Output;

            net.PerceptionLayer[0].Output = low;
            net.PerceptionLayer[1].Output = high;

            net.Pulse();

            lh = net.OutputLayer[0].Output;

            net.PerceptionLayer[0].Output = high;
            net.PerceptionLayer[1].Output = high;

            net.Pulse();

            hh = net.OutputLayer[0].Output;

            strBuilder.Remove(0, strBuilder.Length);
            strBuilder.Append((count * iterations).ToString()).Append(" iterations required for trainning\n");

            strBuilder.Append("\n");
            strBuilder.Append("hh: \t").Append(hh.ToString()).Append(" \t< .5\n");
            strBuilder.Append("ll: \t").Append(ll.ToString()).Append(" \t< .5\n");

            strBuilder.Append("hl: \t").Append(hl.ToString()).Append(" \t> .5\n");
            strBuilder.Append("lh: \t").Append(lh.ToString()).Append(" \t> .5\n");

            Console.WriteLine(strBuilder.ToString());
        }

        public void Test(bool first, bool second)
        {
            bool result, verbose;
            StringBuilder strBuilder;

            verbose = false;
            strBuilder = new StringBuilder();

            net.PerceptionLayer[0].Output = first ? high : low;
            net.PerceptionLayer[1].Output = second ? high : low;

            net.Pulse();

            result = net.OutputLayer[0].Output > .5;

            if (verbose)
            {
                strBuilder.Remove(0, strBuilder.Length);

                strBuilder.Append("PERCEPTION LAYER <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n");
                foreach (Neuron pn in net.PerceptionLayer)
                    AppendNeuronInfo(strBuilder, pn);

                strBuilder.Append("\nHIDDEN LAYER <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n");
                foreach (Neuron hn in net.HiddenLayer)
                    AppendNeuronInfo(strBuilder, hn);

                strBuilder.Append("\nOUTPUT LAYER <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n");
                foreach (Neuron on in net.OutputLayer)
                    AppendNeuronInfo(strBuilder, on);

                strBuilder.Append("\n");

                Console.WriteLine(strBuilder.ToString());
            }

            Console.WriteLine("Result: " + result.ToString());
        }

        private static void AppendNeuronInfo(StringBuilder bld, INeuron neuron)
        {
            int i;
            double value;

            i = 1;
            value = 0;

            bld.Append("========== NEURON ========== \n");
            bld.Append(" output\t: ").Append(neuron.Output.ToString()).Append("\n");
            bld.Append(" error\t: ").Append(neuron.Error.ToString());
            bld.Append("\t last error:\t").Append(neuron.LastError.ToString()).Append("\n");
            //bld.Append(" bias value \t: ").Append(neuron.BiasValue.ToString()).Append("\n");
            bld.Append(" bias\t: ").Append(neuron.Bias.Weight.ToString()).Append("\n\n");



            foreach (KeyValuePair<INeuronSignal, NeuralFactor> f in neuron.Input)
            {
                bld.Append("input ").Append(i++.ToString()).Append(" value= ").Append(f.Key.Output.ToString()); //.Append("\n");
                bld.Append("  \tweight = ").Append(f.Value.Weight).Append("\n");


                value += f.Value.Weight * f.Key.Output;
                //bld.Append("\tSig(").Append((f.Key.Output * f.Value.Weight).ToString()).Append(")=").Append(Neuron.Sigmoid(f.Value.Weight + )).Append("\n");
            }

            bld.Append("parent.bias = ").Append(neuron.Bias.Weight).Append("\n");
            bld.Append("sigmoid=").Append(Neuron.Sigmoid(value + neuron.Bias.Weight)).Append("\n");
            bld.Append("============================ \n\n");

        }
    }
}
