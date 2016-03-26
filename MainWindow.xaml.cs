using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AntColonySimulation
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Colony MyColony = new Colony();

        public static int DiceRoll(int i, int j)
        {
            Random dice = new Random();
            int result = dice.Next(i, j);
            return result;
        }





        public MainWindow()
        {
            InitializeComponent();
            Turn.Click += Turn_Click;
            RollDice.Click += RollDice_Click;
            ForageButton.Click += ForageButton_Click;
            DigButton.Click += DigButton_Click;
            NurseButton.Click += NurseButton_Click;
            NextWorkerButton.Click += NextWorkerButton_Click;

            MyColony.RemoveAnt += MyColony_RemoveAnt;
            MyColony.AddAnt += MyColony_AddAnt;

        }

        private void ForageButton_Click(object sender, RoutedEventArgs e)
        {

            if (MyColony.WorkerList.Count > 0)
            {
                int result = MyColony.WorkerList[MyColony.CurrentAnt].Forage();
                MyColony.ForageTotal += result;
                RollDiceTextBox.Text = result.ToString();
                FoodCollected.Text = MyColony.ForageTotal.ToString();
            }   /*
            switch (result)
            {
                case 1:
                    MessageBox.Show("The worker came near a nest of Argentine ants and was overwhelmed.");
                    break;
                case 2:
                case 3:
                    MessageBox.Show("The worker was eaten by a hunting spider.");
                    break;
                case 4:
                    MessageBox.Show("The worker was harassed when it came near a nest of Forelius ants.");
                    break;

                case 78:
                    MessageBox.Show("The worker found a bonanza of swarming termites and proceeded to bring a winged termite king back to the nest.");
                    MyColony.Food += 50;
                    break;
                case 79:
                case 80:
                    MessageBox.Show("The worker found a juicy termite worker.");
                    MyColony.Food += 30;
                    break;
            }
            */

        }
        private void NurseButton_Click(object sender, RoutedEventArgs e)
        {
            if(MyColony.WorkerList.Count>0)
            {
                MyColony.NurseTotal += MyColony.WorkerList[MyColony.CurrentAnt].Nurse();
                RollDiceTextBox.Text = MyColony.WorkerList[MyColony.CurrentAnt].Nurse().ToString();
                NursesTextBox.Text = MyColony.NurseTotal.ToString();
            }
        }

        private void DigButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyColony.WorkerList.Count > 0)
            {
                MyColony.NestSize += MyColony.WorkerList[MyColony.CurrentAnt].Dig();
                RollDiceTextBox.Text = MyColony.WorkerList[MyColony.CurrentAnt].Dig().ToString();
            }
        }

        private void NextWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyColony.WorkerList.Count > 0)
            {
                MyColony.NextWorker();
                CurrentAntStatsLabel.Content = MyColony.WorkerList[MyColony.CurrentAnt].ToString();
            }
            
        }

         

        private void RollDice_Click(object sender, RoutedEventArgs e)
        {
            RollDiceTextBox.Text = DiceRoll(1, 7).ToString();
        }

        private void Turn_Click(object sender, RoutedEventArgs e)
        {
            MyColony.CreateBroodList(MyColony.AntList);
            MyColony.CreateWorkerList(MyColony.AntList);

            MyColony.EggBirth();
            MyColony.IncrementAge();
            MyColony.BroodMetamorphosis();
            MyColony.AntCalc();
            MyColony.Nurse(MyColony.NurseTotal);
            

            MyColony.CurrentAnt = 0;
            if (MyColony.WorkerList.Count>=1)
                CurrentAntStatsLabel.Content = MyColony.WorkerList[MyColony.CurrentAnt].ToString();
            
                try
                {
                //Food Calculation
                int totalfoodcollected = int.Parse(FoodStreamTextBox.Text) + MyColony.ForageTotal;
                    MyColony.FoodCalc(totalfoodcollected);

                    if (MyColony.Food < 0)
                    {
                        int foodDeficit = MyColony.Food * -1;
                        MessageLabel.Content += "\nThe food deficit was " + foodDeficit + " units";

                        int i = 0;
                            do
                            {
                        MyColony.BroodList[(DiceRoll(0, MyColony.BroodList.Count))].Age--;
                                i++;
                            MyColony.CreateBroodList(MyColony.AntList);

                        } while (i < foodDeficit);

                        MessageLabel.Content += "\nFood is scarce and brood development is slow or at a standstill.";
                
                        MyColony.Food = 0;

                    }
                    //Nursing efficacy calculation
                    switch (MyColony.Nurse(float.Parse(NursesTextBox.Text)))
                    {
                        case 0:
                            MessageLabel.Content = "Some of the brood are sickly due to a lack of ant nurses.";
                            break;
                        case 1:
                            MessageLabel.Content = "Brood development is normal.";
                            break;
                        case 2:
                            MessageLabel.Content = "The brood are receiving excellent care.";
                            break;
                        default:
                            MessageBox.Show("Error");
                            break;
                    }
                    

                    //Nest Size calculation
                    if (MyColony.NestSize + int.Parse(NestSizeTextBox.Text) >= 6)
                        MyColony.NestSize += int.Parse(NestSizeTextBox.Text);
                    else
                    {
                        MyColony.NestSize = 6;
                        MessageBox.Show("The nest size cannot go below 6 cubic centimeters");
                    }
                    if (MyColony.NestSize < (MyColony.Pupae + MyColony.Larvae + MyColony.Eggs + MyColony.Food / 4))
                        MessageLabel.Content += "\nThe Nest is getting crowded. Stored food or larvae may suffer.";

                //Outputting values to screen
                MinorCount.Content = MyColony.Minors;
                MajorCount.Content = MyColony.Majors;
                MaleCount.Content = MyColony.Males;
                FemaleCount.Content = MyColony.Females;
                FoodCount.Content = MyColony.Food;
                NestSizeCount.Content = MyColony.NestSize;
                EggCount.Content = MyColony.Eggs;
                LarvaeCount.Content = MyColony.Larvae;
                PupaeCount.Content = MyColony.Pupae;
                DayCount.Content = MyColony.Day;
                MyColony.AntCalcClear();

                MyColony.ForageTotal = 0;
                MyColony.NurseTotal = 0;
                FoodCollected.Text = "0";
                NursesTextBox.Text = "0";
                NestSizeTextBox.Text = "0";

                MyColony.Day++;


                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("You did not enter a value.");
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter integers only.");
                }
                catch (OverflowException)
                {
                    MessageBox.Show("You have entered too large of a number.");
                }
                catch
                {
                    MessageBox.Show("Error.");
                }

            }
            /*
            //Buttons denoting death of ants and larvae
            private void MajorLost_Click(object sender, RoutedEventArgs e)
            {
                MyColony.Majors--;
                MajorCount.Content = MyColony.Majors;

            }

            private void MinorLost_Click(object sender, RoutedEventArgs e)
            {
                MyColony.Minors--;
                MinorCount.Content = MyColony.Minors;
            }
            f
            private void MaleLost_Click(object sender, RoutedEventArgs e)
            {
                MyColony.Males--;
                MaleCount.Content = MyColony.Males;

            }

            private void FemaleLost_Click(object sender, RoutedEventArgs e)
            {
                MyColony.Females--;
                FemaleCount.Content = MyColony.Females;
            }

            private void PupaeLost_Click(object sender, RoutedEventArgs e)
            {
                MyColony.AntList.Remove();
                MyColony.LarvaeCalc(MyColony.LarvaeCounter);
                PupaeCount.Content = MyColony.Pupae;
            }

            private void LarvaeLost_Click(object sender, RoutedEventArgs e)
            {
                MyColony.LarvaeLoss(MyColony.LarvaeCounter);
                MyColony.LarvaeCalc(MyColony.LarvaeCounter);
                LarvaeCount.Content = MyColony.Larvae;
            }

            private void EggLost_Click(object sender, RoutedEventArgs e)
            {
                MyColony.EggLoss(MyColony.LarvaeCounter);
                MyColony.LarvaeCalc(MyColony.LarvaeCounter);
                EggCount.Content = MyColony.Eggs;

            }

            private void FoodLost_Click(object sender, RoutedEventArgs e)
            {
                MyColony.Food -= 30;
                FoodCount.Content = MyColony.Food;

            }

            private void NestSizeLost_Click(object sender, RoutedEventArgs e)
            {
                if (MyColony.NestSize > 10)
                {
                    MyColony.NestSize -= 5;
                    NestSizeCount.Content = MyColony.NestSize;
                }
                else
                {
                    MyColony.NestSize = 6;
                    MessageBox.Show("The nest size cannot go below 6 cubic centimeters.");
                }

            }
        */
                
    
        private void MyColony_RemoveAnt(object sender, Ant e)
        {
            Colony.AntsToBeRemoved.Add(e);
        }
        private void MyColony_AddAnt(object sender, Ant e)
        {
            Colony.AntsToBeAdded.Add(e);
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MyColony.EggRate = int.Parse(EggRateTextBox.Text);
        }
    }

    public class Colony
    {
        public int ForageTotal = 0;
        public int NurseTotal = 0;
        private float nurseCounter = 0;
        private int eggRate;
        private int majors;
        private int minors;
        private int males;
        private int females;
        private int food;
        private int nestSize;
        private int eggs;
        private int larvae;
        private int pupae;

        public int EggRate
        {
            get { return eggRate; }
            set
            {
                if (value >= 0 && value < 5)
                    eggRate = value;
                else
                    MessageBox.Show("Egg-laying rate must be an integer between zero and five (inclusive).");
            }
        }
        public int Day = 0;
        public int Majors { get { return majors; } set { majors = value; } }
        public int Minors { get { return minors; } set { minors = value; } }
        public int Males { get { return males; } set { males = value; } }
        public int Females { get { return females; } set { females = value; } }
        public int Food { get { return food; } set { food = value; } }
        public int NestSize { get { return nestSize; } set { nestSize = value; } }
        public int Eggs { get { return eggs; } set { eggs = value; } }
        public int Larvae { get { return larvae; } set { larvae = value; } }
        public int Pupae { get { return pupae; } set { pupae = value; } }

        //public int[] LarvaeArray = new int[maxLarvae];
        internal List<Ant> AntList = new List<Ant>();
        internal static List<Ant> AntsToBeRemoved = new List<Ant>();
        internal static List<Ant> AntsToBeAdded = new List<Ant>();
        internal List<Ant> WorkerList = new List<Ant>();
        internal List<Ant> BroodList = new List<Ant>();

        public int AntCounter = 0;
        public int CurrentAnt = 0;


        public Colony()
        {
            eggRate = 1;
            Majors = 0;
            Minors = 0;
            Males = 0;
            Females = 0;
            Food = 200;
            NestSize = 30;
            Eggs = 0;
            Larvae = 0;
            Pupae = 0;
            
        }

        public static int DiceRoll(int i, int j)
        {
            Random dice = new Random();
            int result = dice.Next(i, j);
            return result;
        }

        internal void AntCalc()
        {
            foreach (Ant ant in AntList)
            {
                if (ant.GetType().ToString() == "AntColonySimulation.Egg")
                    eggs++;
                else if (ant.GetType().ToString() == "AntColonySimulation.Larva")
                    larvae++;
                else if (ant.GetType().ToString() == "AntColonySimulation.Pupa")
                    pupae++;
                else if (ant.GetType().ToString() == "AntColonySimulation.MinorWorker")
                    minors++;
                else if (ant.GetType().ToString() == "AntColonySimulation.MajorWorker")
                    majors++;
                else if (ant.GetType().ToString() == "AntColonySimulation.Male")
                    males++;
                else if (ant.GetType().ToString() == "AntColonySimulation.Female")
                    females++;
            }
        }

        public void FoodCalc(int totalfoodcollected)
        {
            Food += totalfoodcollected -
                   (2 * eggRate + Majors * 2 + Minors + Males * 2 + Females * 3 + Larvae);
        }

        public void CreateWorkerList(List<Ant> AntList)
        {
            Ant doAnyWorkersExist = AntList.Find((Ant ant) => { return ant is WorkerAnt; });
            if (doAnyWorkersExist != null)
            {
                WorkerList = AntList.FindAll((Ant ant) => { return ant is WorkerAnt; });
            }
        }

        public void CreateBroodList(List<Ant> AntList)
        {
            Ant doAnyBroodExist = AntList.Find((Ant ant) => { return ant is Brood; });
            if (doAnyBroodExist != null)
            {
                BroodList = AntList.FindAll((Ant ant) => { return ant is Brood; });
            }
        }


        public void EggBirth()
        {
            for (int i = 0; i < eggRate; i++)
            {
                AntList.Add(new Egg(AntCounter));
                AntCounter++;
            }
        }

        public void IncrementAge()
        {
            foreach (Ant ant in AntList)
            {
                ant.Age++;
            }
        }

        public void BroodMetamorphosis()
        {
            foreach (Brood brood in BroodList)
            {
                if (brood.Age == brood.MetamorphosisAge)
                {

                    RemoveAnt(this, brood); //prepare to remove ant object that has already metamorphosized into something else
                    //by adding it to a remove list.
                    AddAnt(this, brood.Metamorphosis(AntCounter));
                    AntCounter++; //increment AntCounter for next ant created.
                }
            }

            foreach (Ant a in AntsToBeRemoved)
            {
                AntList.Remove(a);
            }
            foreach (Ant a in AntsToBeAdded)
            {
                AntList.Add(a);
            }
            AntsToBeRemoved.Clear();
            AntsToBeAdded.Clear();
        }


        public event EventHandler<Ant> RemoveAnt;
        public event EventHandler<Ant> AddAnt;

        public int Nurse(float nurses)
        {
            nurseCounter = nurses * 20 - Larvae * 2 - Pupae / 2 - Eggs;
            //if nurse care is not very good, small chance of larvae dying
            if (nurseCounter < 0)
            {
                for (int i = 0; i < BroodList.Count - 1; i++)
                {
                    
                    if (BroodList[i].Age > 5 && BroodList[i].Age <= 17)
                    {
                        int result = DiceRoll(1, 11);
                        if (result == 1)
                            RemoveAnt(this, BroodList[i]);
                        else if (result == 2)
                            BroodList[i].status = state.sick;

                    }
                        
                }
                return 0;

            }
            else if (nurseCounter > 3 * Larvae + Eggs + Pupae)
            {
                for (int i = 0; i < BroodList.Count - 1; i++)
                {
                    if (BroodList[i].Age > 5 && BroodList[i].Age <= 17)
                        BroodList[i].Age++;
                }
                return 2;
            }

            else
                return 1;
        }

        public void NextWorker()
        {

            if (CurrentAnt < WorkerList.Count() - 1)
            {
                CurrentAnt++;
            }
            else
                CurrentAnt = 0;

        }


        internal void AntCalcClear()
        {
            majors = 0;
            minors = 0;
            males = 0;
            females = 0;
            eggs = 0;
            larvae = 0;
            pupae = 0;
        }



        /*
        public void LarvaeDevelopment(int c)
        {
            for (int i = 0; i < c; i++)
            {
                if (LarvaeArray[i] > 0)
                    LarvaeArray[i]++;
            }
        }

        public void LarvaeCalc(int c)
        {
            int e = 0, l = 0, p = 0;
            for (int i = 0; i < c; i++)
            {
                if (LarvaeArray[i] > 0 && LarvaeArray[i] <= 5)
                    e++;
                else if (LarvaeArray[i] > 5 && LarvaeArray[i] <= 17)
                    l++;
                else if (LarvaeArray[i] > 17 && LarvaeArray[i] <= 24)
                    p++;
                else if (LarvaeArray[i] >= 25)
                {
                    BirthCalc();
                    LarvaeArray[i] = 0;
                }
                Eggs = e; Larvae = l; Pupae = p;
            }
        }
        
        public void PupaeLoss(int c)
        {
            int i = 0;
            while (!(LarvaeArray[i] > 17 && LarvaeArray[i] <= 24) && i <= c)
            {
                i++;
            }
            LarvaeArray[i] = 0;                
        }

        public void LarvaeLoss(int c)
        {
            int i = 0;
            while (!(LarvaeArray[i] > 5 && LarvaeArray[i] <= 17) && i <= c)
            {
                i++;
            }
            LarvaeArray[i] = 0;
        }

        public void EggLoss(int c)
        {
            int i = 0;
            while (!(LarvaeArray[i] <= 5 && LarvaeArray[i] > 0) && i <= c)
            {
                i++;
            }
            LarvaeArray[i] = 0;
        }

   
        public void BirthCalc()
        {
            if (DateTime.Now.Millisecond <= 800)
            {
                Minors++;
            }
            else if (DateTime.Now.Millisecond > 930 && (Food > Larvae * 4) && Day > 45)
            {
                Males++;
            }
            else if (DateTime.Now.Millisecond > 900 && (Food > Larvae * 4) && Day > 45)
            {
                Females++;
            }
            else if (DateTime.Now.Millisecond > 800 && (Food > Larvae * 2))
            {
                Majors++;
            }

        }
        */
       
    }
        

        public enum state { healthy, sick, starving };

        public abstract class Ant
        {

            private int number;

            public int Number
            {
                get { return number; }
                set { number = value; }
            }
            protected double weightmg;
            private int age;
            private int foodConsumption;
            public state status;
            public int Age { get { return age; } set { if (value >= 0) age = value; } }
            public int FoodConsumption { get { return foodConsumption; } set { if (value >= 0) foodConsumption = value; } }

        public Ant(int _antCounter, double weightmgValue, int ageValue, int foodConsumptionValue, state statusValue)
        {
            number = _antCounter;
        }
            public abstract int Forage();
            public abstract int Dig();
            public abstract int Nurse();
            public abstract int Guard();

        
            public override string ToString()
            {
            string toTrimString = "AntColonySimulation.";
            char[] toTrim = toTrimString.ToCharArray();
            string specs = "Type: " + this.GetType().ToString().TrimStart(toTrim) + "\nNumber: " + Number + "\nAge: " + Age + "\nFood Consumption: "
                    + foodConsumption + "\nweight(in mg): " + weightmg + "\nStatus: " + status.ToString();
                return specs;
            }

            public static int DiceRoll(int i, int j)
            {
                Random dice = new Random();
                int result = dice.Next(i, j);
                return result;
            }
        }

        public abstract class Brood : Ant
        {
            public Brood(int _antCounter, int metamorphosisAgeValue, double weightmgValue, int ageValue, int foodConsumptionValue, state statusValue)
                : base(_antCounter, weightmgValue, ageValue, foodConsumptionValue, statusValue)
            {
                Number = _antCounter;
                weightmg = weightmgValue;
                Age = ageValue;
                FoodConsumption = foodConsumptionValue;
                status = statusValue;
                metamorphosisAge = metamorphosisAgeValue;
            }
            private int metamorphosisAge;
            public int MetamorphosisAge { get { return metamorphosisAge; } set { metamorphosisAge = value; } }
            public abstract Ant Metamorphosis(int _antCounter);

        }



        public class Egg : Brood
        {
            public Egg(int _antCounter) : base(_antCounter, 12, 0.2, 0, 0, 0)
            {
                Number = _antCounter;
            }

            public override int Forage() { return 0; }
            public override int Dig() { return 0; }
            public override int Nurse() { return 0; }
            public override int Guard() { return 0; }

            public override Ant Metamorphosis(int _antCounter)
            {
                return new Larva(_antCounter);

            }


        }

        public class Larva : Brood
        {
            public Larva(int _antCounter) : base(_antCounter, 36, 2, 6, 1, 0)
            {
                Number = _antCounter;
            }

            public override int Forage() { return 0; }
            public override int Dig() { return 0; }
            public override int Nurse() { return 0; }
            public override int Guard() { return 0; }

            public override Ant Metamorphosis(int _antCounter)
            {
                return new Pupa(_antCounter);
            }


        }

        public class Pupa : Brood
        {
            public Pupa(int _antCounter) : base(_antCounter, 50, 4, 18, 0, 0)
            {
                Number = _antCounter;
            }

            public override int Forage() { return 0; }
            public override int Dig() { return 0; }
            public override int Nurse() { return 0; }
            public override int Guard() { return 0; }

            public override Ant Metamorphosis(int _antCounter)
            {
                if (DiceRoll(1, 11) >= 8)
                    return new MajorWorker(_antCounter);
                else
                    return new MinorWorker(_antCounter);
            }

        }

        public abstract class WorkerAnt : Ant
        {
            public WorkerAnt(int _antCounter, double weightmgValue, int ageValue, int foodConsumptionValue, state statusValue)
                : base(_antCounter, weightmgValue, ageValue, foodConsumptionValue, statusValue)
            {
                weightmg = weightmgValue;
                Age = ageValue;
                FoodConsumption = foodConsumptionValue;
                status = statusValue;
            }


        }

        public class MinorWorker : WorkerAnt
        {

            public MinorWorker(int _antCounter) : base(_antCounter, 4, 25, 1, 0)
            {
            }

            public override int Forage()
            {
                return DiceRoll(1, 80);
            }

            public override int Dig()
            {
                return DiceRoll(1, 3);
            }

            public override int Nurse()
            {
                return DiceRoll(1, 3);
            }

            public override int Guard()
            {
                return DiceRoll(0, 10);
            }
        }

        public class MajorWorker : WorkerAnt
        {
            public MajorWorker(int _antCounter) : base(_antCounter, 10, 25, 2, 0)
            {
            }

            public override int Forage()
            {
                return DiceRoll(20, 100);
            }

            public override int Dig()
            {
                return DiceRoll(1, 5);
            }

            public override int Nurse()
            {
                return DiceRoll(0, 2);
            }

            public override int Guard()
            {
                return DiceRoll(5, 15);
            }
        }
    
    

}
    
 
        
    





