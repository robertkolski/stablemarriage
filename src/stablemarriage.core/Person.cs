using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stablemarriage.core
{
    public class Person
    {
        public Person(string firstName, string lastName, Gender gender)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.gender = gender;
            this.preferenceList = new SortedList<int, Person>();
        }

        public delegate void GetBackInLine(Person p, Person rejector);

        public event GetBackInLine BackInTheGame;

        private SortedList<int, Person> preferenceList;
        private Gender gender;

        private string firstName;
        private string lastName;

        private Person currentEngagement;
        private int currentEngagementPreference;

        public override string ToString()
        {
            //return string.Format("Id: {0}, First: {1}, Last: {2}, Gender: {3}", uniqueId, firstName, lastName, gender);
            return string.Format("First: {0}, Last: {1}, Gender: {2}", firstName, lastName, gender);
        }

        public string FirstName { get { return firstName; } }

        public string LastName { get { return lastName; } }

        public Gender Gender { get { return gender; } }

        public Person CurrentEngagement
        {
            get
            {
                return currentEngagement;
            }
            set
            {
                currentEngagement = value;
            }
        }

        public void AddToList(Person p)
        {
            preferenceList.Add(preferenceList.Count + 1, p);
        }

        public Person GetNumberOne()
        {
            return preferenceList.First().Value;
        }

        public void Rejected(Person p)
        {
            preferenceList.RemoveAt(0);
            this.BackInTheGame(this, p);
        }

        public bool HasAtLeastOneOption()
        {
            return preferenceList.Count > 0;
        }

        public Answer WillYouMarryMe(Person p)
        {
            if (this.gender == Gender.Female && p.gender == Gender.Male)
            {
                foreach (KeyValuePair<int, Person> option in this.preferenceList)
                {
                    if (PersonComparer.Instance.Equals(p, option.Value))
                    {
                        if (currentEngagement == null)
                        {
                            if (option.Key == this.preferenceList.Keys.First())
                            {
                                currentEngagement = p;
                                currentEngagementPreference = option.Key;
                                return Answer.Yes;
                            }
                            else
                            {
                                currentEngagement = p;
                                currentEngagementPreference = option.Key;
                                return Answer.Maybe;
                            }
                        }
                        else
                        {
                            if (option.Key == this.preferenceList.Keys.First())
                            {
                                currentEngagement.Rejected(this);
                                currentEngagement = p;
                                currentEngagementPreference = option.Key;
                                return Answer.Yes;
                            }
                            else if (currentEngagementPreference < option.Key)
                            {
                                p.Rejected(this);
                                return Answer.No;
                            }
                            else
                            {
                                currentEngagement.Rejected(this);
                                currentEngagement = p;
                                currentEngagementPreference = option.Key;
                                return Answer.Maybe;
                            }
                        }
                    }
                }
            }
            throw new InvalidOperationException("Men propose to women.");
        }
    }
}
