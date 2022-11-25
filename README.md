### 介绍

为了实现两个json字符串之间的映射，该库会根据关系信息生成新的对象或者集合，也集成了访问三方Api的底层拦截器，帮忙实现自动授权。

#### 接口层

##### JsonPathConverter.Abstractions

#####  JsonPathConverter.JsonSource.HttpApi.Abstractions



#### 实现层

##### JsonPathConverter.ColumnMapper.NewObject

json列转换器，主要是实现生成新的json数据的功能。

##### JsonPathConverter.JsonSoure.HttpApi

json数据来源提供者，该提供者是通过访问Api的方式来获取json数据。

###### JsonPathConverter.JsonSoure.HttpApi.Token

提供HttpApi调用时获取AccessToken的功能，可以配置OAuth2.0中的（client_credentials、password、authorization_code、refresh_token、urn:ietf:params:oauth:grant-type:device_code），**如果没有配置就是调用者的Token**

``` json
{
    "tokenClient":{
        "IdsHost": "http://ids....",
        "GrantType": "client_credentials",
        "TokenRequest": {
          "Scope": "a b c",
          "ClientId": "abc",
          "ClientSecret": "abc-abc-abc"
        }
    }
}
```



### 依赖注入

```C#

serviceCollection.AddHttpApiJsonDataProviderWithToken(Configuration.GetSelection("tokenClient").Bind);
// 或者无需Token的项目
serviceCollection.AddHttpApiJsonDataProvider();

serviceCollection.AddColumnMapperNewObject();
```

### 业务层调用

```
IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

 using (var scope = serviceProvider.CreateScope())
 {
     IJsonDataProvider jsonDataProvider = scope.ServiceProvider.GetService<IJsonDataProvider>()!;
     JsonPathRoot jsonPathRoot = new JsonPathRoot("$.pages");

    jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageName", SourceJsonPath = "$.name", RootPath = jsonPathRoot.RootPath });
    jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageUrl", SourceJsonPath = "$.url", RootPath = jsonPathRoot.RootPath });
    jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageNamespace", SourceJsonPath = "$.namespace", RootPath = jsonPathRoot.RootPath });


    IJsonRequestSource requestSource = new JsonHttpApiRequestSource(new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri("https://s.alicdn.com/@xconfig/flasher_classic/manifest") });

    var apiJsonStr = await jsonDataProvider.GetJsonDataAsync(requestSource, default);

    IJsonColumnMapper jsonColumnMapper = scope.ServiceProvider.GetService<IJsonColumnMapper>()!;

    var resultJson = jsonColumnMapper.MapToCollection(apiJsonStr, jsonPathRoot);
}
```

### Json匹配学习

#### 自定义转换

``` C#
services.AddSingleton(new JsonPropertyFormatFunction
{
     FormatKey = "guid",
     FormatFunction = () => Guid.NewGuid()
});
```



#### 数据来源

```JSON
{
        "name": "azir",
        "startCreateData": "2022-10-13 00:00:00",
        "endCreateData": "2022-10-14 00:00:00",
        "hyperLink": {
                "context": "百度一下",
                "url": "https://www.baidu.com"
        },
        "department": {
                "id": "111",
                "name": "研发部"
        },
        "leaders": [{
                "name": "azirliang"
        }, {
                "name": "azir"
        }],
        "roles": [{
                "id": "2222",
                "name": "role2222"
        }],
        "phone": ["10086", "10010", "10001"],
        "tel": ["111", "112", "113", "114"],
        "define": {
                "suggest": {
                        "message": ["aaa", "bbb"]
                }
        }
}
```

#### 配置

```JSON
{
        "contacts": [{
                "phone": "$.phone",
                "tel": "$.tel"
        }],
        "id": "Guid",
        "name": "$.name",
        "createDataRange": {
                "start": "$.startCreateData",
                "end": "$.endCreateData"
        },
        "hyperLinkContext": "$.hyperLink.context",
        "hyperLinkUrl": "$.hyperLink.url",
        "departments": [{
                "id": "$.department.id",
                "name": "$.department.name"
        }],
        "leaders": ["$.leaders.name"],
        "leaderNames": [{
                "name": "$.leaders.name",
                "remark": "leader"
        }],
        "role": {
                "id": "$.roles[0].id",
                "name": "$.roles[0].name"
        },
        "phone1": "$.phone[0]",
        "phone2": "$.phone[1]",
        "suggests": ["$.define.suggest.message"],
        "suggest1": "$.define.suggest.message[0]",
        "suggest2": "$.define.suggest.message[1]",
        "suggest3": "$.define.suggest.message[2]"
}
```

#### 映射结果

```JSON
{
  "contacts": [
    {
      "phone": "10086",
      "tel": "111"
    },
    {
      "phone": "10010",
      "tel": "112"
    },
    {
      "phone": "10001",
      "tel": "113"
    },
    {
      "tel": "114"
    }
  ],
  "id": "9144b149-3769-4156-b3cc-89252b63a465",
  "name": "azir",
  "createDataRange": {
    "start": "2022-10-13 00:00:00",
    "end": "2022-10-14 00:00:00"
  },
  "hyperLinkContext": "百度一下",
  "hyperLinkUrl": "https://www.baidu.com",
  "departments": [
    {
      "id": "111",
      "name": "研发部"
    }
  ],
  "leaders": [
    "azirliang",
    "azir"
  ],
  "leaderNames": [
    {
      "name": "azirliang",
      "remark": "leader"
    },
    {
      "name": "azir",
      "remark": "leader"
    }
  ],
  "role": {
    "id": "2222",
    "name": "role2222"
  },
  "phone1": "10086",
  "phone2": "10010",
  "suggests": [
    "aaa",
    "bbb"
  ],
  "suggest1": "aaa",
  "suggest2": "bbb",
  "suggest3": null
}
```
