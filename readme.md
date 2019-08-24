# Combinary Stream
Combines data from the collectors into a consumable microservice.

## Usage

example `docker-compose.yml`:
```yml
version: "2.2"
services:
  stream:
    image: quay.io/0xff/combinary-stream:master
    ports:
      - "8188:8080"
    environment:
      # Youtube Parameters
      YoutubeConnectionString: "Host=youtube_db;Database=db;Username=user;Password=🔑"
      YoutubeLimit": 50

      # Twitter Parameters
      TwitterConnectionString: "Host=twitter_db;Database=db;Username=user;Password=🔑"
      TwitterLimit": 50

      # RSS Parameters
      RssFeedUrl: "https://feed-url.xml"
      RssFeedLimit: 50

      # Cache Settings ( 0 = Disabled )
      CacheTtl: 1800
      AutoRefreshCache: "true"
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
      }
]
```