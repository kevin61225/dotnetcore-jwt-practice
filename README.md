# JWT Authentication Example

## Objective

* Learn how to do authentication with `JWT`
* Learn how to do Logs with `ILogger`
* Learn how to use `Serilog` to make logs pretter
* Learn how to send log to `Seq` service

## Prerequisite

* Understand how ASP.NET Core pipline works
* Understand how to do `DI` in `Startup.cs`
* Understand what is `JWT` 
* Use Docker to install `Seq` before 

## Reference

### Install `Seq` from docker
```shell=
docker pull datalust/seq:latest
docker run -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest
```