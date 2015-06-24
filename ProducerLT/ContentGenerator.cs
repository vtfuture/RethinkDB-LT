using GenFu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerLT
{
    public class ContentGenerator
    {
        public string GetTwitter()
        {
            return (new TwitterFiller()).GetValue().ToString() + Guid.NewGuid().ToString();
        }

        public string GetArticle()
        {
            return (new ArticleTitleFiller()).GetValue().ToString() + Guid.NewGuid().ToString();
        }

        public string GetEmail()
        {
            return (new EmailFiller()).GetValue().ToString() + Guid.NewGuid().ToString();
        }

        public string GetAddress()
        {
            return (new AddressFiller()).GetValue().ToString() + Guid.NewGuid().ToString();
        }

        public string GetPhone()
        {
            return (new PhoneNumberFiller()).GetValue().ToString() + Guid.NewGuid().ToString();
        }

        public DateTime GetDate()
        {
            return DateTime.Now;
        }

        public string GetName()
        {
            return (new FirstNameFiller()).GetValue().ToString() + " " + (new LastNameFiller()).GetValue().ToString() + Guid.NewGuid().ToString();
        }
    }
}
