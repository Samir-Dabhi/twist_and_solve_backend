# Twist and Solve API

## Overview
Twist and Solve is an ASP.NET-based API for managing a Rubik's Cube learning application. The project includes features for managing users, lessons, algorithms, achievements, and more. This document provides details about the project's setup, database schema, and API endpoints.

---

## Table of Contents
- [Database Schema](#database-schema)
- [Setup Instructions](#setup-instructions)
- [API Endpoints](#api-endpoints)
  - [User Endpoints](#user-endpoints)
  - [Algorithm Endpoints](#algorithm-endpoints)
  - [Achievement Endpoints](#achievement-endpoints)
  - [Other Endpoints](#other-endpoints)

---

## Database Schema
The database consists of the following tables:
- `tbl_User`
- `tbl_Algorithm`
- `tbl_Achievement`
- `tbl_Lessons`
- `tbl_User_Progress`
- `tbl_Videos`
- `tbl_Setting`
- `tbl_Feedback`

Refer to individual table creation scripts for details.

---

## Setup Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/your-repo/twist-and-solve-api.git
   ```

2. Configure the database connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Your-Database-Connection-String"
     }
   }
   ```

3. Run migrations to set up the database schema.

4. Start the API using:
   ```bash
   dotnet run
   ```

5. The API will be available at `http://localhost:5000`.

---

## API Endpoints

### User Endpoints
- **GET** `/User` - Retrieve all users.
- **GET** `/User/{id}` - Retrieve a user by ID.
- **POST** `/User` - Add a new user.
- **PUT** `/User/{id}` - Update user details.
- **DELETE** `/User/{id}` - Delete a user.

### Algorithm Endpoints
- **GET** `/Algorithm` - Retrieve all algorithms.
- **GET** `/Algorithm/{id}` - Retrieve an algorithm by ID.
- **GET** `/Algorithm/ByLesson/{lessonId}` - Retrieve algorithms by lesson ID.
- **POST** `/Algorithm` - Add a new algorithm.
- **PUT** `/Algorithm/{id}` - Update algorithm details.
- **DELETE** `/Algorithm/{id}` - Delete an algorithm.

### Achievement Endpoints
- **GET** `/Achievement` - Retrieve all achievements.
- **GET** `/Achievement/{id}` - Retrieve an achievement by ID.
- **POST** `/Achievement` - Add a new achievement.
- **PUT** `/Achievement/{id}` - Update achievement details.
- **DELETE** `/Achievement/{id}` - Delete an achievement.

### Lessons Endpoints
- **GET** `/Lesson` - Retrieve all lessons.
- **GET** `/Lesson/{id}` - Retrieve a lesson by ID.
- **POST** `/Lesson` - Add a new lesson.
- **PUT** `/Lesson/{id}` - Update lesson details.
- **DELETE** `/Lesson/{id}` - Delete a lesson.

### Video Endpoints
- **GET** `/Video` - Retrieve all videos.
- **GET** `/Video/{id}` - Retrieve a video by ID.
- **GET** `/Video/ByLesson/{lessonId}` - Retrieve videos by lesson ID.
- **POST** `/Video` - Add a new video.
- **PUT** `/Video/{id}` - Update video details.
- **DELETE** `/Video/{id}` - Delete a video.

### Feedback Endpoints
- **GET** `/Feedback` - Retrieve all feedback entries.
- **GET** `/Feedback/{id}` - Retrieve feedback by ID.
- **POST** `/Feedback` - Add feedback.
- **DELETE** `/Feedback/{id}` - Delete feedback.

### User Progress Endpoints
- **GET** `/Progress` - Retrieve all user progress entries.
- **GET** `/Progress/{id}` - Retrieve user progress by ID.
- **GET** `/Progress/User/{userId}` - Retrieve progress for a specific user.
- **POST** `/Progress` - Add user progress.
- **PUT** `/Progress/{id}` - Update user progress.

### Settings Endpoints
- **GET** `/Setting` - Retrieve all user settings.
- **GET** `/Setting/{userId}` - Retrieve settings for a specific user.
- **POST** `/Setting` - Add user settings.
- **PUT** `/Setting/{userId}` - Update user settings.

---

## License
This project is licensed under the MIT License. See the LICENSE file for details.
