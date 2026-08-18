using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Helper
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> ItemsList, int pageNumber, int count, int pageSize, int startFrom, int endTo)
        {
            CurrentPage = pageNumber;

            //calculate this depend on how many cout we have 
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            StartFrom = startFrom;
            EndTo = endTo;


            //we will add items inside here so that we have access to these items inside our page list
            AddRange(ItemsList);
        }

        // the number of the current page
        public int CurrentPage { get; set; }

        //the total number of pages
        public int TotalPages { get; set; }

        //the count of items per each page
        public int PageSize = 10;

        //total count of items in the query
        public int TotalCount { get; set; }
        public int StartFrom { get; set; }
        public int EndTo { get; set; }

        //creating static method that we can call from anywhere
        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            //calculate the count from query that we have
            var count = source.Count();

            var startFrom = ((pageNumber - 1) * pageSize) + 1;

            //calculate what we will skip and what will take
            //l7ad delwa2ty mro7nash eldatabase lma n3ml ToList byro7
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var endTo = startFrom + items.Count - 1;

            //creating and returning object of PageList with constructor
            return new PagedList<T>(items, pageNumber, count, pageSize, startFrom, endTo);
        }

        public static PagedList<T> CreateOrdered(IOrderedQueryable<T> source, int pageNumber, int pageSize)
        {
            //calculate the count from query that we have
            var count = source.Count();

            var startFrom = ((pageNumber - 1) * pageSize) + 1;

            //calculate what we will skip and what will take
            //l7ad delwa2ty mro7nash eldatabase lma n3ml ToList byro7
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var endTo = startFrom + items.Count - 1;

            //creating and returning object of PageList with constructor
            return new PagedList<T>(items, pageNumber, count, pageSize, startFrom, endTo);
        }
        public async static Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            //calculate the count from query that we have
            var count = source.Count();

            var startFrom = ((pageNumber - 1) * pageSize) + 1;

            //calculate what we will skip and what will take
            //l7ad delwa2ty mro7nash eldatabase lma n3ml ToList byro7
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var endTo = startFrom + items.Count - 1;

            //creating and returning object of PageList with constructor
            return new PagedList<T>(items, pageNumber, count, pageSize, startFrom, endTo);
        }
    }
}
