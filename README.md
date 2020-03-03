#                         							Blog Rest Api



## Additional Status

* N.B. **This status will be in response body** 

| Status Code | Meaning            |
| ----------- | ------------------ |
| 0           | Entity Added       |
| 1           | Failed             |
| 2           | Entity NotFound    |
| 3           | Entity NotModified |
| 4           | Entity Modified    |
| 5           | Entity NotDeleted  |
| 6           | Entity Deleted     |
| 7           | Forbidden          |
| 8           | Entity Taken       |

 





## Auth

#### POST `/v1/Auth/Register`

*Request Body*

```
{
    userId*					string
                            maxLength: 100
                            minLength: 5
                            pattern: ^[a-zA-Z0-9]*$
                            nullable: true
    
    firstName*				string
                            maxLength: 100
                            minLength: 5
                            pattern: ^[a-zA-Z]*$
                            nullable: true
    
    lastName				string
                            maxLength: 100
                            pattern: ^[a-zA-Z]*$
                            nullable: true
    
    password*				string
    						min alphabet: 2
    						min numeric: 1
    						min length: 5
    						nullable: true
}
```

*Response Status*

```
200 OK
400 Bad Request
```

##### Example

*Request Body*

```json
{
  "userId": "akaash",
  "firstName": "uttom",
  "lastName": "akash",
  "password": "secret7"
}
```



*Response Body* (200 Ok)

```json
{
  "status": 0,
  "message": "Added"
}
```





#### POST `/v1/Auth/Login`

*Request Body*

```
{
    userId*		string
    password*	string
}
```

*Response Status*

```
200 OK
400 Bad Request 
```

##### Example

*Request Body*

```json
{
  "userId": "akaash",
  "password": "secret7"
}
```



*Response Body* (200 Ok)

```json
{
    "jwtToken": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiJha2FzaCIsIkRldiI6ImFrYXNoIiwiZXhwIjoxNTgzMjI5NDYwLCJpc3MiOiJjZWZhbG8uY29tIiwiYXVkIjoiY2VmYWxvIn0.daLH7Lwe4GeMBDRsZPTf7M5OH2Cbd0GUiPFPjbq88sk",
    "userId": "akash",
    "firstName": "string",
    "lastName": "string"
}
```







## Users

#### **GET** `/v1/User/user/{userId}`

*Response Status*

```
200 OK
404 Not Found
400 Bad Request
```

*Response Body* (200 Ok)

```json
{
    "userId": "stringakash",
    "firstName": "string",
    "lastName": "string"
}
```





#### **GET** `/v1/User/users/{skip}/{top}`

* N.B. Here **skip** and **top** optional
* skip=0 (*default value*)
* top=50 (*default value*)

*Response Status*

```
200 OK
```

*Response Body* (200 Ok)

```json
[
    {
        "userId": "strin11g",
        "firstName": "string",
        "lastName": "string"
    },
    {
        "userId": "string",
        "firstName": "string",
        "lastName": "string"
    }
]
```





#### **PUT** `/v1/User/user`

*Request Header*

```
Authorization: Bearer <Token>
```



*Request Body* 

```
{
    oldPassword			string
    					nullable: true
    
    newPassword			string
    					nullable: true
}
```

*Response Status*

```
404 Not Found
403 Forbidden
400 Bad Request
200 OK
```



##### Example

*Request Body*

```json
{
  "oldPassword": "secret7",
  "newPassword": "secret78"
}
```

*Response Body* (200 Ok)

```json
{
    "status": 4,
    "message": "Modified"
}
```



## Stories

#### POST  `/v1/Stories/story`

*Request Header*

```
Authorization: Bearer <Token>
```



*Request Body*

```
{
	storyId				string($uuid)
	
	title*				string
						maxLength: 250
						minLength: 10
						nullable: true
	
	body*				string
                        minLength: 100
                        nullable: true
	
	publishedDate*		string ($date)
}
```

*Request Status*

```
201 Created
400 Bad Request
```



##### Example

*Request Body*

```json
{
	"title": "Lorem Ipsum",
  	"body": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
   "publishedDate": "2020-02-28"
}
```

*Response Header*

```}
Location : http://localhost:5000/v1/Stories/story/{storyId}
```





#### GET  `/v1/Stories/story/{storyId}`

*Response Status*

```
200 OK
404 Not Found
400 Bad Request
```



*Response Body* (200 Ok)

```json
{
    "storyId": "3fa85f64-5717-4562-b3fc-2c963f66ac12",
    "title": "Lorem Ipsum",
    "body": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
    "publishedDate": "2020-02-28T00:00:00",
    "author": {
        "userId": "akash",
        "firstName": "string",
        "lastName": "string"
    }
}
```





#### **GET**	`/v1/Stories/stories/{skip}/{top}`

* N.B. Here **skip** and **top** optional
* skip=0 (*default value*)
* top=50 (*default value*)

*Response Status*

```
200 OK
```

*Response Body* (200 Ok)

```json
[
  {
    "storyId": "2e529430-5dbd-48e2-41ea-08d7bf382374",
    "title": "Lorem Ipsum",
    "body": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
    "publishedDate": "2020-02-28T00:00:00",
    "author": {
      "userId": "akash",
      "firstName": "string",
      "lastName": "string"
    }
  }
]
```





#### PUT `/v1/Stories/story`

*Request Header*

```
Authorization: Bearer <Token>
```



*Request Body*

```
{
	storyId*			string($uuid)
	
	title*				string
                        maxLength: 250
                        minLength: 10
                        nullable: true

    body*				string
                        minLength: 100
                        nullable: true

	publishedDate*		string($date)
}
```
*Response Status*

```
404 Not Found
403 Forbidden
400 Bad Request
200 OK
```



##### Example

*Request Body*

```json
{
    "storyId": "3fa85f64-5717-4562-b3fc-2c963f66ac12",
    "title": "Lorem Ipsum",
    "body": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
    "publishedDate": "2020-02-28T00:00:00"
}
```



*Response Body* (200 Ok)

```json
{
    "status": 4,
    "message": "Modified"
}
```





#### **DELETE**	`/v1/Stories/story/{storyId}`

*Request Header*

```
Authorization: Bearer <Token>
```



*Response Status*

```
404 Not Found
403 Forbidden
400 Bad Request
200 OK
```

*Response Body* (200 Ok)

```json
{
    "status": 6,
    "message": "Deleted"
}
```






## Error Response Format

#### 500 Internal Server Error

```json
{
    "status": 500,
    "message": "",
    "source": "Core Microsoft SqlClient Data Provider"
}
```



#### 400 Bad Request 

```json
{
    "type": "",
    "title": "",
    "status": 400,
    "traceId": "",
    "errors": {
        "field": [
            "message"
        ]
    }
}
```



#### 404 Not Found

```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
    "title": "Not Found",
    "status": 404,
    "traceId": "|d0d22e0c-4085cb953271417c."
}
```

#### 403Forbidden

```

```

