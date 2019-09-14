using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace TailwindTraders.Mobile
{
    public class WishlistItem
    {
        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Thumbnail")]
        public string ThumbnailImageUrl { get; set; }

        [JsonProperty("Full")]
        public string FullImageUrl { get; set; }
    }
}
