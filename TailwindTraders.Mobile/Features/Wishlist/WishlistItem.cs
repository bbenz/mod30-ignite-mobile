using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace TailwindTraders.Mobile
{
    public class WishlistItem : INotifyPropertyChanged
    {
        string description;
        [JsonProperty("Description")]
        public string Description {
            get => description;
            set
            {
                description = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(Description))
                );
            }
        }

        [JsonProperty("Thumbnail")]
        public string ThumbnailImageUrl { get; set; }

        [JsonProperty("Full")]
        public string FullImageUrl { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
