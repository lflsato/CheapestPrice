using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheapestPrice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Return the cheapest prices
        Product findCheapestPrices(Product[] array, ref Product[,] memoryMatrixComparison)
        {
            //loop through all the groups until it only return 1 group with 1 element
            while(true)
            {
                array=StepFindCheapestPrices(array, ref memoryMatrixComparison);
                if (array.Length == 1)
                    break;
            }
            return array[0];
        }

        //create groups and return each cheaper from each group
        Product[] StepFindCheapestPrices(Product[] array, ref Product[,] memoryMatrixComparison)
        {
            //creating groups of 2
            Product[,] groupArrays = new Product[2,array.Length/2];

            //distributing input numbers into groups
            for (int aux=0, insertedActualGroup=0, numGroup=0; aux < array.Length; aux++, insertedActualGroup++)
            {
                //verify if the group is full
                if(insertedActualGroup==2)
                {
                    //next group
                    insertedActualGroup = 0;
                    numGroup++;
                }
                groupArrays[insertedActualGroup, numGroup] = array[aux];
            }

            //check each group to find who is cheaper in each group
            Product[] cheapestEachGroup = new Product[array.Length / 2];
            for (int aux = 0; aux < array.Length / 2; aux++)
            {
                //verify if the group has ID -1 (1st cheapest)
                if (groupArrays[0, aux].ID == -1)
                {
                    cheapestEachGroup[aux] = new Product();
                    cheapestEachGroup[aux].ID = groupArrays[1, aux].ID;
                    cheapestEachGroup[aux].price = groupArrays[1, aux].price;
                }
                else if (groupArrays[1, aux].ID == -1)
                {
                    cheapestEachGroup[aux] = new Product();
                    cheapestEachGroup[aux].ID = groupArrays[0, aux].ID;
                    cheapestEachGroup[aux].price = groupArrays[0, aux].price;
                }
                else if (memoryMatrixComparison[groupArrays[0, aux].ID, groupArrays[1, aux].ID] != null) //verify if exist in matrix
                {
                    cheapestEachGroup[aux] = new Product();
                    cheapestEachGroup[aux].ID = memoryMatrixComparison[groupArrays[0, aux].ID, groupArrays[1, aux].ID].ID;
                    cheapestEachGroup[aux].price = memoryMatrixComparison[groupArrays[0, aux].ID, groupArrays[1, aux].ID].price;
                }
                else if (groupArrays[0, aux].ID == -1 || groupArrays[0, aux].price >= groupArrays[1, aux].price) //using comparisons
                {
                    cheapestEachGroup[aux] = new Product();
                    cheapestEachGroup[aux].ID = groupArrays[1, aux].ID;
                    cheapestEachGroup[aux].price = groupArrays[1, aux].price;

                    //store comparison into memoryMatrixComparison
                    memoryMatrixComparison[groupArrays[0, aux].ID, groupArrays[1, aux].ID] = new Product();
                    memoryMatrixComparison[groupArrays[0, aux].ID, groupArrays[1, aux].ID].ID = groupArrays[1, aux].ID;
                    memoryMatrixComparison[groupArrays[0, aux].ID, groupArrays[1, aux].ID].price = groupArrays[1, aux].price;
                    memoryMatrixComparison[groupArrays[1, aux].ID, groupArrays[0, aux].ID] = new Product();
                    memoryMatrixComparison[groupArrays[1, aux].ID, groupArrays[0, aux].ID].ID = groupArrays[1, aux].ID;
                    memoryMatrixComparison[groupArrays[1, aux].ID, groupArrays[0, aux].ID].price = groupArrays[1, aux].price;
                }
                else
                {
                    cheapestEachGroup[aux] = new Product();
                    cheapestEachGroup[aux].ID = groupArrays[0, aux].ID;
                    cheapestEachGroup[aux].price = groupArrays[0, aux].price;

                    //store comparison into memoryMatrixComparison
                    memoryMatrixComparison[groupArrays[0, aux].ID, groupArrays[1, aux].ID] = new Product();
                    memoryMatrixComparison[groupArrays[0, aux].ID, groupArrays[1, aux].ID].ID = groupArrays[0, aux].ID;
                    memoryMatrixComparison[groupArrays[0, aux].ID, groupArrays[1, aux].ID].price = groupArrays[0, aux].price;
                    memoryMatrixComparison[groupArrays[1, aux].ID, groupArrays[0, aux].ID] = new Product();
                    memoryMatrixComparison[groupArrays[1, aux].ID, groupArrays[0, aux].ID].ID = groupArrays[0, aux].ID;
                    memoryMatrixComparison[groupArrays[1, aux].ID, groupArrays[0, aux].ID].price = groupArrays[0, aux].price;
                }
            }


            return cheapestEachGroup;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Product[] array = new Product[8];
            array[0] = new Product();
            array[0].ID = 0;
            array[0].price = float.Parse(PriceBox1.Text);

            array[1] = new Product();
            array[1].ID = 1;
            array[1].price = float.Parse(PriceBox2.Text);

            array[2] = new Product();
            array[2].ID = 2;
            array[2].price = float.Parse(PriceBox3.Text);

            array[3] = new Product();
            array[3].ID = 3;
            array[3].price = float.Parse(PriceBox4.Text);

            array[4] = new Product();
            array[4].ID = 4;
            array[4].price = float.Parse(PriceBox5.Text);

            array[5] = new Product();
            array[5].ID = 5;
            array[5].price = float.Parse(PriceBox6.Text);

            array[6] = new Product();
            array[6].ID = 6;
            array[6].price = float.Parse(PriceBox7.Text);

            array[7] = new Product();
            array[7].ID = 7;
            array[7].price = float.Parse(PriceBox8.Text);

            Product[] cheapestPrices = new Product[2];
            Product[,] memoryMatrixComparison = new Product[array.Length, array.Length];

            //finding the first cheapest
            cheapestPrices[0] = findCheapestPrices(array, ref memoryMatrixComparison);

            
            //replacing the first cheapest by ID -1
            for (int aux=0; aux<array.Length; aux++)
            {
                if(array[aux].price==cheapestPrices[0].price)
                {
                    array[aux].ID = -1;
                    break;
                }
            }

            //finding the second cheapest
            cheapestPrices[1] = findCheapestPrices(array, ref memoryMatrixComparison);

            //seting results into UI
            Cheapest1.Text = cheapestPrices[0].price.ToString();
            Cheapest1.Text = cheapestPrices[0].price.ToString();
            Cheapest2.Text = cheapestPrices[1].price.ToString();
            Cheapest2.Text = cheapestPrices[1].price.ToString();
        }

    }
}
