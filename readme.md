# Combinary Stream
Combines data from the collectors into a consumable microservice.

## Usage

example `docker-compose.yml`:
```yml
version: "2.2"
services:
  stream:
    image: quay.io/0xff/combinary-stream:master
    restart: always
    read_only: true
    mem_limit: "1g"
    cap_drop:
      - ALL
    tmpfs:
      - /tmp      
    ports:
      - "8188:8080"
    environment:
      # Youtube Parameters
      YoutubeConnectionString: "Host=youtube_db;Database=db;Username=user;Password=🔑"
      # YoutubeLimit: "50"

      # Twitter Parameters
      TwitterConnectionString: "Host=twitter_db;Database=db;Username=user;Password=🔑"
      # TwitterLimit: "50"

      # Facebook Parameters
      FacebookConnectionString: "Host=twitter_db;Database=db;Username=user;Password=🔑"
      # FacebookLimit: "50"
      # FacebookPageIds:0: '0000000000000' # optional - filter by facebook page_id
      # FacebookPageIds:1: '0000000000000'

      # RSS Parameters
      Rss:0:FeedUrl: "https://feed-0-url.xml"
      Rss:0:Limit: "50"

      # Cache Settings ( 0 = Disabled )
      CacheTtl: "1800"
      AutoRefreshCache: "true"

      # Optional
      ReadMoreText: "read more"
      DateFormat: "dd.MM.yyyy"
      BasePath: "/stream" # mount app in {BasePath} subdirectory
```

A list of items is available as json:
```sh
curl localhost:8188/items.json
```

It will look like this:
```json
[
    {
        "url": "https://twitter.com/username/status/0000000000000000000",
        "itemType": "twitter",
        "body": "tweet",
        "authorName": "Author Name (@screen_name)",
        "authorUrl": "https://twitter.com/screen_name",
        "publishedAt": "2019-08-23T19:11:31+00:00"
    },
    {
        "url": "https://www.youtube.com/watch?v=videoId",
        "itemType": "youtube",
        "title": "Title",
        "body": "Description",
        "thumbnailUrl": "https://i.ytimg.com/vi/videoId/maxresdefault.jpg",
        "authorName": "Channel Name",
        "publishedAt": "2019-08-20T19:26:53+00:00"
    },
    {
        "url": "https://example.com/article",
        "itemType": "rss",
        "title": "Title",
        "body": "Body",
        "thumbnailUrl": "https://cdn.example.com/thumbnail.jpg",
        "authorName": "Author",
        "publishedAt": "2019-08-19T10:19:28+00:00"
    }    
]
```