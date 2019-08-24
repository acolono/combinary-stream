using System;

namespace CombinaryStream.Models {
    public class StreamItem {
        public string Url { get; set; }
        public string ItemType { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ThumbnailUrl { get; set; }
        public string AuthorName { get; set; }
        public string AuthorUrl { get; set; }
        public DateTimeOffset PublishedAt { get; set; }
    }
}