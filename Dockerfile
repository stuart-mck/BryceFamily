FROM microsoft/dotnet:2.1.300-sdk 

COPY ./ /app

WORKDIR /app/src

RUN ls -la

WORKDIR /app/src/BryceFamily.Web.MVC

RUN ls -la

RUN dotnet restore

RUN dotnet build -c Release

EXPOSE 3000

ENTRYPOINT ["dotnet", "run"]