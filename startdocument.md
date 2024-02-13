# WatchList Application

## Introduction

This document serves as a start document and offers an overview for the practical assignment for the "Design Patterns" course. The purpose of this course is to teach students how to understand and recognize various design patterns. Design patterns are a toolkit of solutions to common problems in software design. It is mainly used for achieving better design and maintainability.



## Application description

The student group tries to create a Desktop Application which helps users to keep track of the shows they are watching or wants to watch. Furthermore, the application allows the user to add shows, update and delete the shows they added. A database is going to be used to store the shows. This will be achieved, by using C# WinUI and the database will be MSSQL.



## Features

- Adding shows

- Editing shows

- Displaying a list of shows

- Deleting shows

  

### MoSCoW Analysis

| Priority    | Task                                        |
| ----------- | ------------------------------------------- |
| Must have   | Adding, Editing, Displaying, Deleting Shows |
| Should have | Categories                                  |
| Could have  | Watch Flair, Exporting List                 |
| Wont have   | User Registration                           |

## Technology & Patterns Applied

As part of the course and in order to implement some of the features, the project will use a variety of threading techniques. These include the following:

- GitHub

- C# winUI

- MVC

## Patterns used

- Factory  - Is used to instantiate shows and categories. In the future it can also be used to instantiate new objects. The Factory Pattern is a creational design pattern that provides an interface for making objects in a superclass.
- Command - For handling our database requests. An interface was used to divide the requests that are made to the database. Command is a behavioral design pattern that gives request to an alone object that gives all the information about the request
- Observer - Is used when we want to fully delete a show, we notify all subscribers(categories). The categories are going to check if that show exists within their category, if so it will be removed the list. Observer is a behavioral design pattern that is responsible for notifying it's subscribers when an event happens.



<h3>Definition of Done </h3>

The project is done, when the following requirements are met:

- The Start Document contains all the required fields and features
- The application works as described
- The application has 3 design patterns implemented
- All the must have functionalities have been implemented
- A manual [ReadMe] file has been created to assist in the installation of the application



### Class Diagram

![img](https://gyazo.com/e454dc4db4217de6970625411fbf6c03.png)
https://gyazo.com/e454dc4db4217de6970625411fbf6c03





### 



### About the developers

This application was made by the following students:

| Name               | Student email                                                |
| ------------------ | ------------------------------------------------------------ |
| Teodor Folea       | [teodor.folea@student.nhlstenden.com](mailto:teodor.folea@student.nhlstenden.com) |
| Arian Atapour      | [arian.atapour@student.nhlstenden.com](mailto:arian.atapour@student.nhlstenden.com) |
| Dimitri Vastenhout | [dimitri.vastenhout@student.nhlstenden.com](mailto:dimitri.vastenhout@student.nhlstenden.com) |
