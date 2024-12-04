﻿namespace TweyesBackend.Domain.Common
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecords { get; set; }
        public PagedList(List<T> items, int totalRecords)
        {
            Items = items;
            TotalRecords = totalRecords;
        }
    }
}