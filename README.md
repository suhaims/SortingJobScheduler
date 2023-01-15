# SortingJobScheduler

## Description

SortingJobScheduler is a C# .NET Web API application developed to queue sorting jobs and track their progress. The solution is exposed via REST API, which makes it possible for clients to enqueue an array of numbers to be sorted in the background and to query the state of any previously enqueued job.

### Features

* The client can enqueue a new job, by providing an unsorted array of numbers as input
* The client can retrieve an overview of all jobs (pending, completed and failed jobs)
* The client can retrieve a specific job by its ID, including the output (sorted array) if the job has completed


## Technologies
* C#
* .NET Core


## APIs

### Sorting Jobs
| HTTP Verbs | Endpoints | Action |
| --- | --- | --- |
| GET | /api/sortingjob | To get all sorting job |
| GET | /api/sortingjob/{id} | To get a specific job |
| POST | /api/sortingjob | To create a new job |

### Schemas

#### SortingJob

```
{
	"id": string,
	"timestamp": datetime,
	"duration": number,
	"status": enum,
	"data": number[]
}
```

- `id`: The unique identifier of the job.
- `timestamp`: The date and time when the job was enqueued.
- `duration`: The time(in millisecond) taken to complete the execution of the job.
- `status`: The status of the job.
    - `1`: Pending, the job is still in queue.
    - `2`: Processing, the job is running.
	- `3`: Completed.
	- `4`: Failed.
- `data`: The sorted sequence if the status is `3`. Unsorted sequence if the status is `1` or `2`.


## Architectural Decisions

### Components

* An API interface for the clients to create sorting jobs and query the jobs. Implemented using the ASP.NET Core Web API.
* The `SortingJobQueue` service to enqueue and dequeue the sorting jobs. This service is using a ConcurrentQueue to ensure the thread safe enqueuing and dequeuing.
* The `SortingJobService` service to manage the sorting jobs, which read/write jobs to a dictionary. A ConcurrentDictionary is used to ensure the thread safe access. 
* A `SortingJobHostedService` service running in background, which reads the jobs one at a time from the queue and process the sorting operation. Used the .NET Core `Microsoft.Extensions.Hosting.BackgroundService` to implement it. 

The APIs are not authenticated, so any users can access the API. 
Added some time delays in the sorting operation, which allows the user to see the job status before the job completes.


## Usage

### Running the website locally
The application is build on C# and .NET Core. It can be executed either from Visual Studio/Visual Studio Code or from the command line by running the following command.

```cmd
dotnet build
dotnet run
```

The SwaggerUI is enabled, so the users can easily test the APIs from the launch page.

### Running Unit Tests

The unit tests can be executed from Visual Studio Test Explorer or by running the following command from the root directory.

```cmd
dotnet test
```
