using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stablemarriage.core
{
    public class ListReader
    {
        private Dictionary<Person, Person> population = null;

        public IEnumerable<Person> GetPopulation(string filename)
        {
            population = new Dictionary<Person, Person>(new PersonComparer());

            var masterFile = System.IO.File.OpenText(filename);

            string subFilename;
            while ((subFilename = masterFile.ReadLine()) != null)
            {
                ReadList(subFilename);
            }

            return population.Keys;
        }

        private void ReadList(string subFilename)
        {
            var subFile = System.IO.File.OpenText(subFilename);
            string name = subFile.ReadLine();
            string[] nameSplit = name.Split(' ');
            string firstName = "";
            string lastName = "";
            Gender gender = Gender.Male;

            if (nameSplit.Length >= 1)
            {
                firstName = nameSplit[0];
            }
            if (nameSplit.Length >= 2)
            {
                lastName = nameSplit[1];
            }
            if (nameSplit.Length >= 3)
            {
                string stringGender = nameSplit[2];
                Enum.TryParse<Gender>(stringGender, out gender);
            }

            if (nameSplit.Length < 3)
            {
                return;
            }

            Person tempPerson = new Person(firstName, lastName, gender);
            Person realPerson = null;
            if (population.ContainsKey(tempPerson))
            {
                realPerson = population[tempPerson];
            }
            else
            {
                realPerson = tempPerson;
                population[realPerson] = realPerson;
            }

            tempPerson = null;
            // if it is a man, assume that he wants a woman
            // else it must be a woman, assume that she wants a man
            Gender wantedGender = gender == Gender.Male ? Gender.Female : Gender.Male;

            string blankLine = subFile.ReadLine();
            if (blankLine.Trim().StartsWith("-"))
            {
                string potentialPartner;
                while ((potentialPartner = subFile.ReadLine()) != null)
                {
                    string[] potentialPartnerSplit = potentialPartner.Split(' ');
                    string potentialFirstName = "";
                    string potentialLastName = "";
                    if (potentialPartnerSplit.Length >= 1)
                    {
                        potentialFirstName = potentialPartnerSplit[0];
                    }
                    if (potentialPartnerSplit.Length >= 2)
                    {
                        potentialLastName = potentialPartnerSplit[1];
                    }

                    if (potentialPartnerSplit.Length < 2)
                    {
                        continue;
                    }

                    tempPerson = new Person(potentialFirstName, potentialLastName, wantedGender);
                    Person potentialPerson = null;
                    if (population.ContainsKey(tempPerson))
                    {
                        potentialPerson = population[tempPerson];
                    }
                    else
                    {
                        potentialPerson = tempPerson;
                        population[potentialPerson] = potentialPerson;
                    }
                    tempPerson = null;

                    realPerson.AddToList(potentialPerson);
                }
            }
        }
    }
}
