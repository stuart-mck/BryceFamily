FROM microsoft/dotnet:2.0-sdk

COPY ./ /app

WORKDIR /app/src

RUN ls -la

WORKDIR /app/src/BryceFamily.Web.MVC

RUN ls -la


RUN dotnet restore

RUN dotnet build -c Release

EXPOSE 80

ENTRYPOINT ["dotnet", "run"]