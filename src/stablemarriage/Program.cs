using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using stablemarriage.core;

namespace stablemarriage
{
    class Program
    {
        static List<Person> singleMen = new List<Person>();
        static List<Person> unmarriedWomen = new List<Person>();
        static List<Person> unmarriedMen = new List<Person>();

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                System.Console.Out.WriteLine("usage stablemarriage masterlist.txt");
                return;
            }

            ListReader listReader = new ListReader();
            List<Person> population = listReader.GetPopulation(args[0]).ToList();
            List<Person> men = population.Where(x => x.Gender == Gender.Male).ToList();
            men.ForEach(x => x.BackInTheGame += GetBackInTheGame);
            List<Person> women = population.Where(x => x.Gender == Gender.Female).ToList();
            unmarriedWomen = women.ToList();

            singleMen = men.ToList();
            while (singleMen.Count > 0)
            {
                Person currentMan = singleMen.First();
                Person favoredWoman = currentMan.GetNumberOne();
                if (favoredWoman != null)
                {
                    Answer answer = favoredWoman.WillYouMarryMe(currentMan);
                    if (answer == Answer.Yes)
                    {
                        System.Console.Out.WriteLine("{0} agreed to marry {1}, they got married, and they lived happily ever after.", favoredWoman, currentMan);
                        unmarriedWomen.Remove(favoredWoman);
                    }
                    else if (answer == Answer.Maybe)
                    {
                        System.Console.Out.WriteLine("{0} said that she would marry {1} but we aren't sure that she won't change her mind.", favoredWoman, currentMan);
                    }
                    else if (answer == Answer.No)
                    {
                        System.Console.Out.WriteLine("{0} outright rejected {1}", favoredWoman, currentMan);
                    }
                }
                else
                {
                    System.Console.Out.WriteLine("{0} ran out of options", currentMan);
                }
                singleMen.RemoveAt(0); // remove the man that took his turn
                // he might have been added to the end of the list
            }

            foreach(Person woman in unmarriedWomen.ToList())
            {
                if (woman.CurrentEngagement != null)
                {
                    System.Console.Out.WriteLine("{0} made up her mind and married {1} and they lived happily ever after.", woman, woman.CurrentEngagement);
                    unmarriedWomen.Remove(woman);
                    unmarriedMen.Remove(woman.CurrentEngagement);
                }
                else
                {
                    System.Console.Out.WriteLine("{0} remains unmarried.", woman);
                }
            }

            foreach (Person man in unmarriedMen)
            {
                System.Console.Out.WriteLine("{0} remains unmarried.", man);
            }

            System.Console.Write("Press Enter...");
            string waitForEnter = System.Console.ReadLine();
        }

        static void GetBackInTheGame(Person p, Person rejector)
        {
            singleMen.Add(p);
            System.Console.Out.WriteLine("{0} was rejected by {1}, but got back in the game", p, rejector);
        }
    }
}
