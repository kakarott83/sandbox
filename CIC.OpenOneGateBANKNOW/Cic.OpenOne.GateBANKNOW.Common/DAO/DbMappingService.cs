using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    using OpenOne.Common.DAO;

    public class KeyValueLabel
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string Label { get; set; }
    }

    public abstract class MappingServiceBase : IMappingService
    {
        public Lazy<List<KeyValueLabel>> LazyElements { get; set; }

        public List<KeyValueLabel> Elements
        {
            get { return LazyElements.Value; }
        }

        public void Clear()
        {
            LazyElements = CreateLazy();
        }

        public int? GetIntValue(string key)
        {
            int idInt;
            var element = GetElement(key);
            if (element != null && int.TryParse(element.Value, out idInt))
            {
                return idInt;
            }
            return null;
        }

        public long? GetLongValue(string key)
        {
            long idLong;
            var element = GetElement(key);
            if (element != null && long.TryParse(element.Value, out idLong))
            {
                return idLong;
            }
            return null;
        }

        public string GetValue(string key)
        {
            var element = GetElement(key);

            if (element == null)
                return null;

            return element.Value;
        }
        
        public KeyValueLabel GetElement(string key)
        {
            return LazyElements.Value.FirstOrDefault(a => a.Key == key);
        }

        protected abstract Lazy<List<KeyValueLabel>> CreateLazy();
    }

    public sealed class DbMappingService<TContext> : MappingServiceBase
        where TContext : IDisposable
    {
        private readonly IContextFactory contextFactory;
        private readonly Func<TContext, IQueryable<KeyValueLabel>> getElements;

        public DbMappingService(IContextFactory contextFactory, Func<TContext, IQueryable<KeyValueLabel>> getElements)
        {
            this.contextFactory = contextFactory;
            this.getElements = getElements;

            LazyElements = CreateLazy();
        }
        
        protected override Lazy<List<KeyValueLabel>> CreateLazy()
        {
            return new Lazy<List<KeyValueLabel>>(() =>
            {
                using (var context = contextFactory.Create<TContext>())
                {
                    return getElements(context).ToList();
                }
            });
        }
    }

    public interface IMappingService
    {
        List<KeyValueLabel> Elements { get; }

        void Clear();

        int? GetIntValue(string key);

        long? GetLongValue(string key);

        string GetValue(string key);

        KeyValueLabel GetElement(string key);
    }
}
