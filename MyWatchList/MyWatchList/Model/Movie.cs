﻿using Factory.Factory;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverDesignPatterns.Model
{
    [FirestoreData(ConverterType = typeof(Converter<Movie>))]
    public class Movie : Watchable
    {

        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string ImageUrl { get; set; }
        [FirestoreProperty]
        public string Category { get; set; }

        public Movie()
        {

        }

        public Movie(string name, string description, string imageUrl, string category)
        {
            this.watchable(this.Name, this.Description, this.ImageUrl, this.Category);
        }

        public void watchable(string name, string description, string imageUrl, string category)
        {
            this.Name = name;
            this.Description = description;
            this.ImageUrl = imageUrl;
            this.Category = category;
        }
    }
}
