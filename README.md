# Simple Mock Web Service #

**Simple Mock Web Service (SimpleMock)** provides RESTful web services, for mocking purpose, to be consumed in any JavaScript frameworks using AJAX call.


# Background #

**SimpleMock** is originally inspired by [node EasyMock](https://github.com/CyberAgent/node-easymock) managed by [Patrick Boos](https://github.com/pboos), which provides a web server, using `node.js`, for mocking web services. Instead of using `node.js`, **SimpleMock** does its features through ASP.NET Web API.


# System Requirements #

In order to run **SimpleMock**, it requires:

* .NET Framework 4.5 or higher
* IIS 7.0 or higher
* [URL Rewrite module](http://www.iis.net/downloads/microsoft/url-rewrite)

*NOTE* : **SimpleMock** can be run on IIS 6 with a 3rd party URL rewriting module like [Ionics Isapi Rewrite Filter(IIRF)](http://iirf.codeplex.com), [Helicon URL Rewrite](http://www.isapirewrite.com) or [UrlRewritingNet.UrlRewrite](http://www.urlrewriting.net), as long as .NET Framework 4.5 or higher is installed and run on the server machine.


# Getting Started #

Once your IIS has been setup with a URL of `simplemock.local`, for example, just simply run one of the following URLs:

    GET		http://simplemock.local/api/contents
    GET		http://simplemock.local/api/content/1

Then you will be able to see the JSON formatted response. Those requests are basically using the `GET` method. If you are using [Fiddler](http://fiddler2.com) or something like that, you can also run the following requests:

    POST	http://simplemock.local/api/content
    PUT		http://simplemock.local/api/content/1
    DELETE	http://simplemock.local/api/content/1

Then you will be also able to confirm the JSON response within Fiddler.


# Configuration #

If you want to add your custom RESful URLs, you can simply modify the `SimpleMockWebService.config` file.

    <?xml version="1.0" encoding="utf-8" ?>
    <simpleMockWebService>
      <globalSettings webApiPrefix="api" verbs="GET,POST,PUT,DELETE" jsonFileExtensions="json,js,txt" />
      <apiGroups>
        <apiGroup key="Content">
          <apis>
            <api key="GetContents" group="Content" method="get" url="/api/contents" src="~/responses/get.contents.json" />
            <api key="GetContentById" method="get" url="/api/content/1" src="get.content.1.json" delay="0" />
            <api key="PostContent" method="post" url="/api/content" delay="0" />
            <api key="PutContentById" method="put" url="/api/content/1" src="/responses/put.content.1.json" delay="0" />
            <api key="DeleteContentById" method="delete" url="/api/content/1" src="~/responses/delete.content.1.json" />
          </apis>
        </apiGroup>
      </apiGroups>
    </simpleMockWebService>


## Global Settings Element ##

* `webApiPrefix`: is the first part of the URL segments after the domain name. Default value is `api`.
* `verbs`: is the list of valid method verbs consumed in this service. Default methods are `GET`, `POST`, `PUT` and `DELETE`. You can add `HEAD` or other verbs on your preference.
* `jsonFileExtensions`: is the list of valid file extensions storing JSON formatted responses. Default file extensions are `json`, `js`, and `txt`.


## API Group Element ##

* `key`: is the unique identifier of the API group element. This key is considered as the second part of the URL segments.


## API Element ##

* `key`: is the unique identifier of the API element.
* `group`: points to the API group element key, if the second part of the URL segments is different from the key of its parent group element.
* `method`: represents the method verb.
* `url`: represents the URL.
* `src`: points to the physical location where JSON formatted response is situated. Default location of each response file is `~/responses`.
* `delay`: simulates the server response time in milliseconds.


# RESTful Web Services #

Each method returns JSON response that is predefined and stored in the specified location on the server.

## GET ##

    <api key="GetContents" group="Content" method="get" url="/api/contents" src="~/responses/get.contents.json" />
    <api key="GetContentById" method="get" url="/api/content/1" src="get.content.1.json" delay="0" />

The configurations above expect the `GET` requests below respectively:

    http://simplemock.local/api/contents
    http://simplemock.local/api/content/1

Therefore, each request returns JSON response respectively like:

    [{ "id": 1, "title": "title 1" }, { "id": 2, "title": "title 2" }, { "id": 3, "title": "title 3" }]
    { "id": 1, "title": "title 1" }


## POST ##

    <api key="PostContent" method="post" url="/api/content" delay="0" />

The configuration above expects the `POST` requests below:

    http://simplemock.local/api/content

Therefore, the request returns the JSON response like:

    { "id": 1, "title": "title 1" }


## PUT ##

    <api key="PutContentById" method="put" url="/api/content/1" src="/responses/put.content.1.json" delay="0" />

The configuration above expects the `PUT` requests below:

    http://simplemock.local/api/content/1

Therefore, the request returns the JSON response like:

    { "id": 1, "title": "title 1" }

## DELETE ##

    <api key="DeleteContentById" method="delete" url="/api/content/1" src="~/responses/delete.content.1.json" />

The configuration above expects the `DELETE` requests below:

    http://simplemock.local/api/content/1

Therefore, the request returns the JSON response like:

    { "id": 1, "title": "title 1" }

## Other Verbs ##

Depending on your preferences, you can predefine any acceptable RESTful method and store its desirable response on the server and use it.


# License #

**SimpleMock** is released under [MIT License](http://opensource.org/licenses/MIT).

> The MIT License (MIT)
> 
> Copyright (c) 2013 [aliencube.org](http://aliencube.org)
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
> furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
